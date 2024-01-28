using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ProgressBar : MonoBehaviour
{
    public float remainingTime;
    public int duration = 45;
    public Image insideBar;
    public Text text;

    private float initialImageWidth;

    void Start()
    {
        // set the insideBar to zero
        initialImageWidth = insideBar.rectTransform.rect.width;
        remainingTime = duration;
        SetBarToZero();
    }

    public void SetBarToZero()
    {
        RectTransform rectTransform = insideBar.rectTransform;
        rectTransform.sizeDelta = new Vector2(0f, rectTransform.sizeDelta.y);
    }

    public void UpdateProgressBar()
    {
        float filledWidth = initialImageWidth * (1 - remainingTime / duration);
        insideBar.fillAmount = filledWidth;
    }

}
