using PKFTestAssignment.Enums;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace PKFTestAssignment.World.Characters
{
    public class Enemy : Character
    {
        private readonly List<CombatMoves> _availableMoves = new();

        public override void Initialize(CharacterData characterData)
        {
            base.Initialize(characterData);
            SetAvailableMoves(CharacterData.CombatMoves.Select(pair => pair.Key).ToList());
        }

        public void SetAvailableMoves(IReadOnlyList<CombatMoves> availableMoves) 
        {
            _availableMoves.Clear();

            foreach (CombatMoves move in availableMoves) 
                _availableMoves.Add(move);   
        }

        public CombatMoves ChooseNextCombatMove() 
        {
            return _availableMoves[Random.Range(0, _availableMoves.Count)];
        }
    }
}