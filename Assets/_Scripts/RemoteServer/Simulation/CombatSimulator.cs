using PKFTestAssignment.Enums;
using PKFTestAssignment.RemoteServer.Commands;
using PKFTestAssignment.Utilities;
using PKFTestAssignment.World.Characters;
using System.Collections.Generic;
using UnityEngine;

namespace PKFTestAssignment.RemoteServer.Simulation
{
    internal class CombatSimulator
    {
        private readonly List<PlayCombatMoveCommand> _latestCombatCommands = new();

        internal IReadOnlyList<PlayCombatMoveCommand> LatestCombatCommands => _latestCombatCommands;

        internal void SimulateCombat
            (Adapter targetAdapter, IReadOnlyDictionary<Character, CombatMoves> characterMoveDictionary)
        {
            _latestCombatCommands.Clear();

            foreach (KeyValuePair<Character, CombatMoves> keyValuePair in characterMoveDictionary)
            {               
                if (keyValuePair.Key is Player player)
                {
                    IssueCombatCommandForPlayer(targetAdapter, player, keyValuePair.Value);
                    continue;
                }

                if (keyValuePair.Key is Enemy enemy)
                {
                    IssueCombatCommandForEnemy(targetAdapter, enemy, keyValuePair.Value);
                    continue;
                }
            }
        }

        internal void Reset() 
        {
            _latestCombatCommands.Clear();
        }

        private void IssueCombatCommandForPlayer(Adapter adapter, Player player, CombatMoves combatMove) 
        {
            PlayCombatMoveCommand playCombatMoveCommand;

            switch (combatMove)
            {
                case CombatMoves.IceShard:
                    playCombatMoveCommand = new(player.CharacterData.ID, CombatMoves.IceShard, player.AttackPoint, 
                        Vector2.right, LayerMask.GetMask(LayerParameters.Player));
                    break;

                case CombatMoves.Fireball:
                    playCombatMoveCommand = new(player.CharacterData.ID, CombatMoves.Fireball, player.AttackPoint, 
                        Vector2.right, LayerMask.GetMask(LayerParameters.Player));
                    break;

                case CombatMoves.Barrier:
                    playCombatMoveCommand = new(player.CharacterData.ID, CombatMoves.Barrier, player.CurrentPosition, 
                        Vector2.zero, 0);
                    break;

                case CombatMoves.Regeneration:
                    playCombatMoveCommand = new(player.CharacterData.ID, CombatMoves.Regeneration, player.CurrentPosition, 
                        Vector2.zero, 0);
                    break;

                case CombatMoves.Cleansing:
                    playCombatMoveCommand = new(player.CharacterData.ID, CombatMoves.Cleansing, player.CurrentPosition, 
                        Vector2.zero, 0);
                    break;

                default:
                    playCombatMoveCommand = null;
                    CustomLogger.Log(nameof(Server), $"Simulation for combat move {combatMove} is not " +
                        $"implemented! Implementation is required!", MessageTypes.Error);
                    break;
            }

            _latestCombatCommands.Add(playCombatMoveCommand);
            adapter.ProcessServerCommand(playCombatMoveCommand);
        }

        private void IssueCombatCommandForEnemy(Adapter adapter, Enemy enemy, CombatMoves combatMove) 
        {
            PlayCombatMoveCommand playCombatMoveCommand;

            switch (combatMove)
            {
                case CombatMoves.IceShard:
                    playCombatMoveCommand = new(enemy.CharacterData.ID, CombatMoves.IceShard, enemy.AttackPoint, 
                        Vector2.left, LayerMask.GetMask(LayerParameters.Enemies));
                    break;

                case CombatMoves.Fireball:
                    playCombatMoveCommand = new(enemy.CharacterData.ID, CombatMoves.Fireball, enemy.AttackPoint, 
                        Vector2.left, LayerMask.GetMask(LayerParameters.Enemies));
                    break;

                case CombatMoves.Barrier:
                    playCombatMoveCommand = new(enemy.CharacterData.ID, CombatMoves.Barrier, enemy.CurrentPosition, 
                        Vector2.zero, 0);
                    break;

                case CombatMoves.Regeneration:
                    playCombatMoveCommand = new(enemy.CharacterData.ID, CombatMoves.Regeneration, enemy.CurrentPosition, 
                        Vector2.zero, 0);
                    break;

                case CombatMoves.Cleansing:
                    playCombatMoveCommand = new(enemy.CharacterData.ID, CombatMoves.Cleansing, enemy.CurrentPosition, 
                        Vector2.zero, 0);
                    break;

                default:
                    playCombatMoveCommand = null;
                    CustomLogger.Log(nameof(Server), $"Simulation for combat move {combatMove} is not " +
                        $"implemented! Implementation is required!", MessageTypes.Error);
                    break;
            }

            _latestCombatCommands.Add(playCombatMoveCommand);
            adapter.ProcessServerCommand(playCombatMoveCommand);
        }
    }
}
