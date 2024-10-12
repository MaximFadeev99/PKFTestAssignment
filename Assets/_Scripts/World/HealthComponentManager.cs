using System.Collections.Generic;
using UnityEngine;
using PKFTestAssignment.World.Health;
using PKFTestAssignment.World.Characters;
using System;

namespace PKFTestAssignment.World
{
    [Serializable]
    public class HealthComponentManager
    {
        [SerializeField] private HealthComponent _healthComponentPrefab;
        [SerializeField] private Transform _canvasTransform;

        private readonly List<HealthComponent> _createdHealthComponents = new();

        public void DistributeHealthComponents(params Character[] characters) 
        {
            foreach (Character character in characters) 
            {
                HealthComponent newHealthComponent = GameObject.Instantiate(_healthComponentPrefab,
                    character.CurrentPosition + (Vector3)character.HealthComponentOffset, Quaternion.identity, 
                    _canvasTransform);

                newHealthComponent.Initialize();
                character.SetHealthComponent(newHealthComponent);
                _createdHealthComponents.Add(newHealthComponent);
            }
        }

        public void RemoveCreatedHealthComponents() 
        {
            while (_createdHealthComponents.Count > 0)
            {
                GameObject.Destroy(_createdHealthComponents[0].GameObject);
                _createdHealthComponents.Remove(_createdHealthComponents[0]);
            }
        }
    }
}