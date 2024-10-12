namespace PKFTestAssignment.RemoteServer.Commands
{
    public class SetHealthCommand : ServerCommand
    {
        public readonly string CharacterID;
        public readonly int CurrentHealth;

        public SetHealthCommand(string characterID, int currentHealth) 
        {
            CharacterID = characterID;
            CurrentHealth = currentHealth;
        }
    }
}