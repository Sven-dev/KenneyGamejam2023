using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerManager : MonoBehaviour
{
    #region Instance
    //put instance stuff here
    private static PlayerManager _Instance;
    public static PlayerManager Instance
    {
        get
        {
            return _Instance;
        }
    }

    private void Awake()
    {
        if (_Instance == null)
        {
            _Instance = this;
        }
        else
        {
            Destroy(this);
        }
    }

    #endregion

    PlayerControls playerControls = null;

    Inventory inventory = new Inventory();

    [SerializeField]
    Unit controlledUnit = null;

    [SerializeField]
    GameObject[] TowerTypes = null;

    bool buildMode = false;

    private void Start()
    {
        if (playerControls == null)
            SearchPlayerControls();

        // If new Game
        if (GameManager.Instance && GameManager.Instance.NewGame)
        {
            // TODO; stuff important for a new game
            // turorial?

        }
        else
        {
            // TODO; LOAD FILE
            //Load the string from the file


            //position gets changed
            // positionUnit = ???
            // isOutside = ???
        }

    }

    private void Update()
    {
         controlledUnit.UpdateAnimation(buildMode);
    }

    private void OnDestroy()
    {
        if (_Instance == this) _Instance = null;
    }

    public void SetBuildMode(bool _mode)
    {
        buildMode = _mode;
    }

    public void CommandUnit(Vector3 _at, GameObject _destination = null)
    {
        if (controlledUnit)
        {
            controlledUnit.GetTo(_at, _destination);
        }
    }

    private void SearchPlayerControls()
    {
        playerControls = GetComponent<PlayerControls>();
    }

    public void SwitchCommand(bool _one, CommandType _type)
    {
        if (playerControls == null)
            SearchPlayerControls();

        if (playerControls)
        {

            playerControls.SetActionType(_one, _type);
        }
    }


    public void BuildModeActivate(int towID, int _num)
    {
        playerControls.GoBuildMode(towID, _num);
    }

    public GameObject RemoveFromInventory(int num)
    {
        GameObject tower = inventory.SelectTower(num);
        inventory.RemoveFromInventory(num);

        if (CanvasManager.Instance != null)
        {
            CanvasManager.Instance.UpdateButtons(inventory.GetList());
        }

        return tower;
    }
    public GameObject SelectFromInventory(int num)
    {
        return inventory.SelectTower(num);
    }

    public bool PickUpTower(GameObject _tower)
    {
        if (inventory != null && TowerTypes.Length > 0)
        {
            Tower towStats = _tower.GetComponentInChildren<Tower>();
            int towType = 0;
            if (towStats.GetAttack() == Attack.AreaOfEffect)
            { towType = 3; }

            switch (towStats.GetElement())
            {
                case Element.Fire:
                    towType += 1;
                    break;
                case Element.Ice:
                    towType += 2;
                    break;
                default:
                    break;
            }



            bool success = inventory.AddToInventory(TowerTypes[towType]);
            if (CanvasManager.Instance != null)
            {
                CanvasManager.Instance.UpdateButtons(inventory.GetList());
            }

            return success;
        }

        return false;
    }
    public bool InputToInventory(Unit _unit, GameObject _tower)
    {
        if (_unit == controlledUnit && inventory != null)
        {
            bool success = inventory.AddToInventory(_tower);
            if (CanvasManager.Instance != null)
            {
                CanvasManager.Instance.UpdateButtons(inventory.GetList());
            }

            return success;
        }

        return false;
    }

    public void ArrivalMessage()
    {

    }

    public Vector3 GetPlayerPosition()
    {
        if (controlledUnit != null)
        {
            return controlledUnit.transform.position;
        }
        return Vector3.zero;
    }
}

/*/
 * if (controlledUnit == null && SceneExtraManager.Instance)
        {
            controlledUnit = SceneExtraManager.Instance.SpawnUnit(positionUnit);
            controlledUnit.transform.SetParent(null);
        }
        
        if (controlledUnit)
        {
            controlledUnit.gameObject.SetActive(isOutside);
            controlledUnit.transform.position = positionUnit;

            controlledUnit.HasArrived += ArrivalMessage;
        }
/*/
