using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AngleIndicator : MonoBehaviour
{
    public PoorPostureDetection poorPostureDetection;
    public Image angleUpperHalf;
    public Image angleLowerHalf;
    public Transform headset;
    public float colorThreshold = 5f;
    public float degreesPerSecond = 5f;
    public float testAngle = 0f;

    void RotateHeadset()
    {
        float angle = testAngle;
        if (angle >= 0 && angle <= 180f)
        {
            if (angle > 90f)
            {
                angle = 90f;
            }

            angleLowerHalf.fillAmount = angle / 90f;
            if (angle > poorPostureDetection.upperAngleThreshold && angle < poorPostureDetection.upperAngleThreshold + colorThreshold)
            {
                angleLowerHalf.color = Color.yellow;
            }
            else if (angle > poorPostureDetection.upperAngleThreshold + colorThreshold)
            {
                angleLowerHalf.color = Color.red;
            }

            Vector3 currentRotation = headset.rotation.eulerAngles;
            headset.rotation = Quaternion.Euler(currentRotation.x, currentRotation.y, angle);
        }
        else
        {
            angle = 360f - angle;
            if (angle > 90f)
            {
                angle = 90f;
            }

            angleUpperHalf.fillAmount = angle / 90f;
            if (angle > poorPostureDetection.upperAngleThreshold && angle < poorPostureDetection.upperAngleThreshold + colorThreshold)
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
   
    void Update()
    {
        RotateHeadset();
    }
}
