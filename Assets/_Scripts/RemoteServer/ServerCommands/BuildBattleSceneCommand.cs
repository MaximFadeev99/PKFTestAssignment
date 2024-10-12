using PKFTestAssignment.World;
using PKFTestAssignment.World.Characters;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace PKFTestAssignment.RemoteServer.Commands
{
    public class BuildBattleSceneCommand : ServerCommand
    {
        public readonly Vector2Int BattleGridSize = new (3, 3);
        public readonly IReadOnlyDictionary<Type, Vector2Int> CharacterPositionDictionary = 
            new Dictionary<Type, Vector2Int>() 
        {
            { typeof(Player), Vector2Int.left },
            { typeof(Swordsman), Vector2Int.right }
        };

        public readonly IReadOnlyDictionary<Type, CharacterData> CharacterDataDictionary;

        public BuildBattleSceneCommand(IReadOnlyDictionary<Type, CharacterData> characterDataDictionary) 
        {
            CharacterDataDictionary = characterDataDictionary;
        }
    }
}
