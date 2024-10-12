using PKFTestAssignment.Enums;
using PKFTestAssignment.World;
using PKFTestAssignment.World.Characters;
using PKFTestAssignment.World.Controls;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

namespace PKFTestAssignment.LocalClient
{
    public class Client : MonoBehaviour
    {
        public readonly int AccountID = 12345;

        [SerializeField] private TilePlacer _tilePlacer;
        [SerializeField] private Spawner _spawner;
        [SerializeField] private Grid _grid;
        [SerializeField] private ControlsPanel _controlsPanel;
        [SerializeField] private Button _restartButton;
        [SerializeField] private HealthComponentManager _healthComponentManager;

        private CombatManager _combatManager;
        private Enemy _enemy;
        private Player _player;

        public Action ConnectionRequired;
        public Action CombatRestartInquired;
        public Action AllCombatMovesPerformed;
        public Action<IReadOnlyDictionary<Character, CombatMoves>> CombatMovesSet;

        private void Awake()
        {
            _combatManager = new(_spawner);
            _spawner.Initalize();
        }

        private void OnEnable()
        {
            _combatManager.AllMovesPlayed += OnAllCombatMovesPlayed;
            _restartButton.onClick.AddListener(OnRestartButtonClick);
        }

        private void OnDisable()
        {
            _combatManager.AllMovesPlayed -= OnAllCombatMovesPlayed;
            _restartButton.onClick.RemoveListener(OnRestartButtonClick);
        }

        private void Start()
        {
            ConnectionRequired?.Invoke();
        }

        private void OnDestroy()
        {
            _controlsPanel.CombatMoveChosen -= OnPlayerMoveChosen;
        }

        public void BuildBattleScene(Vector2Int battleGridSize, 
            IReadOnlyDictionary<Type,Vector2Int> characterPositionDictionary,
            IReadOnlyDictionary<Type, CharacterData> characterDataDictionary) 
        {
            _tilePlacer.PlaceTiles(battleGridSize.x, battleGridSize.y);

            foreach (KeyValuePair<Type,Vector2Int> keyValuePair in characterPositionDictionary) 
            {
                Character newCharacter = _spawner.SpawnCharacter(keyValuePair.Key, characterDataDictionary[keyValuePair.Key], 
                    _grid.CellToWorld((Vector3Int)keyValuePair.Value));
                _healthComponentManager.DistributeHealthComponents(newCharacter);

                if (newCharacter is Player player) 
                {
                    _player = player;
                    _controlsPanel.Initialize(player.CharacterData.CombatMoves.Select(pair => pair.Key));
                    _controlsPanel.CombatMoveChosen += OnPlayerMoveChosen;
                    continue;
                }

                if (newCharacter is Enemy enemy) 
                {
                    _enemy = enemy;
                }
            }

            _restartButton.interactable = true;
        }

        public void SetHealth(string characterID, int newHealth) 
        {
            Character targetCharacter = GetCharacterByID(characterID);

            targetCharacter.SetCurrentHealth(newHealth);
        }

        public void ToggleStatusEffectIcon(string characterID, StatusEffects statusEffect, string turnsLeft) 
        {
            Character targetCharacter = GetCharacterByID(characterID);

            targetCharacter.ToggleStatusEffect(statusEffect, turnsLeft);
        }

        public void TurnOffBarrier(string characterID) 
        {
            Character targetCharacter = GetCharacterByID(characterID);

            _combatManager.TurnOffBarrier(targetCharacter.CurrentPosition);
        }

        public void ToggleControlButton(CombatMoves combatMove, string turnsLeft) 
        {
            _controlsPanel.ToggleControlButton(combatMove, turnsLeft);
        }

        public void SetAvailableMovesForEnemy(string id, IReadOnlyList<CombatMoves> availableMoves) 
        {
            if (_enemy.CharacterData.ID != id)
                return;

            _enemy.SetAvailableMoves(availableMoves);
        }

        public void PlayCombatMove(CombatMoves combatMove, Vector3 launchPosition, Vector2 launchDirection,
            LayerMask ignoredLayers)
        {
            _combatManager.PlayCombatMove(combatMove, launchPosition, launchDirection, ignoredLayers);
        }

        public void ReloadScene() 
        {
            _tilePlacer.RemovePlacedTile();
            Destroy(_enemy.GameObject);
            Destroy(_player.GameObject);
            _enemy = null;
            _player = null;
            _healthComponentManager.RemoveCreatedHealthComponents();
            _controlsPanel.DestroyCreatedControlButtons();
            _controlsPanel.CombatMoveChosen -= OnPlayerMoveChosen;
            _combatManager.TurnOffAllActiveMoves();
        }

        private void OnPlayerMoveChosen(CombatMoves combatMove) 
        {
            _controlsPanel.DisableAllButtons();
            Dictionary<Character, CombatMoves> characterMoveDictionary = new()
            {
                { _player, combatMove },
                { _enemy, _enemy.ChooseNextCombatMove() }
            };
            CombatMovesSet?.Invoke(characterMoveDictionary);
        }

        private void OnAllCombatMovesPlayed() 
        {
            AllCombatMovesPerformed?.Invoke();
        }

        private void OnRestartButtonClick() 
        {
            _restartButton.interactable = false;
            CombatRestartInquired?.Invoke();
        }

        private Character GetCharacterByID(string id) 
        {
            return id == _player.CharacterData.ID ? _player : _enemy;
        }       
    }
}