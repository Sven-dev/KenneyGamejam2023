using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class ControllerUnit : MonoBehaviour, IInteractable
{
    [SerializeField] GameObject Model = null;

    public GameObject GoingTo { get; private set; }

    NavMeshAgent agent = null;

    private void Start()
    {
        if (agent == null)
        {
            agent = GetComponent<NavMeshAgent>();
        }
    }

    #region Connected Info
    /*/
        public void SetUnitTo(PawnInfo _info)
        {
            Info = _info;
            SetInfo();

            if (Info != null)
            {
                transform.position = Info.LastSavedPosition;
                Info.RemoveStationed();
            }

            //TODO; Smoke Effect
        }
        public void Removed()
        {
            Info.SavePosition(transform.position);

            GoingTo = null;
            Info = null;

            //TODO; Smoke Effect
        }
    //*/
    #endregion

    #region Movement and Commands

    public void GoTo(Vector3 _destination, GameObject _special = null)
    {
        if (agent)
        {
            GoingTo = _special;
            if (_special != null)
            {
                float distance = Vector3.Distance(transform.position, _special.transform.position);
                if (distance <= 0.8f)
                {
                    EnteringArea();
                }
            }

            if(gameObject.activeSelf)
                agent.SetDestination(_destination);
        }
    }

    private void EnteringArea()
    {
        if (GoingTo != null)
            transform.position = GoingTo.transform.position;
        GoingTo = null;

        if (HasArrived != null)
        {
            HasArrived.Invoke();
        }
    }

    public void OnTriggerEnter(Collider other)
    {
        if (GoingTo == other.gameObject)
        {
            EnteringArea();
        }
    }

    public event Action HasArrived;

    #endregion
}
