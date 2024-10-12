using PKFTestAssignment.Enums;
using System.Collections.Generic;

namespace PKFTestAssignment.RemoteServer.Commands
{
    public class SetAvailableMovesCommand : ServerCommand
    {
        public readonly string CharacterID;
        public readonly IReadOnlyList<CombatMoves> AvailableMoves;

        public SetAvailableMovesCommand(string characterID, IReadOnlyList<CombatMoves> availableMoves)
        {
            CharacterID = characterID;
            AvailableMoves = availableMoves;
        }
    }
}
