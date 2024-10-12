using PKFTestAssignment.Enums;
namespace PKFTestAssignment.RemoteServer.Commands
{
    public class ToggleControlsCommand : ServerCommand
    {
        public readonly CombatMoves CombatMove;
        public readonly string TurnsLeft;

        public ToggleControlsCommand(CombatMoves combatMove, string turnsLeft)
        {
            CombatMove = combatMove;
            TurnsLeft = turnsLeft;
        }
    }
}