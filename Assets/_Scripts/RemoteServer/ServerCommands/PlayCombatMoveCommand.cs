using PKFTestAssignment.Enums;
using UnityEngine;

namespace PKFTestAssignment.RemoteServer.Commands
{
    public class PlayCombatMoveCommand : ServerCommand
    {
        public readonly string PerformerName;
        public readonly CombatMoves CombatMove;
        public readonly Vector3 SpawnPosition;
        public readonly Vector2 LaunchDirection;
        public readonly LayerMask IgnoredLayers;

        public PlayCombatMoveCommand(string characterName, CombatMoves combatMove, Vector3 spawnPosition, 
            Vector2 launchDirection, LayerMask ignoredLayers) 
        {
            PerformerName = characterName;
            CombatMove = combatMove;
            SpawnPosition = spawnPosition;
            LaunchDirection = launchDirection;
            IgnoredLayers = ignoredLayers;
        }
    }
}
