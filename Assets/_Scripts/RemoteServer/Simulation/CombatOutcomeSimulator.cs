using PKFTestAssignment.Enums;
using PKFTestAssignment.RemoteServer.Commands;
using PKFTestAssignment.World.Characters;
using System;
using System.Collections.Generic;
using System.Linq;

namespace PKFTestAssignment.RemoteServer.Simulation
{
    internal class CombatOutcomeSimulator 
    {
        internal const int IceShardRow = 0;
        internal const int FireballRow = 1;
        internal const int BarrierRow = 2;
        internal const int RegenerationRow = 3;
        internal const int CleansingRow = 4;

        internal const int NameColumn = 0;
        internal const int DurationColumn = 1;
        internal const int CooldownColumn = 2;

        private readonly List<SimulationClone> _activeClones = new();

        internal Action<Adapter> GameOver;

        internal void CreateSimulationClones(params CharacterData[] characterData) 
        {
            foreach (CharacterData datum in characterData) 
            {
                SimulationClone newClone = new(datum);
                _activeClones.Add(newClone);
            }
        }

        internal void SimulateCombatOutcome(Adapter adapter, IReadOnlyList<PlayCombatMoveCommand> latestCombatCommands) 
        {
            ApplyDirectEffects(latestCombatCommands);
            ApplyIndirectEffects(adapter);
            SynchronizeHealth(adapter);

            if (_activeClones.Any(clone => clone.Health <= 0))
                GameOver?.Invoke(adapter);

            SynchronizeStatusEffectIcons(adapter);
            SynchronizeControls(adapter);
            SynchronizeAvailableCombatMoves(adapter);
        }

        internal void Reset() 
        {
            _activeClones.Clear();
        }

        private void ApplyDirectEffects(IReadOnlyList<PlayCombatMoveCommand> latestCombatCommands) 
        {
            foreach (PlayCombatMoveCommand command in latestCombatCommands)
            {
                SimulationClone performer = _activeClones.First(clone => clone.Name == command.PerformerName);
                SimulationClone enemy = _activeClones.First(clone => clone.Name != command.PerformerName);

                switch (command.CombatMove)
                {
                    case CombatMoves.IceShard:
                        performer.ActualMoveData[IceShardRow, CooldownColumn] =
                            (performer.CombatMovesDictionary[CombatMoves.IceShard].CoolDown + 1).ToString();
                        enemy = ApplyDamage(performer, enemy, CombatMoves.IceShard);
                        break;

                    case CombatMoves.Fireball:
                        performer.ActualMoveData[FireballRow, CooldownColumn] =
                            (performer.CombatMovesDictionary[CombatMoves.Fireball].CoolDown + 1).ToString();
                        enemy.OnFireDuration = performer.CombatMovesDictionary[CombatMoves.Fireball].Duration;
                        enemy = ApplyDamage(performer, enemy, CombatMoves.Fireball);
                        break;

                    case CombatMoves.Barrier:
                        performer.ActualMoveData[BarrierRow, CooldownColumn] =
                            (performer.CombatMovesDictionary[CombatMoves.Barrier].CoolDown + 1).ToString();
                        performer.ActualMoveData[BarrierRow, DurationColumn] =
                            performer.CombatMovesDictionary[CombatMoves.Barrier].Duration.ToString();
                        performer.BarrierAmountLeft = performer.CombatMovesDictionary[CombatMoves.Barrier].MainValue;
                        break;

                    case CombatMoves.Regeneration:
                        performer.ActualMoveData[RegenerationRow, CooldownColumn] =
                            (performer.CombatMovesDictionary[CombatMoves.Regeneration].CoolDown + 1).ToString();
                        performer.ActualMoveData[RegenerationRow, DurationColumn] =
                            performer.CombatMovesDictionary[CombatMoves.Regeneration].Duration.ToString();
                        break;

                    case CombatMoves.Cleansing:
                        performer.OnFireDuration = 0;
                        performer.ActualMoveData[CleansingRow, CooldownColumn] =
                            (performer.CombatMovesDictionary[CombatMoves.Cleansing].CoolDown + 1).ToString();
                        break;

                }

                SubstituteClone(performer);
                SubstituteClone(enemy);
            }
        }

        private void ApplyIndirectEffects(Adapter adapter) 
        {
            for (int i = _activeClones.Count - 1; i >= 0; i--)
            {
                SimulationClone newClone = _activeClones[i];

                if (newClone.OnFireDuration > 0)
                {
                    newClone.Health -= newClone.OnFireDamage;
                    newClone.OnFireDuration--;
                }

                if (newClone.ActualMoveData[RegenerationRow, DurationColumn] != "0")
                {
                    int intDuration = Convert.ToInt32(newClone.ActualMoveData[RegenerationRow, DurationColumn]) - 1;

                    newClone.ActualMoveData[RegenerationRow, DurationColumn] = intDuration.ToString();
                    newClone.Health += newClone.CombatMovesDictionary[CombatMoves.Regeneration].MainValue;

                    if (newClone.Health > newClone.MaxHealth)
                        newClone.Health = newClone.MaxHealth;
                }

                if (newClone.ActualMoveData[BarrierRow, DurationColumn] != "0" && newClone.BarrierAmountLeft > 0)
                {
                    int intDuration = Convert.ToInt32(newClone.ActualMoveData[BarrierRow, DurationColumn]) - 1;

                    newClone.ActualMoveData[BarrierRow, DurationColumn] = intDuration.ToString();
                }
                else if ((newClone.ActualMoveData[BarrierRow, DurationColumn] != "0" && newClone.BarrierAmountLeft <= 0) ||
                        newClone.ActualMoveData[BarrierRow, DurationColumn] == "0")
                {
                    newClone.BarrierAmountLeft = 0;
                    newClone.ActualMoveData[BarrierRow, DurationColumn] = "0";
                    adapter.ProcessServerCommand(new TurnOffBarrierCommand(newClone.Name));
                }

                SubstituteClone(newClone);
            }
        }

