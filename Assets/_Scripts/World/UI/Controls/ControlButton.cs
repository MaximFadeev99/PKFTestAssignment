using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using PKFTestAssignment.Utilities;
using System;
using PKFTestAssignment.Enums;

namespace PKFTestAssignment.World.Controls
{
    internal class ControlButton : MonoBehaviour
    {
        [SerializeField] private TMP_Text _textField;

        private Button _button;
        private Image _image;

        internal GameObject GameObject { get; private set; }
        internal CombatMoves CombatMove { get; private set; } = CombatMoves.None;

        internal Action<CombatMoves> Pressed;

        internal void Initialize(KeyValuePair<CombatMoves, Sprite> moveSpritePair) 
        {
            if (CombatMove != CombatMoves.None) 
            {
                CustomLogger.Log(nameof(ControlButton), "You can not call Initialize method more than once!",
                    MessageTypes.Assertion);
                return;
            }

            GameObject = gameObject;
            _button = GetComponent<Button>();
            _image = GetComponent<Image>();

            CombatMove = moveSpritePair.Key;
            _image.sprite = moveSpritePair.Value;
            _button.interactable = true;
            _textField.text = "";

            _button.onClick.AddListener(OnButtonPressed);
        }

        internal void ToggleDependingOnTurns(string turnsLeft) 
        {
            _button.interactable = turnsLeft == "0";
            _textField.text = turnsLeft != "0" ? turnsLeft : "";
        }

        internal void Disable() 
        {
            _button.interactable = false;
        }

        internal void Dispose()
        {
            _button.onClick.RemoveListener(OnButtonPressed);
        }

        private void OnButtonPressed() 
        {
            Pressed?.Invoke(CombatMove);
        }
    }
}