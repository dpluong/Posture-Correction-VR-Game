using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;
using UnityEngine.UI;


public class PoorPostureDetection : MonoBehaviour
{
    public InputDeviceCharacteristics controllerCharacteristics;
    private InputDevice m_targetDevice;

    private float m_height;
    private float m_minHeight;
    private float m_neck;

    private bool m_isHeightRecorded = false;
    private bool m_isMinHeightRecorded = false;

    //public float heightThreshold;
    public float upperAngleThreshold;
    public float lowerAngleThreshold;


    public float holdAndReleaseTime;

    public bool m_isPoorPosture = false;

    public bool interventionTriggered = false;

    [SerializeField]
    GameObject angleValue;

    [SerializeField]
    GameObject slider;

    public float poorPostureTime = 0f;

    public float poorPostureTimeThreshold = 3f;

    void Start()
    {
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

    IEnumerator HoldButtonSlider()
    {
        float time = 0f;
        slider.SetActive(true);
        while (time < holdAndReleaseTime)
        {
            slider.GetComponent<Slider>().value = Mathf.Lerp(0f, 1f, time / holdAndReleaseTime);
            time += Time.deltaTime;
            yield return null;
        }

        slider.GetComponent<Slider>().value = 1f;
        slider.SetActive(false);
    }

    void RecordHeight()
    {
        m_targetDevice.TryGetFeatureValue(CommonUsages.triggerButton, out bool triggerValue);
        if (triggerValue)
        {
            StartCoroutine(HoldButtonSlider());
            m_height = Camera.main.transform.localPosition.y;
            m_isHeightRecorded = true;
        }
    }

    void RecordMinHeight()
    {
        m_targetDevice.TryGetFeatureValue(CommonUsages.gripButton, out bool gripValue);
        if (gripValue)
        {
            StartCoroutine(HoldButtonSlider());
            m_minHeight = Camera.main.transform.localPosition.y;
            m_neck = (m_height - m_minHeight) / (1f - Mathf.Cos(Camera.main.transform.eulerAngles.x * Mathf.Deg2Rad));
            m_isMinHeightRecorded = true;
            angleValue.SetActive(false);
        } 
    }

    float CalculateSafeHeight(float angle)
    {
        float angleRad = angle * Mathf.Deg2Rad;
        float safeHeight = m_height - m_neck + m_neck * Mathf.Cos(angleRad);
        return safeHeight;
    }

    void PostureDetection()
    {
        
        float currentHeight = Camera.main.transform.localPosition.y;

        if (Camera.main.transform.eulerAngles.x < upperAngleThreshold)
        {
            float angle = Mathf.Round(Camera.main.transform.eulerAngles.x);
            float safeHeight = CalculateSafeHeight(Camera.main.transform.eulerAngles.x);
        
            if (safeHeight - currentHeight >= 0.01f)
            {
                m_isPoorPosture = true;
            }
            else
            {
                m_isPoorPosture = false;
            }
        }
        else
        {
            m_isPoorPosture = true;
        }

        if (Camera.main.transform.eulerAngles.x > lowerAngleThreshold)
        {
            if (m_height > currentHeight)
            {
                m_isPoorPosture = true;
            }
            else
            {
                m_isPoorPosture = false;
            }
        }
    }

    void DisplayTiltAngle()
    {
        float angle = Mathf.Round(Camera.main.transform.eulerAngles.x);
        angleValue.GetComponent<TMPro.TextMeshProUGUI>().text = angle.ToString();
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

            if (m_isHeightRecorded && !m_isMinHeightRecorded )
            {
                RecordMinHeight();
            }
            
            if (m_isHeightRecorded && m_isMinHeightRecorded)
            {
                
                PostureDetection();
            }
            else
            {
                DisplayTiltAngle();
            }
            
        }

        if (m_isPoorPosture)
        {
            poorPostureTime += Time.deltaTime;
        }
        else
        {
            poorPostureTime = 0f;
        }


    }
}
