using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerDefenseGenerator : MonoBehaviour
{
    [SerializeField] private List<GameObject> Layouts;

    // Start is called before the first frame update
    void Start()
    {
        int rnd = Random.Range(0, Layouts.Count);
        Layouts[rnd].SetActive(true);
    }
}