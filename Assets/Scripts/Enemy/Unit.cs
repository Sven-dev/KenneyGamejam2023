using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Unit : MonoBehaviour
{
    

    NavMeshAgent agent = null;

    void Awake()
    {

    }

    // Start is called before the first frame update
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }



    GameObject destination = null;

    public void GetTo(Vector3 _at, GameObject _destination = null)
    {
        if (agent != null)
        {
            agent.SetDestination(_at);

        }

        destination = _destination;
    }

    public void OnTriggerEnter(Collider other)
    {
        if (other.gameObject == destination)
        {
            Arrival();
            
        }
        else
        {
            IInteractable interactable = other.gameObject.GetComponent<IInteractable>();
            if (interactable != null)
            {
                TowerPickup tp = other.gameObject.GetComponent<TowerPickup>();
                if (tp != null)
                {
                    if (PlayerManager.Instance != null)
                    {
                        if(PlayerManager.Instance.InputToInventory(this, other.gameObject))
                        {
                            //success inserted to Inventory
                            tp.RemovePickUP();
                        }
                    }
                }
            }
        }
    }

    public delegate void ArrivalAnnounce();
    public ArrivalAnnounce unitArrived;

    private void Arrival()
    {
        Debug.Log("Arrived!");
        gameObject.SetActive(false);

        unitArrived();
    }

}
