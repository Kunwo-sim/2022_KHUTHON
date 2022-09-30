using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EasterEgg : MonoBehaviour
{
    public void ChagneScene()
    {
        string name = SceneManager.GetActiveScene().name;

        if (name == "Mobile")
        {
            SceneManager.LoadScene("InGame");
        }
        else if (name == "InGame")
        {
            SceneManager.LoadScene("Mobile");
        }
    }

    void Start()
    {
        DontDestroyOnLoad(this.gameObject);
    }
}
