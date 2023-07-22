using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitPool : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public ControllerUnit GetUnit()
    {
        Transform trns = null;
        ControllerUnit unit = null;

        for (int i = 0; i < transform.childCount; i++)
        {
            trns = transform.GetChild(i);
            if (trns != null && !trns.gameObject.activeSelf)
            {
                unit = trns.GetComponent<ControllerUnit>();
                if (unit != null)
                {
                    i = transform.childCount;
                    return unit;
                }
            }
        }

        trns = transform.GetChild(0);
        if (trns != null)
        {
            Transform egg = Instantiate(trns);
            egg.transform.SetParent(transform);
            unit = egg.GetComponent<ControllerUnit>();

            return unit;
        }
        else
            Debug.LogError("Unit does not exist! Can't create new Unit");

        return null;
    }
}
