using PKFTestAssignment.Utilities;
using System;
using UnityEngine;

namespace PKFTestAssignment.World.Combat
{
    public abstract class CombatMove : MonoBehaviour, IMonoBehaviourPoolElement
    {
        [field: SerializeField] public int MainValue { get; private set; }
        [field: SerializeField] public int Duration { get; private set; }
        [field: SerializeField] public int CoolDown { get; private set; }

        public GameObject GameObject { get; private set; }
        protected Transform Transform { get; private set; }

        public Action<CombatMove> Performed;

        public virtual void Awake()
        {
            GameObject = gameObject;
            Transform = transform;
        }

        public virtual void Use() 
        {
            GameObject.SetActive(true);
        }

        public void SetWorldPosition(Vector3 worldPosition) 
        {
            Transform.position = worldPosition;
        }

        public void TurnOff()
        {
            GameObject.SetActive(false);
        }
    }
}