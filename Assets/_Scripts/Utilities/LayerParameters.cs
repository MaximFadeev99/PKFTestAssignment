using UnityEngine;

namespace PKFTestAssignment.Utilities
{
    public static class LayerParameters
    {
        public static readonly string Player = nameof(Player);
        public static readonly string Enemies = nameof(Enemies);
        public static readonly string Projectiles = nameof(Projectiles);

        static LayerParameters() 
        {
            CheckIfLayerExists(Player);
            CheckIfLayerExists(Enemies);
            CheckIfLayerExists(Projectiles);
        }

        private static void CheckIfLayerExists(string layerName) 
        {
            int check = LayerMask.NameToLayer(layerName);

            if (LayerMask.NameToLayer(layerName) == -1)
            {
                CustomLogger.Log(nameof(LayerParameters), $"Layer name {layerName} is not found among the existing " +
                    $"layers. Check the spelling!", MessageTypes.Error);
            }
        }
    }
}