        private void SynchronizeHealth(Adapter adapter) 
        {
            foreach (SimulationClone clone in _activeClones)
                adapter.ProcessServerCommand(new SetHealthCommand(clone.Name, clone.Health));
        }

        private void SynchronizeStatusEffectIcons(Adapter adapter) 
        {
            foreach (SimulationClone clone in _activeClones)
            {
                adapter.ProcessServerCommand(new ToggleIconCommand
                    (clone.Name, StatusEffects.OnFire, clone.OnFireDuration.ToString()));
                adapter.ProcessServerCommand(new ToggleIconCommand
                    (clone.Name, StatusEffects.Regenerating, clone.ActualMoveData[RegenerationRow, DurationColumn]));
                adapter.ProcessServerCommand(new ToggleIconCommand
                    (clone.Name, StatusEffects.Shielded, clone.ActualMoveData[BarrierRow, DurationColumn]));
            }
        }

        private void SynchronizeControls(Adapter adapter) 
        {
            SimulationClone playerClone = _activeClones.First(clone => clone.Name == "Player");

            for (int i = 0; i < playerClone.ActualMoveData.GetLength(0); i++)
            {
                CombatMoves combatMove = i switch
                {
                    IceShardRow => CombatMoves.IceShard,
                    FireballRow => CombatMoves.Fireball,
                    BarrierRow => CombatMoves.Barrier,
                    RegenerationRow => CombatMoves.Regeneration,
                    CleansingRow => CombatMoves.Cleansing,
                    _ => CombatMoves.None
                };

                if (playerClone.ActualMoveData[i, CooldownColumn] != "0")
                {
                    int updatedCooldown = Convert.ToInt32(playerClone.ActualMoveData[i, CooldownColumn]) - 1;

                    playerClone.ActualMoveData[i, CooldownColumn] = updatedCooldown.ToString();
                    adapter.ProcessServerCommand(new ToggleControlsCommand
                        (combatMove, playerClone.ActualMoveData[i, CooldownColumn]));
                }
                else 
                {
                    adapter.ProcessServerCommand(new ToggleControlsCommand(combatMove, "0"));
                }
            }

            SubstituteClone(playerClone);
        }

        private void SynchronizeAvailableCombatMoves(Adapter adapter) 
        {
            List<SimulationClone> enemies = _activeClones.Where(clone => clone.Name != "Player").ToList();

            for (int i = enemies.Count - 1; i >= 0; i--)
            {
                SimulationClone currentClone = enemies[i];
                List<CombatMoves> availableMoves = new();

                for (int j = 0; j < currentClone.ActualMoveData.GetLength(0); j++)
                {
                    CombatMoves combatMove = j switch
                    {
                        IceShardRow => CombatMoves.IceShard,
                        FireballRow => CombatMoves.Fireball,
                        BarrierRow => CombatMoves.Barrier,
                        RegenerationRow => CombatMoves.Regeneration,
                        CleansingRow => CombatMoves.Cleansing,
                        _ => CombatMoves.None
                    };

                    int currentCooldown = Convert.ToInt32(currentClone.ActualMoveData[j, CooldownColumn]);

                    if (currentCooldown > 0)
                    {
                        currentClone.ActualMoveData[j, CooldownColumn] = (currentCooldown - 1).ToString();
                    }
                    else if (currentCooldown < 0) 
                    {
                        currentClone.ActualMoveData[j, CooldownColumn] = "0";
                    }

                    if (currentClone.ActualMoveData[j, CooldownColumn] == "0")
                        availableMoves.Add(combatMove);
                }

                SubstituteClone(currentClone);
                adapter.ProcessServerCommand(new SetAvailableMovesCommand(currentClone.Name, availableMoves));
            }
        }

        private SimulationClone ApplyDamage(SimulationClone performer, SimulationClone enemy, CombatMoves combatMove) 
        {
            int incomingDamage = performer.CombatMovesDictionary[combatMove].MainValue;

            if (enemy.BarrierAmountLeft == 0)
            {
                enemy.Health -= incomingDamage;
            }
            else
            {
                if (enemy.BarrierAmountLeft > incomingDamage)
                {
                    enemy.BarrierAmountLeft -= incomingDamage;
                }
                else if (enemy.BarrierAmountLeft == incomingDamage)
                {
                    enemy.BarrierAmountLeft = 0;
                }
                else if (enemy.BarrierAmountLeft < incomingDamage)
                {
                    incomingDamage -= enemy.BarrierAmountLeft;
                    enemy.BarrierAmountLeft = 0;
                    enemy.Health -= incomingDamage;
                }
            }

            return enemy;
        }

        private void SubstituteClone(SimulationClone newClone) 
        {
            SimulationClone deletedClone = _activeClones.First(clone => clone.Name == newClone.Name);

            _activeClones.Remove(deletedClone);
            _activeClones.Add(newClone);
        }
    }   
}
