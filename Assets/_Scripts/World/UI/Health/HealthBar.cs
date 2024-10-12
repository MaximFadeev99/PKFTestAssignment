using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace PKFTestAssignment.World.Health
{
    internal class HealthBar : MonoBehaviour
    {
        [SerializeField] private GameObject _fill;
        [SerializeField] private TMP_Text _captionField;
        [SerializeField] private float _durationPerPoint = 0.02f;

        private Slider _slider;

        internal void Initialize(int maxValue) 
        {
            _slider = GetComponent<Slider>();
            _slider.maxValue = maxValue;
            _slider.value = maxValue;
            DrawCaption();
        }

        internal void SetCurrentValue(int currentValue) 
        {
            float tweenDuration = (_slider.value - currentValue) * _durationPerPoint;
            _slider
                .DOValue(currentValue, tweenDuration)
                .SetEase(Ease.Linear)
                .OnUpdate(DrawCaption)
                .OnComplete(TryDisableFill);
        }

        private void TryDisableFill() 
        {
            if (_slider.value <= 0) 
                _fill.SetActive(false);
        }

        private void DrawCaption() 
        {
            _captionField.text = $"{_slider.value}/{_slider.maxValue}";
        }
    }
}