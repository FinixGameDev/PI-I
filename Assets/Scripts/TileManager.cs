using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileManager : MonoBehaviour
{
    [SerializeField] GameObject[] _tilePrefabs;
    [SerializeField] float _zSpawn = 0;
    [SerializeField] float _tileLength = 30;
    [SerializeField] int _maxActiveTiles = 5;

    [SerializeField] Transform playerTransform;

    private List<GameObject> _activeTiles = new List<GameObject>();

    // Start is called before the first frame update
    void Start()
    {
        for (int i = 0; i < _maxActiveTiles; i++)
        {
            if (i == 0)
                SpawnTile(0);
            else
                SpawnTile(Random.Range(0, _tilePrefabs.Length));
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (playerTransform.position.z - 35 > _zSpawn - (_maxActiveTiles * _tileLength))
        {
            SpawnTile(Random.Range(0, _tilePrefabs.Length));
            DeleteTile();
        }

    }

    public void SpawnTile(int tileIndex)
    {
        GameObject newTile = Instantiate(_tilePrefabs[tileIndex],transform.forward * _zSpawn, transform.rotation, this.transform);
        _activeTiles.Add(newTile);
        _zSpawn += _tileLength;
    }

    public void DeleteTile()
    {
        Destroy(_activeTiles[0]);
        _activeTiles.RemoveAt(0);
    }
}
