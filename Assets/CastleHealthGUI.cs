using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CastleHealthGUI : MonoBehaviour
{
    [SerializeField] private CanvasGroup HealthObject;
    [SerializeField] private Slider HealthBar;

    private float Cooldown = 5f;

    public void UpdateHealthValue(int value)
    {
        HealthObject.alpha = 1;
        HealthBar.value = value;

        Cooldown = 5f;
    }

    private IEnumerator _FadeOutHealthBar()
    {
        while (true)
        {
            if (Cooldown > 0)
            {
                Cooldown -= Time.deltaTime;
                yield return null;
            }

            yield return null;
        }
    }
}