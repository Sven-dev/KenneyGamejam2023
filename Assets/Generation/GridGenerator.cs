using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridGenerator : MonoBehaviour
{
    [Range(1, 10)]
    [SerializeField] private int BoundSize;
    [Space]
    [Range(1, 20)]
    [SerializeField] private int TowerDefenseSize;
    [Space]
    [Range(0, 1)]
    [SerializeField] private float GrassTerrainChance;
    [Range(0, 1)]
    [SerializeField] private float IceTerrainChance;
    [Range(0, 1)]
    [SerializeField] private float DesertTerrainChance;
    [Space]
    [Range(0, 1)]
    [SerializeField] private float TreeChance;


    [SerializeField] private int GridSize = 10;
    [SerializeField] private Transform GridPrefab;
    [Space]
    [SerializeField] private List<Tile> TerrainPrefabs;
    [SerializeField] private List<Tile> GrassTreePrefabs;
    [SerializeField] private List<Tile> IceTreePrefabs;
    [SerializeField] private List<Tile> DesertTreePrefabs;
    [Space]
    [SerializeField] private List<List<Tile>> Grid = new List<List<Tile>>();

    public bool Test = false;
    public bool GridGenerated = false;

    private Texture2D TerrainPerlin;
    private Texture2D TreePerlin;

    private IEnumerator Start()
    {
        while(true)
        {
            if (TerrainPerlin != null && TreePerlin != null)
            {
                break;
            }

            yield return null;
        }

        GenerateGridStructure();
    }

    private void Update()
    {
        if (Test)
        {
            Test = false;

            foreach(Transform child in transform)
            {
                Destroy(child.gameObject);
            }

            GenerateGridStructure();
        }
    }

    public void GetTerrainPerlin(Texture2D perlinTexture)
    {
        TerrainPerlin = perlinTexture;
    }

    public void GetTreePerlin(Texture2D perlinTexture)
    {
        TreePerlin = perlinTexture;
    }

    public Transform GetGridPosition(int z, int x)
    {
        print(Grid[z][x].name);
        return Grid[z][x].transform;
    }

    private void GenerateGridStructure()
    {
        for (int z = 0; z < GridSize; z++)
        {
            Transform row = Instantiate(GridPrefab, Vector3.forward * z, Quaternion.identity, transform);
            List<Tile> temp = new List<Tile>();
            for (int x = 0; x < GridSize; x++)
            {
                Tile prefab;

                float ztemp = (GridSize - Mathf.Abs(z - GridSize / 2)) / 100f;
                float xtemp = (GridSize - Mathf.Abs(x - GridSize / 2)) / 100f;
                float grassWeight = Mathf.Clamp01(((ztemp + xtemp) / 2) - 0);


                print(grassWeight);

                //Center area (just spawn grass tiles)
                if (z > GridSize / 2 - TowerDefenseSize / 2 && z < GridSize / 2 + TowerDefenseSize / 2 &&
                    x > GridSize / 2 - TowerDefenseSize / 2 && x < GridSize / 2 + TowerDefenseSize / 2)
                {
                    prefab = TerrainPrefabs[0];
                }
                else if (grassWeight > 0.95f)
                {
                    prefab = TerrainPrefabs[0];
                }
                else
                {
                    float terrainPerlinValue = TerrainPerlin.GetPixel(z, x).r;
                    if (terrainPerlinValue > 0.6f)
                    {
                        //Desert
                        if (z > GridSize / 2 - TowerDefenseSize / 1.5f && z < GridSize / 2 + TowerDefenseSize / 1.5f &&
                            x > GridSize / 2 - TowerDefenseSize / 1.5f && x < GridSize / 2 + TowerDefenseSize / 1.5f)
                        {
                            prefab = TerrainPrefabs[2];
                        }
                        else
                        {
                            float treePerlinValue = TreePerlin.GetPixel(z, x).r;
                            if (x <= BoundSize || x >= GridSize - BoundSize || z <= BoundSize || z >= GridSize - BoundSize)
                            {
                                //Bound
                                prefab = DesertTreePrefabs[Random.Range(0, DesertTreePrefabs.Count)];
                            }
                            else if (treePerlinValue > 1-TreeChance)
                            {
                                //Trees
                                prefab = DesertTreePrefabs[Random.Range(0, DesertTreePrefabs.Count)];
                            }
                            else
                            {
                                //No trees
                                prefab = TerrainPrefabs[2];
                            }
                        }
                    }
                    else if (terrainPerlinValue > 0.4f)
                    {
                        //Grass
                        if (z > GridSize / 2 - TowerDefenseSize / 1.5f && z < GridSize / 2 + TowerDefenseSize / 1.5f &&
                            x > GridSize / 2 - TowerDefenseSize / 1.5f && x < GridSize / 2 + TowerDefenseSize / 1.5f)
                        {
                            prefab = TerrainPrefabs[0];
                        }
                        else
                        {
                            float treePerlinValue = TreePerlin.GetPixel(z, x).r;
                            if (x <= BoundSize || x >= GridSize - BoundSize || z <= BoundSize || z >= GridSize - BoundSize)
                            {
                                //Bound
                                prefab = GrassTreePrefabs[Random.Range(0, GrassTreePrefabs.Count)];
                            }
                            else if (treePerlinValue > 1 - TreeChance)
                            {
                                //Trees
                                prefab = GrassTreePrefabs[Random.Range(0, GrassTreePrefabs.Count)];
                            }
                            else
                            {
                                //No trees
                                prefab = TerrainPrefabs[0];
                            }
                        }
                    }
                    else
                    {
                        //Ice
                        if (z > GridSize / 2 - TowerDefenseSize / 1.5f && z < GridSize / 2 + TowerDefenseSize / 1.5f &&
                            x > GridSize / 2 - TowerDefenseSize / 1.5f && x < GridSize / 2 + TowerDefenseSize / 1.5f)
                        {
                            prefab = TerrainPrefabs[1];
                        }
                        else
                        {
                            float treePerlinValue = TreePerlin.GetPixel(z, x).r;
                            if (x <= BoundSize || x >= GridSize - BoundSize || z <= BoundSize || z >= GridSize - BoundSize)
                            {
                                //Bound
                                prefab = IceTreePrefabs[Random.Range(0, IceTreePrefabs.Count)];
                            }
                            else if (treePerlinValue > 1 - TreeChance)
                            {
                                //Trees
                                prefab = IceTreePrefabs[Random.Range(0, IceTreePrefabs.Count)];
                            }
                            else
                            {
                                //No trees
                                prefab = TerrainPrefabs[1];
                            }
                        }
                    }
                }


                int rnd = Random.Range(0, 4);
                Vector3 rotationVector = Vector3.up * 90 * rnd;
                Quaternion rotation = Quaternion.Euler(rotationVector);

                Tile tile = Instantiate(prefab, Vector3.forward * z + Vector3.right * x, rotation, row);           
                temp.Add(tile);
            }

            Grid.Add(temp);
        }

        GridGenerated = true;
    }

    /*
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
    */
}
