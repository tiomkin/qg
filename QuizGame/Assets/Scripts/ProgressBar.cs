using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[ExecuteInEditMode()]
public class ProgressBar : MonoBehaviour
{
    public int min;
    public int max;
    public int current;
    public Image imgFill;


    // Start is called before the first frame update
    void Start()
    {
        //UpdateProgressBar();
    }

    public void UpdateProgressBar()
    {
        imgFill.fillAmount = GetCurrentFill();
    }

    private float GetCurrentFill()
    {
        float currentOffset = current - min;
        float maxOffset = max - min;
        float fillAmount = currentOffset / maxOffset;
        return fillAmount;
    }

    public void Add(int value)
    {
        current += value;
        if (current >= max)
        {
            current = max;
            imgFill.fillAmount = GetCurrentFill();
        }
    }
}
