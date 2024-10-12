using PKFTestAssignment.Enums;

namespace PKFTestAssignment.RemoteServer.Commands
{
    public class ToggleIconCommand : ServerCommand
    {
        public readonly string CharacterID;
        public readonly StatusEffects StatusEffect;
        public readonly string TurnsLeft;

        public ToggleIconCommand(string characterID, StatusEffects statusEffect, string turnsLeft)
        {
            CharacterID = characterID;
            StatusEffect = statusEffect;
            TurnsLeft = turnsLeft;
        }
    }
}