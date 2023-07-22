using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory
{

    List<GameObject> listTowers = new List<GameObject>();

    public bool AddToInventory(GameObject _tower)
    {
        if(listTowers.Count < 8)
        {
            listTowers.Add(_tower);
            return true;
        }
        return false;
    }

    public GameObject SelectTower(int _num)
    {
        GameObject selected = null;
        if (listTowers.Count < _num)
        {
            selected = listTowers[_num];
        }
        return selected;
    }
    public GameObject RemoveFromInventory(int _num)
    {
        GameObject removed = null;
        if (listTowers.Count < _num)
        {
            removed = listTowers[_num];
            listTowers.RemoveAt(_num);
        }
        return removed;
    }

}
