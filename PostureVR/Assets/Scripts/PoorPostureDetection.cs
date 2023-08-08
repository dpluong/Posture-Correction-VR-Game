using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;
using UnityEngine.UI;
using Unity.XR.CoreUtils;

public class PoorPostureDetection : MonoBehaviour
{
    // Start is called before the first frame update
    public XROrigin XROrigin;
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
    private float holdTimerTrigger;
    private float holdTimerGrip;

    public bool m_isPoorPosture = false;

    [SerializeField]
    private GameObject Screen;

    [SerializeField]
    GameObject uiAngleValue;

    [SerializeField]
    GameObject dot;

    [SerializeField]
    GameObject destination;

    [SerializeField]
    GameObject slider;

    private Vector3 dotInitialPosition;
    private Vector3 dotInitialRotation;

    public float dotSpeed;

    public float poorPostureTime = 0f;
    private float dotStartMovingTime = 0f;
    public float dotEndMovingTime = 0f;

    public float poorPostureTimeThreshold = 3f;

    void Start()
    {
        holdTimerTrigger = holdAndReleaseTime;
        holdTimerGrip = holdAndReleaseTime;
        TryInitialize();
        dotInitialPosition = dot.transform.localPosition;
        dotInitialRotation = dot.transform.localEulerAngles;
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
            holdTimerTrigger -= Time.deltaTime;
            if (holdTimerTrigger < 0)
            {
                m_height = Camera.main.transform.localPosition.y;
                m_isHeightRecorded = true;
            }
            StartCoroutine(HoldButtonSlider());
        }
    }

    void RecordMinHeight()
    {
        if (m_targetDevice.TryGetFeatureValue(CommonUsages.gripButton, out bool triggerValue) && triggerValue)
        {
            holdTimerGrip -= Time.deltaTime;
            if (holdTimerGrip < 0)
            {
                m_minHeight = Camera.main.transform.localPosition.y;
                m_isMinHeightRecorded = true;
            }
            StartCoroutine(HoldButtonSlider());
        }
        m_neck = (m_height - m_minHeight) / (1f - Mathf.Cos(Camera.main.transform.eulerAngles.x * Mathf.Deg2Rad));
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

        if (Camera.main.transform.eulerAngles.x < upperAngleThreshold || Camera.main.transform.eulerAngles.x > lowerAngleThreshold)
        {
            float angle = Mathf.Round(Camera.main.transform.eulerAngles.x);
            float safeHeight = CalculateSafeHeight(Camera.main.transform.eulerAngles.x);
            uiAngleValue.GetComponent<TMPro.TextMeshProUGUI>().text = "Safe height: " + safeHeight.ToString() + " Current height: " + currentHeight.ToString()
                                                                    + " Angle: " + angle;
            if (safeHeight - currentHeight >= 0.013f)
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

        /*
        if (m_isPoorPosture)
        {
            Screen.GetComponent<Renderer>().material.color = new Color(0, 0, 0);
        }
        else
        {
            Screen.GetComponent<Renderer>().material.color = new Color(1, 1, 1);
        }*/
    }

    void DisplayTiltAngle()
    {
        float angle = Mathf.Round(Camera.main.transform.eulerAngles.x);
        uiAngleValue.GetComponent<TMPro.TextMeshProUGUI>().text = angle.ToString();
    }

    public bool IsPoorPosture()
    {
        return m_isPoorPosture;
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
            dotStartMovingTime = 0f;
        }
    }
}
