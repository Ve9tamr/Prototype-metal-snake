using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ArrowDissolve : MonoBehaviour
{
    public float Dissolvation;
    public bool Started;
    private Image arrowImg;
    private Color ArrowColor;
    private void Awake()
    {
        arrowImg = GetComponent<Image>();
        ArrowColor = Color.white;
    }
    private void Update()
    {
        if(Started)
        {
            if (Dissolvation <= 0f)
            {
                Started = false;
                Dissolvation = 0f;
            }
            else
            {
                Dissolvation -= Time.deltaTime;
            }
        }
        ArrowColor.a = Dissolvation;
        arrowImg.color = ArrowColor;
    }
    public void StartDissolvation()
    {
        Started = true;
    }
    public void NewPlacing()
    {
        Started = false;
        Dissolvation = 1f;
    }
}
