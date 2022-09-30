using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Vote : MonoBehaviour
{
    public Slider Choice1Slider;
    public Slider Choice2Slider;

    int choice1Cnt = 0;
    int choice2Cnt = 0;
    int cntSum = 0;

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
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
