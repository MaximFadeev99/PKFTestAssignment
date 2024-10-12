using UnityEngine;

namespace PKFTestAssignment.World.Combat
{
    public class Fireball : Projectile
    {
        [field: SerializeField] public int OvertimeDamage { get; private set; } = 1;
    }
}