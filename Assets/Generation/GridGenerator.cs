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
    [Space]
    [SerializeField] private List<Transform> Towers = new List<Transform>();

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

    public void GetTerrainPerlin(Texture2D perlinTexture)
    {
        TerrainPerlin = perlinTexture;
    }

    public void GetTreePerlin(Texture2D perlinTexture)
    {
        TreePerlin = perlinTexture;
    }

    public Tile GetGridPosition(int z, int x)
    {
        return Grid[z][x];
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
                bool hasTree = false;

                float ztemp = (GridSize - Mathf.Abs(z - GridSize / 2)) / 100f;
                float xtemp = (GridSize - Mathf.Abs(x - GridSize / 2)) / 100f;
                float grassWeight = Mathf.Clamp01(((ztemp + xtemp) / 2) - 0);

                //Center area (don't spawn anything)
                if (z >= GridSize / 2 - TowerDefenseSize / 2 && z <= GridSize / 2 + TowerDefenseSize / 2 &&
                    x >= GridSize / 2 - TowerDefenseSize / 2 && x <= GridSize / 2 + TowerDefenseSize / 2)
                {
                    continue;
                }
                else if (grassWeight > 0.97f)
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
                                hasTree = true;
                            }
                            else if (treePerlinValue > 1-TreeChance)
                            {
                                //Trees
                                prefab = DesertTreePrefabs[Random.Range(0, DesertTreePrefabs.Count)];
                                hasTree = true;
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
                                hasTree = true;
                            }
                            else if (treePerlinValue > 1 - TreeChance)
                            {
                                //Trees
                                prefab = GrassTreePrefabs[Random.Range(0, GrassTreePrefabs.Count)];
                                hasTree = true;
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
                                hasTree = true;
                            }
                            else if (treePerlinValue > 1 - TreeChance)
                            {
                                //Trees
                                prefab = IceTreePrefabs[Random.Range(0, IceTreePrefabs.Count)];
                                hasTree = true;
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
                tile.HasTree = hasTree;
                temp.Add(tile);
            }

            Grid.Add(temp);
        }

        int amountOfTowers = 0;
        while (amountOfTowers < 15)
        {
            int rndx = Random.Range(0, GridSize);
            int rndz = Random.Range(0, GridSize);

            Tile randomtile = GetGridPosition(rndz, rndx);

            if (!randomtile.HasTree)
            {
                Transform tower;
                switch (randomtile.TerrainType)
                {
                    case Element.Normal:
                        int rnd1 = Random.Range(0, 2);
                        tower = Towers[rnd1];
                        break;

                    case Element.Fire:
                        int rnd2 = Random.Range(2, 4);
                        tower = Towers[rnd2];
                        break;

                    case Element.Ice:
                        int rnd3 = Random.Range(4, 6);
                        tower = Towers[rnd3];
                        break;

                    default:
                        throw new System.Exception("What the fuck is this element?");
                }

                Instantiate(tower, randomtile.transform.position + Vector3.up * 0.5f, Quaternion.identity);

                amountOfTowers++;
            } 

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
