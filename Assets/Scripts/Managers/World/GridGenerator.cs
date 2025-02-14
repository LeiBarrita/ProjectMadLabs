using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridGenerator : MonoBehaviour
{
    public GameObject gridObject;

    [SerializeField] private int worldSizeX = 10;
    [SerializeField] private int worldSizeZ = 10;
    [SerializeField] private int worldSizeY = 10;
    [SerializeField] private int gridOffset = 2;

    void Start()
    {
        // generateCube();
        generateLab();
    }

    public void generateLab()
    {

    }

    public void generateCube()
    {
        for (int x = 0; x < worldSizeX; x++)
        {
            for (int z = 0; z < worldSizeZ; z++)
            {
                for (int y = 0; y < worldSizeY; y++)
                {
                    Vector3 pos = new(x * gridOffset, y * gridOffset, z * gridOffset);
                    GameObject block = Instantiate(gridObject, pos, Quaternion.identity);
                    block.transform.SetParent(transform);
                }
            }
        }
    }
}
