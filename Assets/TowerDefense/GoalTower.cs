using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoalTower : MonoBehaviour
{
    [SerializeField] private int HealthPoints = 100;
    [SerializeField] private UnityIntEvent OnHealthChanged;

    public void TakeDamage(int value)
    {
        HealthPoints -= value;
        OnHealthChanged?.Invoke(HealthPoints);
    }
}