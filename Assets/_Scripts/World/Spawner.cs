using PKFTestAssignment.Utilities;
using PKFTestAssignment.World.Characters;
using PKFTestAssignment.World.Combat;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace PKFTestAssignment.World
{
    public class Spawner : MonoBehaviour
    {
        [SerializeField] private List<Character> _characterPrefabs;
        [SerializeField] private List<CombatMove> _combatMovePrefabs;
        [SerializeField] private Transform _combatMoveContainer;

        private readonly List<MonoBehaviourPool<CombatMove>> _combatMovePools = new();

        public void Initalize()
        {
            foreach (CombatMove combatMove in _combatMovePrefabs) 
            {
                MonoBehaviourPool<CombatMove> newPool = new(combatMove, _combatMoveContainer, 3);
                _combatMovePools.Add(newPool);           
            }
        }

        public Character SpawnCharacter(Type characterType, CharacterData characterData, Vector3 spawnPosition) 
        {
            Character targetPrefab = _characterPrefabs.FirstOrDefault(prefab => prefab.GetType() == characterType);

            if (targetPrefab == null) 
            {
                CustomLogger.Log(nameof(Spawner), "You have tried to spawn a character that has not been added to the " +
                    "prefab list!", MessageTypes.Error);
                return null;
            }

            Character spawnedCharacter = Instantiate(targetPrefab, spawnPosition, Quaternion.identity);
            spawnedCharacter.Initialize(characterData);

            return spawnedCharacter;
        }

        public CombatMove SpawnCombatMove(Type combatMoveType,  Vector3 spawnPosition) 
        {
            MonoBehaviourPool<CombatMove> targetPool = _combatMovePools
                .FirstOrDefault(pool => pool.AllElements[0].GetType() == combatMoveType);

            if (targetPool == null) 
            {
                CustomLogger.Log(nameof(Spawner), "You have tried to spawn a combat move that has not been added to the " +
                    "prefab list!", MessageTypes.Error);
                return null;
            }

            CombatMove spawnedMove = targetPool.GetIdleElement();
            spawnedMove.SetWorldPosition(spawnPosition);

            return spawnedMove;
        }
    }
}