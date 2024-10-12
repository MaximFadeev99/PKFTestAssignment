using PKFTestAssignment.Utilities;
using PKFTestAssignment.Enums;
using System;
using UnityEditor;

namespace PKFTestAssignment.World.Health 
{
    [Serializable]
    public class EffectIconDictionary : SerializableDictionary<StatusEffects, StatusEffectIcon> { }

#if UNITY_EDITOR
    [CustomPropertyDrawer(typeof(EffectIconDictionary))]
    public class EffectIconDictionaryDrawer : DictionaryDrawer<StatusEffects, StatusEffectIcon> { }
#endif
}