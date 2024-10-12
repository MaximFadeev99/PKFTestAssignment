using PKFTestAssignment.Utilities;
using PKFTestAssignment.Enums;
using System;
using UnityEditor;
using UnityEngine;

namespace PKFTestAssignment.World.Controls 
{
    [Serializable]
    public class CombatMoveSpriteDictionary : SerializableDictionary<CombatMoves, Sprite> { }

#if UNITY_EDITOR
    [CustomPropertyDrawer(typeof(CombatMoveSpriteDictionary))]
    public class CombatMoveSpriteDictionaryDrawer : DictionaryDrawer<CombatMoves, Sprite> { }
#endif
}