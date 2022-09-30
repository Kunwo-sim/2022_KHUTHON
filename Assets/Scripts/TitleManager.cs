using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TitleManager : MonoBehaviour
{

    bool isMulti = false;
    public void GameStartBtnClicked()
    {
        SceneManager.LoadScene("InGame");
    }

    public void MultiBtnClicked()
    {
        if (isMulti)
        {
            isMulti = false;
        }
        else
        {
            isMulti = true;
        }
        Debug.Log(isMulti);
    }

    public void ObserveBtnClicked()
    {

    }

    public void CollectionBtnClicked()
    {

    }
}
