using Cysharp.Threading.Tasks;
using PKFTestAssignment.Enums;
using PKFTestAssignment.LocalClient;
using PKFTestAssignment.RemoteServer.Commands;
using PKFTestAssignment.World.Characters;
using System.Collections.Generic;
using UnityEngine;

namespace PKFTestAssignment.RemoteServer
{
    public class Adapter : MonoBehaviour
    {
        [SerializeField] private Server _server;
        [SerializeField] private Client _client;

        private void OnEnable()
        {
            _client.ConnectionRequired += ConnectToServer;
            _client.CombatMovesSet += OnCombatMovesSet;
            _client.AllCombatMovesPerformed += OnAllCombatMovesPerformed;
            _client.CombatRestartInquired += OnCombatRestartRequested;
        }

        private void OnDisable()
        {
            _client.ConnectionRequired -= ConnectToServer;
            _client.CombatMovesSet -= OnCombatMovesSet;
            _client.AllCombatMovesPerformed -= OnAllCombatMovesPerformed;
            _client.CombatRestartInquired -= OnCombatRestartRequested;
        }

        public void ProcessServerCommand(ServerCommand command)
        {
            if (command is BuildBattleSceneCommand buildBattleSceneCommand)
            {
                _client.BuildBattleScene(buildBattleSceneCommand.BattleGridSize,
                    buildBattleSceneCommand.CharacterPositionDictionary,
                    buildBattleSceneCommand.CharacterDataDictionary);
                return;
            }

            if (command is PlayCombatMoveCommand playCombatMoveCommand)
            {
                _client.PlayCombatMove(playCombatMoveCommand.CombatMove, playCombatMoveCommand.SpawnPosition,
                    playCombatMoveCommand.LaunchDirection, playCombatMoveCommand.IgnoredLayers);
                return;
            }

            if (command is SetHealthCommand setHealthCommand)
            {
                _client.SetHealth(setHealthCommand.CharacterID, setHealthCommand.CurrentHealth);
                return;
            }

            if (command is ToggleIconCommand toggleIconCommand)
            {
                _client.ToggleStatusEffectIcon(toggleIconCommand.CharacterID, toggleIconCommand.StatusEffect,
                    toggleIconCommand.TurnsLeft);
                return;
            }

            if (command is ToggleControlsCommand toggleControlsCommand)
            {
                _client.ToggleControlButton(toggleControlsCommand.CombatMove, toggleControlsCommand.TurnsLeft);
                return;
            }

            if (command is SetAvailableMovesCommand setAvailableMovesCommand)
            {
                _client.SetAvailableMovesForEnemy(setAvailableMovesCommand.CharacterID,
                    setAvailableMovesCommand.AvailableMoves);
                return;
            }

            if (command is TurnOffBarrierCommand turnOffBarrierCommand)
            {
                _client.TurnOffBarrier(turnOffBarrierCommand.CharacterID);
                return;
            }

            if (command is ReloadCombatCommand)
            {
                _client.ReloadScene();
                return;
            }
        }

        private async void ConnectToServer() 
        {
            await UniTask.WaitUntil(() => _server.IsWorking == true);
            _server.ConnectClient(_client.AccountID, this);
        }

        private void OnCombatMovesSet(IReadOnlyDictionary<Character, CombatMoves> characterMoveDictionary) 
        {
            _server.SimulateCombat(_client.AccountID, characterMoveDictionary);
        }

        private void OnAllCombatMovesPerformed() 
        {
            _server.SimulateCombatOutcome(_client.AccountID);
        }

        private void OnCombatRestartRequested() 
        {
            _server.RestartCombat(_client.AccountID);
        }       
    }
}