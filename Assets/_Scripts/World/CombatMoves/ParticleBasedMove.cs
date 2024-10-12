using Cysharp.Threading.Tasks;
using UnityEngine;

namespace PKFTestAssignment.World.Combat
{
    public class ParticleBasedMove : CombatMove
    {
        private ParticleSystem _particleSystem;

        public override void Awake()
        {
            base.Awake();
            _particleSystem = GetComponent<ParticleSystem>();
        }

        public async override void Use()
        {
            base.Use();
            await UniTask.WaitForSeconds(_particleSystem.main.duration);
            Performed?.Invoke(this);
        }
    }
}