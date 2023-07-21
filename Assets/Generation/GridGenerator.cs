using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridGenerator : MonoBehaviour
{
    [SerializeField] private int GridSize = 10;
    [SerializeField] private Transform GridPrefab;
    [Space]
    [SerializeField] private List<GameObject> TilePrefabs;

    [SerializeField] private List<List<Transform>> Grid = new List<List<Transform>>();
    
    
    // Start is called before the first frame update
    void Start()
    {
        GenerateGridStructure();
        GetGridPosition(2, 3);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public Transform GetGridPosition(int z, int x)
    {
        print(Grid[z][x].name);
        return Grid[z][x];
    }

    private void GenerateGridStructure()
    {
        for (int z = 0; z < GridSize; z++)
        {
            Transform row = Instantiate(GridPrefab, Vector3.forward * z, Quaternion.identity, transform);
            List<Transform> temp = new List<Transform>();
            for (int x = 0; x < GridSize; x++)
            {
                Transform tile = Instantiate(GridPrefab, Vector3.forward * z + Vector3.right * x, Quaternion.identity, row);
                temp.Add(tile);
            }

            Grid.Add(temp);
        }
    }

    public void PlaceStructure(int x, int y, float value)
    {
        Transform tile = Grid[y][x];
        if (tile.childCount != 0)
        {
            Destroy(tile.GetChild(0).gameObject);
        }

        if (value < 0.5f)
        {
            Instantiate(TilePrefabs[0], tile);
            
        }
        else
        {
            Instantiate(TilePrefabs[1], tile);
        }
    }
}
