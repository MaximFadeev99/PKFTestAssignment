using Cysharp.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PKFTestAssignment.World.Combat
{
    public class Barrier : CombatMove
    {
        public Vector2 CurrentPosition => Transform.position;

        public async override void Use()
        {
            base.Use();
            await UniTask.WaitForSeconds(1f);
            Performed?.Invoke(this);
        }
    }
}