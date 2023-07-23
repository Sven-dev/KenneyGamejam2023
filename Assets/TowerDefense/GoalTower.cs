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

        if (HealthPoints == 0)
        {
            print("Game over");
            AudioManager.Instance.Play("GoalTowerExplode");
        }

        AudioManager.Instance.RandomizePitch("GoalTowerHit", 0.9f, 1.1f);
        AudioManager.Instance.Play("GoalTowerHit");
    }
}