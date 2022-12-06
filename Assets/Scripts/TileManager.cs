using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileManager : MonoBehaviour {

    public GameObject[] tilePrefabs;
    public float zSpawnPoint = 0;
    public float tileLength = 30;
    public int numberOfTiles = 5;
    public Transform playerTransform;
    private List<GameObject> activeTiles = new List<GameObject>();

    // Start is called before the first frame update
    void Start() {
        for (int i = 0; i < numberOfTiles; i++) {
            if (i == 0)
            {
                spawnTile(0);
                spawnTile(0);
            }
            else spawnTile(Random.Range(0, tilePrefabs.Length));
        }
    }

    // Update is called once per frame
    void Update() {
        if (playerTransform.position.z - 35 > zSpawnPoint - (numberOfTiles * tileLength)) {
            spawnTile(Random.Range(0, tilePrefabs.Length));
            DeleteTile();
        }
    }

    public void spawnTile(int tileIndex) {
        GameObject tile = Instantiate(tilePrefabs[tileIndex], transform.forward * zSpawnPoint, transform.rotation);
        activeTiles.Add(tile);
        zSpawnPoint += tileLength;
    }

    public void DeleteTile() {
        Destroy(activeTiles[0]);
        activeTiles.RemoveAt(0);
    }
}
