using PKFTestAssignment.Utilities;
using UnityEngine;

namespace PKFTestAssignment.World.Combat
{
    public abstract class Projectile : CombatMove
    {
        [SerializeField] private float _flyingSpeed;

        private Rigidbody2D _rigidbody;
        private Collider2D _collider;
        private Vector2 _flyingDirection = Vector2.zero;
        private bool _isFlying = false;

        public override void Awake()
        {
            base.Awake();
            _rigidbody = GetComponent<Rigidbody2D>();
            _collider = GetComponent<Collider2D>();
        }

        public override void Use()
        {
            base.Use();

            if (_flyingDirection == Vector2.zero)
            {
                CustomLogger.Log(GameObject.name, "You can not use a projectile until its Preset method called",
                    MessageTypes.Assertion);
                return;
            }

            _isFlying = true;
        }

        public void Preset(Vector2 normalizedDirection, LayerMask layersToExclude)
        {
            _flyingDirection = normalizedDirection;
            _collider.excludeLayers = layersToExclude;
        }

        protected virtual void OnHitCollider()     
        {
            _isFlying = false;
            _flyingDirection = Vector2.zero;
            _collider.excludeLayers = 0;
            GameObject.SetActive(false);
            Performed?.Invoke(this);
        }

        private void Update()
        {
            if (_isFlying == false)
                return;

            _rigidbody.velocity = _flyingDirection * _flyingSpeed;
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            OnHitCollider();
        }
    }
}