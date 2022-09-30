using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Vote : MonoBehaviour
{
    public GameObject ClientVotePanel;
    public GameObject VotePanel;
    public Slider Choice1Slider;
    public Slider Choice2Slider;
    public TextMeshPro WinText;


    public int choice1Cnt = 0;
    public int choice2Cnt = 0;
    int cntSum = 0;

    public void StartVote()
    {
        StartCoroutine(StartVoteEffect());
    }

    public void ClientVoteEnd(int winIndex)
    {
        ClientVotePanel.SetActive(false);

        if (winIndex == 1)
        {
            WinText.text = "1�� �������� �� ���� ǥ�� �޾ҽ��ϴ�!";
        }
        else if (winIndex == 2)
        {
            WinText.text = "2�� �������� �� ���� ǥ�� �޾ҽ��ϴ�!";
        }

        WinText.GetComponent<GameObject>().SetActive(true);
    }

    public void EndVote()
    {
        if (choice1Cnt == choice2Cnt)
        {
            choice1Cnt += 1;
        }

        if (choice1Cnt > choice2Cnt)
        {
            WinText.text = "1�� �������� �� ���� ǥ�� �޾ҽ��ϴ�!";
        }
        else
        {
            WinText.text = "2�� �������� �� ���� ǥ�� �޾ҽ��ϴ�!";
        }

        WinText.GetComponent<GameObject>().SetActive(true);
    }

    public void ChoiceIncrease(int choice)
    {
        if (choice == 1)
        {
            choice1Cnt += 1;
        }
        else
        {
            choice2Cnt += 1;
        }

        cntSum += 1;
        Choice1Slider.value = choice1Cnt / (float)cntSum;
        Choice2Slider.value = choice2Cnt / (float)cntSum;
    }

    IEnumerator StartVoteEffect()
    {
        // 3
        yield return new WaitForSeconds(1.0f);
        // 2
        yield return new WaitForSeconds(1.0f);
        // 1
        yield return new WaitForSeconds(1.0f);

        VotePanel.SetActive(true);
        yield return null;
    }
}
