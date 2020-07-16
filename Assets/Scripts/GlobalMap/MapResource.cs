using SingletonsPreloaders;
using UnityEngine;

public class MapResource : MonoBehaviour
{
    
    [SerializeField] private ItemName _resourceName;
    public ItemName ResourceName => _resourceName;

    [SerializeField] private int _amount = 1;
    public int Amount => _amount;

    private bool _isTriggered;

    private void OnTriggerEnter(Collider other)
    {
        if (_isTriggered)
            return;
        Debug.Log("On Trigger enter!!! Parent: " + other.transform.parent.gameObject.name);
        var playerGlobal = other.gameObject.GetComponent<PlayerGlobal>();
        var isPlayer = playerGlobal != null;
        if(isPlayer)
        {
            for(int i = 0; i < _amount; i++)
            {
                GlobalPlayer.Instance.PlayerInventory.AddItem(_resourceName);
            }
        }

        Destroy(gameObject, 0.5f);
        _isTriggered = true;
    }
}
