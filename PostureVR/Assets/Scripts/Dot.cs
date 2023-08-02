using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dot : MonoBehaviour
{
    public PoorPostureDetection poorPostureDetection;

    [SerializeField]
    GameObject dot;

    private Vector3 dotInitialPosition;
    private Vector3 dotInitialRotation;

    public float dotSpeed;
    private float dotPosition = 0f;

    private float dotStartMovingTime = 0f;
    public float dotEndMovingTime = 0f;

    void Start()
    {
        dotInitialPosition = dot.transform.localPosition;
        dotInitialRotation = dot.transform.localEulerAngles;
    }


    void Update()
    {
        DotMovement();
    }

    void DotMovement()
    {
        //center.transform.position = Camera.main.transform.position;
        if (poorPostureDetection.m_isPoorPosture && poorPostureDetection.poorPostureTime >= poorPostureDetection.poorPostureTimeThreshold)
        {
            dot.SetActive(true);
            dot.transform.parent = null;

            dot.transform.Translate(Vector3.up * dotSpeed * Time.deltaTime);

            dotStartMovingTime += Time.deltaTime;
            if (dotStartMovingTime >= dotEndMovingTime)
            {
                dot.transform.parent = Camera.main.gameObject.transform;
                dot.transform.localPosition = dotInitialPosition;
                dotStartMovingTime = 0f;
            }
        }
        else
        {
            dot.transform.parent = Camera.main.gameObject.transform;
            dot.transform.localPosition = dotInitialPosition;
            dot.transform.localEulerAngles = dotInitialRotation;
            dot.SetActive(false);
        }
    }
}
