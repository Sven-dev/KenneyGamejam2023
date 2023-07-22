using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerPickup : MonoBehaviour, IInteractable
{

    [SerializeField]
    GameObject towerPrefab = null;

    public GameObject GetTower()
    {
        return towerPrefab;
    }

    public void RemovePickUP()
    {
        gameObject.SetActive(false);
    }
}
