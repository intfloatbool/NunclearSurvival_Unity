using GoMap;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LootGenerator : MonoBehaviour
{
    [SerializeField] private List<MapResource> _mapResourcePrefabs;
    private Dictionary<ItemName, MapResource> _mapResourceDict = new Dictionary<ItemName, MapResource>();

    [SerializeField] private List<MapResource> _currentResources;

    [SerializeField] private int _minResourcesCount = 5;
    [SerializeField] private int _maxResourcesCount = 15;
    
    [SerializeField] private GOMap _goMap;
    private GOTile _lastGoTile;

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
        Debug.Log("LOOT GENERATOR: TILE LOADING! : " + tile.gameObject.name);
    }

    private MapResource CreateRandomResource()
    {
        var prefab = _mapResourcePrefabs[Random.Range(0, _mapResourcePrefabs.Count)];


        var resourceInstance = Instantiate(prefab);
        return resourceInstance;

    }

    private void GenerateResources(GOTile goTile)
    {
        
        var tileMeshFilter = goTile.GetComponent<MeshFilter>();
        if (tileMeshFilter == null)
        {
            return;
        }

        var vertices = tileMeshFilter.mesh.vertices;
        var rndCount = Random.Range(_minResourcesCount, _maxResourcesCount);
        for (int i = 0; i < rndCount; i++)
        {
            var randomVertice = vertices[Random.Range(0, vertices.Length)];
            var worldPos = gameObject.transform.TransformPoint(randomVertice);          
            var resource = CreateRandomResource();
            resource.transform.position = worldPos;

            _currentResources.Add(resource);
        }

        var col = GetComponent<MeshCollider>();
    }
}
