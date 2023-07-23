using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory
{
    int maxSlot = 4;
    List<GameObject> listTowers = new List<GameObject>();

    public bool AddToInventory(GameObject _tower)
    {
        if (listTowers.Count < maxSlot)
        {
            listTowers.Add(_tower);
            return true;
        }
        return false;
    }

    public int GetCount()
    {
        return listTowers.Count;
    }
    public List<GameObject> GetList()
    {
        return listTowers;
    }
    public GameObject SelectTower(int _num)
    {
        GameObject selected = null;
        if (listTowers.Count > _num)
        {
            selected = listTowers[_num];
        }
        return selected;
    }
    public GameObject RemoveFromInventory(int _num)
    {
        GameObject removed = null;
        if (listTowers.Count > _num)
        {
            removed = listTowers[_num];
            listTowers.RemoveAt(_num);
        }
        return removed;
    }

}
