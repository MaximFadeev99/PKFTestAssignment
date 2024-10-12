using PKFTestAssignment.Enums;
using UnityEngine;

namespace PKFTestAssignment.World.Health
{
    public class HealthComponent : MonoBehaviour
    {
        [SerializeField] private StatusEffectPanel _statusEffectPanel;
        [SerializeField] private HealthBar _healthBar;
        [SerializeField] private int InitialHealth = 50;

        public GameObject GameObject { get; private set; }

        public void Initialize() 
        {
            GameObject = gameObject;
            _healthBar.Initialize(InitialHealth);
            _statusEffectPanel.Initialize();
        }

        public void SetCurrentHealth(int currentHealth) 
        {
            _healthBar.SetCurrentValue(currentHealth);
        }

        public void ToggleStatusEffect(StatusEffects targetEffect, string turnsLeft) 
        {
            _statusEffectPanel.ToggleIcon(targetEffect, turnsLeft);
        }
    }
}