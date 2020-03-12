using GoMap;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LootGenerator : MonoBehaviour
{
    [SerializeField] private List<MapResource> _mapResourcePrefabs;
    [SerializeField] private List<MapResource> _currentResources;
    [SerializeField] private int _minResourcesCount = 5;
    [SerializeField] private int _maxResourcesCount = 15;
    [SerializeField] private GOMap _goMap;
    private GOTile _lastGoTile;

    [SerializeField] private Transform _lootParent;
    
    private void Awake()
    {
        _goMap.OnTileLoad.AddListener(OnTileLoad);
    }

    private void OnTileLoad(GOTile tile)
    {
        if(_lastGoTile == tile)
        {
            return;
        }

        GenerateResources(tile);
        _lastGoTile = tile;
    }

    private MapResource CreateRandomResource()
    {
        var prefab = _mapResourcePrefabs[Random.Range(0, _mapResourcePrefabs.Count)];
        var resourceInstance = Instantiate(prefab, _lootParent);
        return resourceInstance;

    }

    private void GenerateResources(GOTile goTile)
    {          
        var rndCount = Random.Range(_minResourcesCount, _maxResourcesCount);     
        UpdateResources();
        _lootParent = goTile.transform;
        for (int i = 0; i < rndCount; i++)
        {
            var randomPoint = MapHelpers.GetRandomWorldPointFromGoTile(goTile);
            if(!MapHelpers.IsWater(randomPoint))
            {
                var resource = CreateRandomResource();
                resource.transform.position = randomPoint;
                _currentResources.Add(resource);
            }
            
        }

    }

    private void UpdateResources()
    {
        _currentResources.RemoveAll(r => r == null);
    }
}
