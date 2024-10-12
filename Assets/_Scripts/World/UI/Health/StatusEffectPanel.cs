using PKFTestAssignment.Enums;
using PKFTestAssignment.Utilities;
using System.Collections.Generic;
using UnityEngine;

namespace PKFTestAssignment.World.Health
{
    internal class StatusEffectPanel : MonoBehaviour
    {
        [SerializeField] private EffectIconDictionary _effectIconDictionary;

        private bool _isInitialized = false;

        internal void Initialize() 
        {
            foreach (KeyValuePair<StatusEffects, StatusEffectIcon> keyValuePair in _effectIconDictionary) 
            {
                keyValuePair.Value.Initialize();
            }

            _isInitialized = true;
        }

        internal void ToggleIcon(StatusEffects targetEffect, string turnsLeft) 
        {
            if (_isInitialized == false) 
            {
                CustomLogger.Log(nameof(StatusEffectPanel), "You can not toggle icons on the panel until it is properly " +
                    $"initialized. You must call StatusEffectPanel.Initialize method first",
                    MessageTypes.Error);
                return;
            }

            if (_effectIconDictionary.ContainsKey(targetEffect) == false) 
            {
                CustomLogger.Log(nameof(StatusEffectPanel), $"You are trying to toggle an icon for an unregistered status " +
                    $"effect {targetEffect}. You must add this effect and an icon for it to the dictionary first",
                    MessageTypes.Warning);
                return;
            }

            _effectIconDictionary[targetEffect].SetActive(turnsLeft != "0");
            _effectIconDictionary[targetEffect].DrawDuration(turnsLeft);
        }
    }
}