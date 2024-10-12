using PKFTestAssignment.Enums;
using PKFTestAssignment.World.Characters;
using PKFTestAssignment.World.Combat;
using System.Collections.Generic;

namespace PKFTestAssignment.RemoteServer.Simulation
{
    internal struct SimulationClone
    {
        internal string Name;
        internal int Health;
        internal int MaxHealth;
        internal CombatMovesDictionary CombatMovesDictionary;
        internal string[,] ActualMoveData;

        internal int BarrierAmountLeft;

        internal int OnFireDuration;
        internal int OnFireDamage;

        internal SimulationClone(CharacterData characterData)
        {
            Name = characterData.ID;
            Health = characterData.InitialHealth;
            MaxHealth = characterData.InitialHealth;
            CombatMovesDictionary = characterData.CombatMoves;
            ActualMoveData = new string[CombatMovesDictionary.Count, 3];

            foreach (KeyValuePair<CombatMoves, CombatMove> keyValuePair in CombatMovesDictionary)
            {
                int targetRow = keyValuePair.Key switch
                {
                    CombatMoves.IceShard => CombatOutcomeSimulator.IceShardRow,
                    CombatMoves.Fireball => CombatOutcomeSimulator.FireballRow,
                    CombatMoves.Barrier => CombatOutcomeSimulator.BarrierRow,
                    CombatMoves.Regeneration => CombatOutcomeSimulator.RegenerationRow,
                    CombatMoves.Cleansing => CombatOutcomeSimulator.CleansingRow,
                    _ => -1
                };

                ActualMoveData[targetRow, CombatOutcomeSimulator.NameColumn] = keyValuePair.Key.ToString();
                ActualMoveData[targetRow, CombatOutcomeSimulator.DurationColumn] = "0";
                ActualMoveData[targetRow, CombatOutcomeSimulator.CooldownColumn] = "0";
            }

            BarrierAmountLeft = 0;

            OnFireDuration = 0;
            OnFireDamage = 0;

            if (CombatMovesDictionary.ContainsKey(CombatMoves.Fireball))
            {
                Fireball fireball = (Fireball)CombatMovesDictionary[CombatMoves.Fireball];
                OnFireDamage = fireball.OvertimeDamage;
            }
        }
    }
}
