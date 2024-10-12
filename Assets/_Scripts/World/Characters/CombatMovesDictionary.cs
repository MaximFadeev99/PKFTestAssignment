using PKFTestAssignment.Enums;
using PKFTestAssignment.Utilities;
using PKFTestAssignment.World.Combat;
using System;
using UnityEditor;

namespace PKFTestAssignment.World.Characters
{
    [Serializable]
    public class CombatMovesDictionary : SerializableDictionary<CombatMoves, CombatMove> { }

#if UNITY_EDITOR
    [CustomPropertyDrawer(typeof(CombatMovesDictionary))]
    public class CombatMoveSpriteDictionaryDrawer : DictionaryDrawer<CombatMoves, CombatMove> { }
#endif

}
