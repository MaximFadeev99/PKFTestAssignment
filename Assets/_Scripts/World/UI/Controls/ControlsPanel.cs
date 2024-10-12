using PKFTestAssignment.Enums;
using PKFTestAssignment.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace PKFTestAssignment.World.Controls
{
    public class ControlsPanel : MonoBehaviour
    {
        [SerializeField] private CombatMoveSpriteDictionary _moveSpriteDictionary;
        [SerializeField] private ControlButton _controlButtonPrefab;

        private readonly List<ControlButton> _createdControlButtons = new();

        private Transform _transform;

        public Action<CombatMoves> CombatMoveChosen;

        public void Initialize(IEnumerable<CombatMoves> combatMoves)
        {
            _transform = transform;

            foreach (CombatMoves combatMove in combatMoves) 
            {
                if (_moveSpriteDictionary.ContainsKey(combatMove) == false) 
                {
                    CustomLogger.Log(nameof(ControlsPanel), $"You are trying to draw a button for unknown " +
                        $"combat move {combatMove}. You must first register it in {nameof(_moveSpriteDictionary)}",
                        MessageTypes.Error);
                    continue;
                }

                ControlButton newControlButton = Instantiate(_controlButtonPrefab, _transform);
                newControlButton.Initialize(_moveSpriteDictionary.First(pair => pair.Key == combatMove));
                newControlButton.Pressed += OnControlButtonPressed;
                _createdControlButtons.Add(newControlButton);
            }
        }

        public void ToggleControlButton(CombatMoves combatMove, string turnsLeft) 
        {
            ControlButton targetButton = _createdControlButtons.First(button => button.CombatMove == combatMove);

            targetButton.ToggleDependingOnTurns(turnsLeft);
        }

        public void DisableAllButtons() 
        {
            foreach (ControlButton button in _createdControlButtons) 
                button.Disable();
        }

        public void DestroyCreatedControlButtons() 
        {
            OnDestroy();

            while (_createdControlButtons.Count > 0)
            {
                Destroy(_createdControlButtons[0].GameObject);
                _createdControlButtons.Remove(_createdControlButtons[0]);
            }
        }

        private void OnDestroy()
        {
            foreach (ControlButton createdButton in _createdControlButtons) 
            {
                createdButton.Pressed -= OnControlButtonPressed;
                createdButton.Dispose();
            }
        }

        private void OnControlButtonPressed(CombatMoves chosenMove) 
        {
            CombatMoveChosen?.Invoke(chosenMove);
        }
    }
}