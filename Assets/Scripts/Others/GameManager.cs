using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{

    #region Instance
    //put instance stuff here
    private static GameManager _Instance;
    public static GameManager Instance
    {
        get
        {
            if (_Instance == null)
            {
                Debug.LogError("Instance for GameManager is NULL");
            }

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

        NewGame = true;
    }
    #endregion

    #region SceneHandling
    public void LoadScene(int _index)
    {
        SceneManager.LoadScene(_index);
    }

    // Close the Application, or stop play in Editor
    public void ExitGame()
    {
#if UNITY_EDITOR
        EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }
    #endregion

    #region PlayTime

    private void Start()
    {

    }


    public bool NewGame { get; private set; }

    #endregion
}
