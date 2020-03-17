using System.Collections;
using System.Collections.Generic;
using Player;
using UnityEngine;

public class MapResource : MonoBehaviour
{
    [SerializeField] private ItemName _resourceName;
    public ItemName ResourceName => _resourceName;

    [SerializeField] private int _amount = 1;
    public int Amount => _amount;

    private void OnTriggerEnter(Collider other)
    {
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
    }
}
