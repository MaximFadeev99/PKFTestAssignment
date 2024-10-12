using UnityEngine;

namespace PKFTestAssignment.Utilities
{
    public static class CustomLogger
    {
        public static void Log(string scriptName, string message, MessageTypes messageType) 
        {
            string compiledMessage = $"{scriptName}: {message}";

            switch (messageType) 
            {
                case MessageTypes.Message:
                    Debug.Log(compiledMessage);
                    break;

                case MessageTypes.Warning:
                    Debug.LogWarning(compiledMessage);
                    break;

                case MessageTypes.Assertion:
                    Debug.LogAssertion(compiledMessage);
                    break;

                case MessageTypes.Error:
                    Debug.LogError(compiledMessage);
                    break;
            }
        }
    }

    public enum MessageTypes 
    {
        Message, 
        Warning,
        Assertion,
        Error
    }
}