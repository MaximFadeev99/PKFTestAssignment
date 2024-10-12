using PKFTestAssignment.Enums;
using PKFTestAssignment.World;
using PKFTestAssignment.World.Combat;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace PKFTestAssignment.LocalClient
{
    internal class CombatManager
    {
        private readonly Spawner _spawner;
        private readonly List<CombatMove> _activeMoves = new();
        private readonly List<Barrier> _activeBarriers = new();

        internal Action AllMovesPlayed;

        internal CombatManager(Spawner spawner)
        {
            _spawner = spawner;
        }

        internal void PlayCombatMove(CombatMoves combatMove, Vector2 position, 
            Vector2 normalizedDirection, LayerMask layersToExclude)
        {
            Type combatMoveType = combatMove switch
            {
                CombatMoves.Fireball => typeof(Fireball),
                CombatMoves.IceShard => typeof(IceShard),
                CombatMoves.Barrier => typeof(Barrier),
                CombatMoves.Regeneration => typeof(Regeneration),
                CombatMoves.Cleansing => typeof(Cleansing),
                _ => null,
            };
            CombatMove spawnedMove = _spawner.SpawnCombatMove(combatMoveType, position);

            if (spawnedMove is Projectile spawnedProjectile)
            {
                spawnedProjectile.Preset(normalizedDirection, layersToExclude);
            }

            spawnedMove.Performed += OnCombatMovePerformed;
            _activeMoves.Add(spawnedMove);

            if (spawnedMove is Barrier spawnedBarrier)
                _activeBarriers.Add(spawnedBarrier);

            spawnedMove.Use();
        }

        internal void TurnOffBarrier(Vector2 position) 
        {
            Barrier targetBarrier = _activeBarriers
                .FirstOrDefault(barrier => barrier.CurrentPosition == position);

            if (targetBarrier == null) 
                return;

            targetBarrier.TurnOff();
            _activeBarriers.Remove(targetBarrier);
        }

        internal void TurnOffAllActiveMoves() 
        {
            while (_activeBarriers.Count > 0)
            {
                _activeBarriers[0].TurnOff();
                _activeBarriers.Remove(_activeBarriers[0]);
            }

            while (_activeMoves.Count > 0)
            {
                _activeMoves[0].TurnOff();
                _activeMoves.Remove(_activeMoves[0]);
            }
        }

        private void OnCombatMovePerformed(CombatMove performedMove) 
        {
            performedMove.Performed -= OnCombatMovePerformed;
            _activeMoves.Remove(performedMove);

            if (_activeMoves.Count == 0)
                AllMovesPlayed?.Invoke();
        }
    }
}