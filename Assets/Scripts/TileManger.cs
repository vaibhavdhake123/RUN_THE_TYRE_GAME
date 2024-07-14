using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileManger : MonoBehaviour
{
    public GameObject[] tilePrefabs;

    public float zSpawn = 0;

    public float tileLength = 30;

    public int numberOfTile = 5;

    public Transform playerTransform;

    private List<GameObject> activeTile = new List<GameObject>();

    void Start()
    {
        for(int i = 0; i < numberOfTile; i++) 
        {
            if (i == 0)
                SpawnTile(0);
            else
                SpawnTile(Random.Range(0, tilePrefabs.Length));
        }
        
    }

    void Update()
    {
        if(playerTransform.position.z - 35 > zSpawn - (numberOfTile * tileLength))
        {
            SpawnTile(Random.Range(0, tilePrefabs.Length));
            DeleteTile();

        }

    }

    public void SpawnTile(int tileIndex)
    {
        GameObject go = Instantiate(tilePrefabs[tileIndex], transform.forward * zSpawn, transform.rotation);
        activeTile.Add(go);
        zSpawn += tileLength;
    }

    private void DeleteTile()
    {
        Destroy(activeTile[0]);
        activeTile.RemoveAt(0);
    }
}
