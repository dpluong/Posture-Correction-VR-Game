using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dot : MonoBehaviour
{
    public PoorPostureDetection poorPostureDetection;

    [SerializeField] private float distance = 3.0f;

    public float dotSpeed;

    private float dotStartMovingTime = 0f;
    public float dotEndMovingTime = 0f;


    void Update()
    {
        DotMovement();
    }

    void DotMovement()
    {
        if (poorPostureDetection.m_isPoorPosture && poorPostureDetection.poorPostureTime >= poorPostureDetection.poorPostureTimeThreshold)
        {
            if (gameObject.GetComponent<MeshRenderer>().enabled == false)
            {
                //dot.transform.parent = null;
                gameObject.GetComponent<Renderer>().material.SetColor("_Color", new Color(255, 0, 0));
                gameObject.GetComponent<MeshRenderer>().enabled = true;
            }

            //gameObject.transform.Translate(Vector3.up * dotSpeed * Time.deltaTime);
            dotStartMovingTime += Time.deltaTime;
            gameObject.transform.position = FindTargetPosition() + Vector3.up * dotSpeed * dotStartMovingTime;
            
            
            if (dotStartMovingTime >= dotEndMovingTime)
            {
                gameObject.transform.position = FindTargetPosition();
                dotStartMovingTime = 0f;
            }
        }
        if (!poorPostureDetection.m_isPoorPosture)
        {
            gameObject.GetComponent<Renderer>().material.SetColor("_Color", new Color(0, 255, 0));
            gameObject.transform.position = FindTargetPosition();
            StartCoroutine(WaitBeforeDisableDot());            
        }
    }

    private Vector3 FindTargetPosition()
    {
        return Camera.main.transform.position + (Camera.main.transform.forward * distance);
    }

    IEnumerator WaitBeforeDisableDot()
    {
        yield return new WaitForSeconds(1f);
        gameObject.GetComponent<MeshRenderer>().enabled = false;
    }

}
