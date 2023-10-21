using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraReadjustment : MonoBehaviour
{
    
    public PoorPostureDetection poorPostureDetection;
    public Transform cameraTransform;
    public Transform cameraOffset;
    [SerializeField] private float degreesPerSecond = 5f;
    private float rotateAngle = 0f; 

    void RotateCameraAroundXAxis()
    {
        transform.Rotate(new Vector3(degreesPerSecond, 0, 0) * Time.deltaTime);
        rotateAngle += degreesPerSecond * Time.deltaTime;
    }

    void Update()
    {
        if (poorPostureDetection.m_isPoorPosture && poorPostureDetection.poorPostureTime >= poorPostureDetection.poorPostureTimeThreshold)
        {
            //Debug.Log("rotate");
            if (this.transform.parent.gameObject == cameraTransform.gameObject)
            {
                this.transform.SetParent(null);
                this.transform.position = cameraOffset.position;
                this.transform.rotation = cameraTransform.rotation;
                this.transform.SetParent(cameraOffset);
                //this.transform.localPosition = cameraTransform.localPosition;

                cameraTransform.SetParent(this.transform);
            }

            if (rotateAngle < 5f)
            {
                RotateCameraAroundXAxis();
            }
        }

        if (!poorPostureDetection.m_isPoorPosture)
        {
            if (cameraTransform.parent.gameObject == this.gameObject)
            {
                this.transform.DetachChildren();
                this.transform.position = cameraTransform.position;
                this.transform.rotation = cameraTransform.rotation;
                this.transform.SetParent(cameraTransform);
                cameraTransform.SetParent(cameraOffset);
                rotateAngle = 0f;
            }
        }
    }
}
