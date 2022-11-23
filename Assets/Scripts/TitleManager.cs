using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;
using UnityEngine.SceneManagement;

using TMPro;

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

    // 컬렉션
    public GameObject collection;
    public GameObject[] endingList;
    public Sprite UnlockEndingBG;

    [SerializeField] private List<Sprite> endingCutScenes;

    [SerializeField] private GameObject collectionPanel;
    [SerializeField] private Image collectionDisplayImage;

    private void Start()
    {
        // endingUnLockBtn.onClick.AddListener(() => UnlockEnding(1));

        for (int i = 1; i <= 6; i++)
        {
            if (PlayerPrefs.GetInt($"UnlockEnding{i}", 0) is 1)
            {
                UnlockEnding(i);
            }
        }
    }

    private void Update()
    {
        if (videoPlayer.frame >= 100)
        {
            startButton.gameObject.SetActive(true);
            collectionButton.gameObject.SetActive(true);
        }

        if (Input.GetKeyDown(KeyCode.F12))
        {
            PlayerPrefs.DeleteAll();
            SceneManager.LoadScene(SceneManager.GetActiveScene().name, LoadSceneMode.Single);
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

    public void UnlockEnding(int endingNum)
    {
        GameObject targetEnding = endingList[endingNum];
        Image endingBG = targetEnding.GetComponentInChildren<Image>();
        endingBG.sprite = UnlockEndingBG;

        string endingText = "";

        switch (endingNum)
        {
            case 1:
                endingText = "Ending 1 : 이누공 엔딩 (수집)";
                break; 
            case 2:
                endingText = "Ending 2 : 넷카마 엔딩 (수집)";
                break;
            case 3:
                endingText = "Ending 3 : her 엔딩 (수집)";
                break;
            case 4:
                endingText = "Ending 4 : 대학원 엔딩 (수집)";
                break;
            case 5:
                endingText = "Ending 5 : 도믿맨 엔딩 (수집)";
                break;
            case 6:
                endingText = "Ending 6 : 사이버망령 엔딩 (수집)";
                break;
            default:
                break;
        }

        TextMeshProUGUI targetText = targetEnding.GetComponentInChildren<TextMeshProUGUI>();
        targetText.text = endingText;
        targetText.color = Color.black;
    }

    public void OpenCollection(int endingNum)
    {
        if (PlayerPrefs.GetInt($"UnlockEnding{endingNum}", 0) is not 1)
        {
            return;
        }

        collectionPanel.SetActive(true);
        collectionDisplayImage.sprite = endingCutScenes[endingNum - 1];
    }
}
