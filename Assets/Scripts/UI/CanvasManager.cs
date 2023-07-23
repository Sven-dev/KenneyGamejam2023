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

    [SerializeField] 
    GameObject GameOverScreen;


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
    
    public void GameOver(bool _died)
    {
        GameOverScreen.SetActive(_died);
    }

    public void GoBackToMenu()
    {
        if (LevelManager.Instance != null)
        {
            LevelManager.Instance.LoadLevel(1, Transition.Crossfade);
        }
    }

    public void CallTower(int num)
    {

        if (PlayerManager.Instance != null)
        {
            GameObject TowerOBJ = PlayerManager.Instance.SelectFromInventory(num);
            if (TowerOBJ != null)
            {
                Tower tower = TowerOBJ.GetComponentInChildren<Tower>();

                if (tower != null)
                {
                    int typeID = 0;
                    if (tower.GetAttack() == Attack.AreaOfEffect)
                    { typeID = 3; }

                    switch (tower.GetElement())
                    {
                        case Element.Fire:
                            typeID += 1;
                            break;
                        case Element.Ice:
                            typeID += 2;
                            break;
                        default:
                            break;
                    }

                    PlayerManager.Instance.BuildModeActivate(typeID, num);
                }
            }
        }
    }
    public void UndoCall()
    {


    }
    public void UpdateButtons(List<GameObject> towerlist)
    {
        int num = 0;
        Tower tow = null;


        for (int i = 0; i < 8; i++)
        {
            TowButtons[i].gameObject.SetActive(false);
        }

        foreach (GameObject towerObject in towerlist)
        {
            tow = towerObject.GetComponentInChildren<Tower>();
            if (tow != null)
            {
                UpdateButt(num, tow);
                num++;
            }

        }
    }

    public void UpdateButt(int num, Tower tow)
    {
        if (num >= 0 && num < 8 && TowButtons != null)
        {
            TowButtons[num].gameObject.SetActive(true);
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
