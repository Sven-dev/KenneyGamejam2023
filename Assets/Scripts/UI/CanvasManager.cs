using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CanvasManager : MonoBehaviour
{
    [SerializeField]
    GameObject UndoButton = null;

    [SerializeField]
    TowerButton[] TowButtons = null;
    [SerializeField]
    Sprite[] TowIcons = null;

    #region Instance
    //put instance stuff here
    private static CanvasManager _Instance;
    public static CanvasManager Instance
    {
        get
        {
            return _Instance;
        }
    }

    private void Awake()
    {
        if (_Instance == null)
        {
            _Instance = this;
        }
        else
        {
            Destroy(this);
        }
    }

    #endregion

    public void CallTower(int num)
    {
        UndoButton.SetActive(true);
    }
    public void UndoCall()
    {
        UndoButton.SetActive(false);
    }
    public void UpdateButtons(List<GameObject> towerlist)
    {
        int num = 0;
        Tower tow = null;

        foreach (GameObject towerObject in towerlist)
        {
            towerObject.SetActive(false);
        }

        foreach (GameObject towerObject in towerlist)
        {
            tow = towerObject.GetComponent<Tower>();
            if (tow != null)
            {
                towerObject.SetActive(true);
                UpdateButt(num, tow);
                num++;
            }
        }
    }

    public void UpdateButt(int num, Tower tow)
    {
        if (num >= 0 && num < 8 && TowButtons != null)
        {
            TowButtons[num].SetButtonLayout(tow);
        }
    }

    public Sprite GetTowerImage(Attack _atk, Element _elmo)
    {
        int num = 0;
        if (_atk == Attack.AreaOfEffect)
        { num = 3; }

        switch (_elmo)
        {
            case Element.Fire:
                num += 1;
                break;
            case Element.Ice:
                num += 2;
                break;
            default:
                break;
        }

        return TowIcons[num];
    }
}
