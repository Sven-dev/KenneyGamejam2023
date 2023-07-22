using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TowerButton : MonoBehaviour
{
    [SerializeField]
    Image towerImage = null;

    public void SetButtonLayout(Tower tow)
    {
        if (towerImage != null)
        {
            if (CanvasManager.Instance != null)
            {
                towerImage.sprite = CanvasManager.Instance.GetTowerImage(tow.GetAttack(), tow.GetElement());
            }
        }
    }

}
