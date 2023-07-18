using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;
using TMPro;

public class PoorPostureDetection : MonoBehaviour
{
    // Start is called before the first frame update
    public InputDeviceCharacteristics controllerCharacteristics;
    private InputDevice m_targetDevice;

    private float m_height;
    private float m_minHeight;
    private float m_neck;

    private bool m_isHeightRecorded = false;
    private bool m_isMinHeightRecorded = false;

    public float heightThreshold;
    public float angleThreshold;

    public float holdAndReleaseTime;
    private float holdTimer;
    private float holdTimerGrip;

    private bool m_isPoorPosture = false;

    [SerializeField]
    private GameObject Screen;

    [SerializeField]
    GameObject uiAngleValue;
    

    void Start()
    {
        holdTimer = holdAndReleaseTime;
        holdTimerGrip = holdAndReleaseTime;
        TryInitialize();
    }

    void TryInitialize()
    {
        List<InputDevice> devices = new List<InputDevice>();

        InputDevices.GetDevicesWithCharacteristics(controllerCharacteristics, devices);
        if (devices.Count > 0)
        {
            m_targetDevice = devices[0];
        }
    }

    void RecordHeight()
    {
        if (m_targetDevice.TryGetFeatureValue(CommonUsages.triggerButton, out bool triggerValue) && triggerValue)
        {
            holdTimer -= Time.deltaTime;
            if (holdTimer < 0)
            {
                //m_height = Mathf.Ceil(Camera.main.transform.position.y * 100f);
                //m_height = m_height / 100f;
                m_height = Camera.main.transform.position.y;
                m_isHeightRecorded = true;
            }
        }
    }

    void PostureDetection()
    {
        float CurrentHeight = Mathf.Floor(Camera.main.transform.position.y * 100f);
        CurrentHeight = CurrentHeight / 100f;        

        if ((Camera.main.transform.eulerAngles.x >= angleThreshold && Camera.main.transform.eulerAngles.x <= 50f))
        {
            m_isPoorPosture = true;
        }

        if (m_height - CurrentHeight >= heightThreshold && (Camera.main.transform.eulerAngles.x < 3f && Camera.main.transform.eulerAngles.x > 357f))
        {
            m_isPoorPosture = true;
        }
        
        if (Camera.main.transform.eulerAngles.x < angleThreshold)
        {
            m_isPoorPosture = false;
            // if (m_height - CurrentHeight >= heightThreshold)
            // time count 
        }

        if (m_height - CurrentHeight >= heightThreshold * 1.5f)
        {
            m_isPoorPosture = true;
        }

        if (m_height - CurrentHeight < heightThreshold)
        {
            m_isPoorPosture = false;
        }

        if (m_isPoorPosture)
        {
            Screen.GetComponent<Renderer>().material.color = new Color(0, 0, 0);
        }
        else
        {
            Screen.GetComponent<Renderer>().material.color = new Color(1, 1, 1);
        }
    }

    float CalculateSafeHeight(float angle)
    {
        float angleRad = angle * Mathf.Deg2Rad;
        Debug.Log("Values:");
        Debug.Log("Euler angle: " + angle);
        Debug.Log("Radian angle: " + angleRad);
        Debug.Log("My height: " + m_height);
        Debug.Log("My min height: " + m_minHeight);
        
        float safeHeight = m_height - m_neck + m_neck * Mathf.Cos(angleRad);
        //safeHeight = Mathf.Ceil(safeHeight * 100f);
        //safeHeight = safeHeight / 100f;
        
        return safeHeight;
    }

    void PostureDetection2()
    {
        /*
        if ((Camera.main.transform.eulerAngles.x >= angleThreshold && Camera.main.transform.eulerAngles.x <= 50f))
        {
            m_isPoorPosture = true;
        }
        else
        {
            m_isPoorPosture = false;
        }*/

        //float currentHeight = Mathf.Floor(Camera.main.transform.position.y * 100f);
        //currentHeight = currentHeight / 100f;

        float currentHeight = Camera.main.transform.position.y;

        if (Camera.main.transform.eulerAngles.x < angleThreshold)
        {
            float angle = Mathf.Round(Camera.main.transform.eulerAngles.x);
            float safeHeight = CalculateSafeHeight(Camera.main.transform.eulerAngles.x);
            Debug.Log("Safe height: " + safeHeight);
            uiAngleValue.GetComponent<TMPro.TextMeshProUGUI>().text = "Safe height: " + safeHeight.ToString() + " Current height: " + currentHeight.ToString()
                                                                    + " Angle: " + angle;
            if (safeHeight - currentHeight >= 0.015f)
            {
                m_isPoorPosture = true;
            }
            else
            {
                m_isPoorPosture = false;
            }
        }

        if (m_isPoorPosture)
        {
            Screen.GetComponent<Renderer>().material.color = new Color(0, 0, 0);
        }
        else
        {
            Screen.GetComponent<Renderer>().material.color = new Color(1, 1, 1);
        }
    }

    void RecordMinHeight()
    {
        if (m_targetDevice.TryGetFeatureValue(CommonUsages.gripButton, out bool triggerValue) && triggerValue)
        {
            holdTimerGrip -= Time.deltaTime;
            if (holdTimerGrip < 0)
            {
                m_minHeight = Mathf.Ceil(Camera.main.transform.position.y * 100f);
                m_minHeight = m_minHeight / 100f;
                m_isMinHeightRecorded = true;
            }
        }
        m_neck = (m_height - m_minHeight) / (1f - Mathf.Cos(Camera.main.transform.eulerAngles.x * Mathf.Deg2Rad));
    }

    void DisplayHeadMovementValues()
    {
        float angle = Mathf.Round(Camera.main.transform.eulerAngles.x);
        uiAngleValue.GetComponent<TMPro.TextMeshProUGUI>().text = angle.ToString();
    }

    // Update is called once per frame
    void Update()
    {
        if (!m_targetDevice.isValid)
        {
            TryInitialize();
        }
        else
        {
            if (!m_isHeightRecorded)
            {
                RecordHeight();
            }

            if (!m_isMinHeightRecorded )
            {
                RecordMinHeight();
            }
            
            if (m_isHeightRecorded && m_isMinHeightRecorded)
            {
                PostureDetection2();
            }
            else
            {
                DisplayHeadMovementValues();
            }
            
        }
        
    }
}
