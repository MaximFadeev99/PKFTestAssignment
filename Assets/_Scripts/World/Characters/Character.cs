using PKFTestAssignment.Enums;
using PKFTestAssignment.Utilities;
using PKFTestAssignment.World.Health;
using UnityEngine;

namespace PKFTestAssignment.World.Characters
{
    public abstract class Character : MonoBehaviour
    {
        [SerializeField] private Transform _attackPointTransform;
        [Tooltip("When instantiated, the HealthComponent for this Character will be offset from its center " +
            "by this value")]
        [SerializeField] private Vector2 _healthComponentOffset;

        private HealthComponent _healthComponent;

        public CharacterData CharacterData { get; private set; }
        public GameObject GameObject { get; private set; }
        protected BoxCollider BoxCollider { get; private set; }
        protected Transform Transform { get; private set; }

        public Vector3 CurrentPosition => Transform.position;
        public Vector3 AttackPoint => _attackPointTransform.position;
        public Vector2 HealthComponentOffset => _healthComponentOffset;

        public virtual void Initialize(CharacterData characterData) 
        {
            GameObject = gameObject;
            Transform = transform;
            CharacterData = characterData;
            BoxCollider = GetComponent<BoxCollider>();
        }

        public void SetHealthComponent(HealthComponent healthComponent) 
        {
            if (_healthComponent != null) 
            {
                CustomLogger.Log(GameObject.name, "There has been an attempt to reset HealthComponent. " +
                    "It is not allowed!", MessageTypes.Error);
                return;
            }

            _healthComponent = healthComponent;
        }

        public void SetCurrentHealth(int currentHealth) 
        {
            _healthComponent.SetCurrentHealth(currentHealth);
        }

        public void ToggleStatusEffect(StatusEffects targetEffect, string turnsLeft) 
        {
            _healthComponent.ToggleStatusEffect(targetEffect, turnsLeft);
        }
    }
}