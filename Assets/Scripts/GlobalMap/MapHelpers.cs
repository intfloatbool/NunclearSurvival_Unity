using GoMap;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class MapHelpers 
{
    private static readonly string waterName = "water";
    public static bool IsWater(Vector3 position)
    {
        var radius = 1f;
        var colliders = Physics.OverlapSphere(position, radius);
        foreach (var collider in colliders)
        {
            if (collider.transform.parent.name.Equals(waterName))
                return true;
        }
        return false;
    }

    public static Vector3 GetRandomWorldPointFromGoTile(GOTile goTile)
    {
        var tileMeshFilter = goTile?.GetComponent<MeshFilter>();
        if (tileMeshFilter == null)
        {
            Debug.LogError("Go tile not contains meshFilter!");
            return Vector3.zero;
        }

        var vertices = tileMeshFilter.mesh.vertices;
        var localToWorld = goTile.transform.localToWorldMatrix;

        var randomVertice = vertices[Random.Range(0, vertices.Length)];
        var worldPos = localToWorld.MultiplyPoint3x4(randomVertice);

        return worldPos;
    }
}
