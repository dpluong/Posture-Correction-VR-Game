using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dot : MonoBehaviour
{
    public PoorPostureDetection poorPostureDetection;

    [SerializeField]
    GameObject dot;

    [SerializeField]
    GameObject followPosition;

    private Vector3 dotInitialPosition;
    private Vector3 dotInitialRotation;

    public float dotSpeed;

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
            dot.GetComponent<Renderer>().material.SetColor("_Color", new Color(255, 0, 0));
            dot.SetActive(true);
            dot.transform.parent = null;
            dot.transform.position = new Vector3(followPosition.transform.position.x, dot.transform.position.y, followPosition.transform.position.z);


            dot.transform.Translate(Vector3.up * dotSpeed * Time.deltaTime);

            dotStartMovingTime += Time.deltaTime;
            if (dotStartMovingTime >= dotEndMovingTime)
            {
                dot.transform.parent = Camera.main.gameObject.transform;
                dot.transform.localPosition = dotInitialPosition;
                dotStartMovingTime = 0f;
            }
        }
        if (!poorPostureDetection.m_isPoorPosture)
        {
            dot.GetComponent<Renderer>().material.SetColor("_Color", new Color(0, 255, 0));
            StartCoroutine(WaitBeforeDisableDot());            
        }
    }

    IEnumerator WaitBeforeDisableDot()
    {
        yield return new WaitForSeconds(1f);
        dot.SetActive(false);
        dot.transform.parent = Camera.main.gameObject.transform;
        dot.transform.localPosition = dotInitialPosition;
    }

}
