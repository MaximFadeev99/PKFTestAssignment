using UnityEngine;

namespace PKFTestAssignment.World.Characters
{
    [CreateAssetMenu(fileName = "NewCharacterData", menuName = "ProjectData/CharacterData", order = 51)]
    public class CharacterData : ScriptableObject
    {
        [field: SerializeField] public string ID { get; private set; }
        [field: SerializeField] public int InitialHealth { get; private set; }
        [field: SerializeField] public CombatMovesDictionary CombatMoves { get; private set; }
    }
}
