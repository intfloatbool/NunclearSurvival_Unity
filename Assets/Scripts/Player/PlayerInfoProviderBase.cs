using UnityEngine;

namespace Player
{
    public abstract class PlayerInfoProviderBase : MonoBehaviour
    {
        public abstract string LoadPlayerName();
        public abstract void SetPlayerName(string freshName);
        public abstract PlayerInventory LoadInventory();

        public abstract string GetValue(string key);
        public abstract void SetValue(string key, string value);
    }
}
