using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoalTower : MonoBehaviour
{
    [SerializeField] private int HealthPoints = 100;
    [SerializeField] private UnityIntEvent OnHealthChanged;

    bool hasDied = false;

    public void TakeDamage(int value)
    {
        HealthPoints -= value;
        OnHealthChanged?.Invoke(HealthPoints);

        if (!hasDied && HealthPoints <= 0)
        {
            hasDied = true;

            if (CanvasManager.Instance != null)
            {
                CanvasManager.Instance.GameOver(hasDied);
            }
            AudioManager.Instance.Play("GoalTowerExplode");
        }
        AudioManager.Instance.RandomizePitch("GoalTowerHit", 0.9f, 1.1f);
        AudioManager.Instance.Play("GoalTowerHit");
    }
}