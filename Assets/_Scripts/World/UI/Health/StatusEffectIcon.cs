using TMPro;
using UnityEngine;

namespace PKFTestAssignment.World.Health
{
    public class StatusEffectIcon : MonoBehaviour
    {
        [SerializeField] private TMP_Text _captionField;

        private GameObject _gameObject;

        internal void Initialize() 
        {
            _gameObject = gameObject;
        }

        internal void SetActive(bool isActive) 
        {
            _gameObject.SetActive(isActive);
        }

        internal void DrawDuration(string turnsLeft) 
        {
            _captionField.text = turnsLeft;
        }
    }
}