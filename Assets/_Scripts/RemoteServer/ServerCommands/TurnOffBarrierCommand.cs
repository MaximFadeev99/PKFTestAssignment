namespace PKFTestAssignment.RemoteServer.Commands
{
    public class TurnOffBarrierCommand : ServerCommand
    {
        public readonly string CharacterID;

        public TurnOffBarrierCommand(string charatcerID)
        {
            CharacterID = charatcerID;
        }
    }
}