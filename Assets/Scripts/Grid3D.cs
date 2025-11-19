using System.Collections.Generic;
using UnityEngine;

public class Grid3D : MonoBehaviour
{
    public int sizeX = 5;
    public int sizeY = 5;
    public int sizeZ = 5;
    public float spacing = 1f;
    public GameObject pointPrefab;

    [HideInInspector]
    public List<Vector3> pointsWorld = new List<Vector3>();

    void Start()
    {
        GenerateGrid();
    }

    void GenerateGrid()
    {
        pointsWorld.Clear();

        for (int x = 0; x < sizeX; x++)
        {
            for (int y = 0; y < sizeY; y++)
            {
                for (int z = 0; z < sizeZ; z++)
                {
                    Vector3 pos = new Vector3(x * spacing, y * spacing, z * spacing);
                    pointsWorld.Add(pos);
                    Instantiate(pointPrefab, pos, Quaternion.identity, transform);
                }
            }
        }
    }
}