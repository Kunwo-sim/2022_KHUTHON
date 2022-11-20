using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;
using UnityEngine.SceneManagement;

public class TitleManager : MonoBehaviour
{
    bool isMulti = false;

    [SerializeField]
    private VideoPlayer videoPlayer;

    [SerializeField]
    private GameObject displayVideoTexture;

    [SerializeField]
    private Button startButton;

    [SerializeField]
    private Button collectionButton;

    public GameObject collection;
    
    // private void Start()
    // {
    //     videoPlayer.loopPointReached += HandleOnVideoFinished;
    // }

    // private void HandleOnVideoFinished(VideoPlayer vp)
    // {
    //     displayVideoTexture.SetActive(false);
    // }

    private void Update()
    {
        if (videoPlayer.frame >= 90)
        {
            startButton.gameObject.SetActive(true);
            collectionButton.gameObject.SetActive(true);
        }
    }

    public void GameStartBtnClicked()
    {
        SceneManager.LoadScene("InGame");
    }

    public void CollectionBtnClicked()
    {
        collection.SetActive(true);
    }

    public void CloseBtnClicked()
    {
        collection.SetActive(false);
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
}
