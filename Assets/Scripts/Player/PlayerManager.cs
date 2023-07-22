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

    private void OnDestroy()
    {
        if (_Instance == this) _Instance = null;
    }

    // Update is called once per frame
    void Update()
    {


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

    public bool InputToInventory(Unit _unit, GameObject _tower)
    {
        if (_unit == controlledUnit && inventory != null)
        {
            return inventory.AddToInventory(_tower);
        }

        return false;
    }

    public void ArrivalMessage()
    {

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
