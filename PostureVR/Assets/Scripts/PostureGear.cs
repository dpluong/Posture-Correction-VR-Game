using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PostureGear : MonoBehaviour
{
    public PoorPostureDetection poorPostureDetection;

    public SpriteRenderer goodPostureIcon;
    public SpriteRenderer badPostureIcon15;
    public SpriteRenderer badPostureIcon30;
    public SpriteRenderer badPostureIcon60;

    public float alphaValue = 0.3f;

    private float redValue;
    private float greenValue;
    private float blueValue;

    private bool isPostureCorrected = false;

    void Start() 
    {
        redValue = goodPostureIcon.color.r;
        greenValue = goodPostureIcon.color.g;
        blueValue = goodPostureIcon.color.b;
    }

    void DisplayPostureIcon()
    {
        if (poorPostureDetection.m_isPoorPosture && poorPostureDetection.poorPostureTime >= poorPostureDetection.poorPostureTimeThreshold)
        {
            if (!isPostureCorrected)
                ShowIconSprite();
            if (Camera.main.transform.eulerAngles.x >= 0f && Camera.main.transform.eulerAngles.x <= 15f)
            {
                
                ResetIconAlphaValue(true, false, true, true);
                badPostureIcon15.color = new Color(redValue, greenValue, blueValue, 1f);
            }
            else if (Camera.main.transform.eulerAngles.x > 15f && Camera.main.transform.eulerAngles.x <= 30f)
            {
                ResetIconAlphaValue(true, true, false, true);
                badPostureIcon30.color = new Color(redValue, greenValue, blueValue, 1f);
            }
            else if (Camera.main.transform.eulerAngles.x > 30f && Camera.main.transform.eulerAngles.x < 90f)
            {
                ResetIconAlphaValue(true, true, true, false);
                badPostureIcon60.color = new Color(redValue, greenValue, blueValue, 1f);
            }
            else 
            {
                ResetIconAlphaValue(true, false, true, true);
                badPostureIcon15.color = new Color(redValue, greenValue, blueValue, 1f);
            }
            isPostureCorrected = true;
            poorPostureDetection.interventionTriggered = true;
        }
        else
        {
            ResetIconAlphaValue(false,true,true,true);
            goodPostureIcon.color = new Color(redValue, greenValue, blueValue, 1f);
            if (isPostureCorrected)
                StartCoroutine(ResetIconAfterGoodPosture());
        }
    }

    IEnumerator ResetIconAfterGoodPosture()
    {
        yield return new WaitForSeconds(2f);
        HideIconSprite();
        isPostureCorrected = false;
        poorPostureDetection.interventionTriggered = false;
    }

    void ResetIconAlphaValue(bool flag0, bool flag15, bool flag30, bool flag60)
    {
        if (flag0)
            goodPostureIcon.color = new Color(redValue, greenValue, blueValue, alphaValue);
        
        if (flag15)
            badPostureIcon15.color = new Color(redValue, greenValue, blueValue, alphaValue);
        
        if (flag30)
            badPostureIcon30.color = new Color(redValue, greenValue, blueValue, alphaValue);

        if (flag60)
            badPostureIcon60.color = new Color(redValue, greenValue, blueValue, alphaValue);
    }

    void ShowIconSprite()
    {
        goodPostureIcon.enabled = true;
        badPostureIcon15.enabled = true;
        badPostureIcon30.enabled = true;
        badPostureIcon60.enabled = true;
    }

    void HideIconSprite()
    {
        goodPostureIcon.enabled = false;
        badPostureIcon15.enabled = false;
        badPostureIcon30.enabled = false;
        badPostureIcon60.enabled = false;
    }

    void Update()
    {
        DisplayPostureIcon();
    }
}
