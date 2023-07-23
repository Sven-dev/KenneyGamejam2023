using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Unit : MonoBehaviour
{

    Animator anim = null;
    NavMeshAgent agent = null;

    void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        anim = GetComponentInChildren<Animator>();
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }


    public void UpdateAnimation(bool _build)
    {
        if (agent != null)
        {
            if (agent.velocity.magnitude > 0.0f && AudioManager.Instance != null)
            {
                //AudioManager.Instance.
            }

            if (anim != null)
            {
                anim.SetFloat("Speed", agent.velocity.magnitude);
                anim.SetBool("Holding", _build);
            }
        }
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
                        if (PlayerManager.Instance.InputToInventory(this, tp.GetTower()))
                        {
                            //success inserted to Inventory
                            tp.RemovePickUP();
                            AudioManager.Instance.Play("TowerPickUp");
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
        //gameObject.SetActive(false);
        //unitArrived();
    }

}
