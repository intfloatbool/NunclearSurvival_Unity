using UnityEngine;

namespace NunclearGame.Items
{
    [System.Serializable]
    public class ItemValue
    {
        [SerializeField] private string _valueKey;
        public string ValueKey => _valueKey;

        [SerializeField] private int _value;
        public int Value => _value;
    }
}
