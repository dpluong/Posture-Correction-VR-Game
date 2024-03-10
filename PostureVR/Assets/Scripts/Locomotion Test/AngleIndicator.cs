using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AngleIndicator : MonoBehaviour
{
    public PoorPostureDetection poorPostureDetection;
    public Image angleUpperHalf;
    public Image angleLowerHalf;
    public Image headsetImage;
    public Transform headset;
    public Image correctPosture;
    public Image wrongPosture;
    public float colorThreshold = 5f;
    public float degreesPerSecond = 5f;
   // public float testAngle = 0f;

    void RotateHeadset()
    {
        float angle = poorPostureDetection.GetCenterEyeAngle();
        if (angle >= 0 && angle <= 180f)
        {
            if (angle > 90f)
            {
                angle = 90f;
            }

            angleLowerHalf.fillAmount = angle / 90f;
            if (angle <= poorPostureDetection.upperAngleThreshold)
            {
                angleLowerHalf.color = Color.green;
            }
            else if (angle > poorPostureDetection.upperAngleThreshold && angle < poorPostureDetection.upperAngleThreshold + colorThreshold)
            {
                angleLowerHalf.color = Color.yellow;
            }
            else if (angle > poorPostureDetection.upperAngleThreshold + colorThreshold)
            {
                angleLowerHalf.color = Color.red;
            }

            Vector3 currentRotation = headset.rotation.eulerAngles;
            headset.rotation = Quaternion.Euler(currentRotation.x, currentRotation.y, -angle);
        }
        else
        {
            angle = 360f - angle;
            if (angle > 90f)
            {
                angle = 90f;
            }

            angleUpperHalf.fillAmount = angle / 90f;
            if (angle <= poorPostureDetection.upperAngleThreshold)
            {
                angleLowerHalf.color = Color.green;
            }
            else if (angle > poorPostureDetection.upperAngleThreshold && angle < poorPostureDetection.upperAngleThreshold + colorThreshold)
            {
                angleUpperHalf.color = Color.yellow;
            }
            else if (angle > poorPostureDetection.upperAngleThreshold + colorThreshold)
            {
                angleUpperHalf.color = Color.red;
            }

            Vector3 currentRotation = headset.rotation.eulerAngles;
            headset.rotation = Quaternion.Euler(currentRotation.x, currentRotation.y, angle);
        }
    }

    void DisplayPostureIcon()
    {

    }
   
    void Update()
    {
        RotateHeadset();
        if (poorPostureDetection.m_isPoorPosture && poorPostureDetection.poorPostureTime > poorPostureDetection.poorPostureTimeThreshold)
        {
            wrongPosture.color = new Color(wrongPosture.color.r, wrongPosture.color.g, wrongPosture.color.b, 1f);
            angleUpperHalf.color = new Color(angleUpperHalf.color.r, angleUpperHalf.color.g, angleUpperHalf.color.b, 1f);
            angleLowerHalf.color = new Color(angleLowerHalf.color.r, angleLowerHalf.color.g, angleLowerHalf.color.b, 1f);
            headsetImage.color = new Color(headsetImage.color.r, headsetImage.color.g, headsetImage.color.b, 1f);
        }
        else
        {
            wrongPosture.color = new Color(wrongPosture.color.r, wrongPosture.color.g, wrongPosture.color.b, 0f);
            angleUpperHalf.color = new Color(angleUpperHalf.color.r, angleUpperHalf.color.g, angleUpperHalf.color.b, 0f);
            angleLowerHalf.color = new Color(angleLowerHalf.color.r, angleLowerHalf.color.g, angleLowerHalf.color.b, 0f);
            headsetImage.color = new Color(headsetImage.color.r, headsetImage.color.g, headsetImage.color.b, 0f);
            correctPosture.color = new Color(correctPosture.color.r, correctPosture.color.g, correctPosture.color.b, 1f);
        }
    }
}
