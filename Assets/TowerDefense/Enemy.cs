using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [HideInInspector] public int Rank;

    [SerializeField] private float HealthPoints = 1;
    [SerializeField] private float Damage = 1;

    void Update()
    {
        //navmesh?
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "GoalTower")
        {
            other.GetComponent<GoalTower>().TakeDamage((int)Damage);
            Destroy(gameObject);
        }
    }


    public void TakeDamage(float value)
    {
        HealthPoints -= value;

        if (HealthPoints <= 0)
        {
            Destroy(gameObject);
        }
    }
}