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

    //public float heightThreshold;
    public float upperAngleThreshold;
    public float lowerAngleThreshold;


    public float holdAndReleaseTime;
    private float holdTimerTrigger;
    private float holdTimerGrip;

    private bool m_isPoorPosture = false;
    public bool isPoorPostureNotified = false;

    [SerializeField]
    private GameObject Screen;

    [SerializeField]
    GameObject uiAngleValue;

    [SerializeField]
    GameObject dot;

    private Vector3 dotInitialPosition;
    public float dotSpeed;
    private float dotPosition = 0f;

    public GameObject center;

    void Start()
    {
        holdTimerTrigger = holdAndReleaseTime;
        holdTimerGrip = holdAndReleaseTime;
        TryInitialize();
        dotInitialPosition = dot.transform.localPosition;
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
                m_height = Camera.main.transform.position.y;
                m_isHeightRecorded = true;
            }
        }
    }

    void RecordMinHeight()
    {
        if (m_targetDevice.TryGetFeatureValue(CommonUsages.gripButton, out bool triggerValue) && triggerValue)
        {
            holdTimerGrip -= Time.deltaTime;
            if (holdTimerGrip < 0)
            {
                m_minHeight = Camera.main.transform.position.y;
                m_isMinHeightRecorded = true;
            }
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
        
        float currentHeight = Camera.main.transform.position.y;

        if (Camera.main.transform.eulerAngles.x < upperAngleThreshold || Camera.main.transform.eulerAngles.x > lowerAngleThreshold)
        {
            float angle = Mathf.Round(Camera.main.transform.eulerAngles.x);
            float safeHeight = CalculateSafeHeight(Camera.main.transform.eulerAngles.x);
            Debug.Log("Safe height: " + safeHeight);
            uiAngleValue.GetComponent<TMPro.TextMeshProUGUI>().text = "Safe height: " + safeHeight.ToString() + " Current height: " + currentHeight.ToString()
                                                                    + " Angle: " + angle;
            if (safeHeight - currentHeight >= 0.0115f)
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

        if (m_isPoorPosture)
        {
            Screen.GetComponent<Renderer>().material.color = new Color(0, 0, 0);
        }
        else
        {
            Screen.GetComponent<Renderer>().material.color = new Color(1, 1, 1);
        }
    }

    void DisplayTiltAngle()
    {
        float angle = Mathf.Round(Camera.main.transform.eulerAngles.x);
        uiAngleValue.GetComponent<TMPro.TextMeshProUGUI>().text = angle.ToString();
    }

    void DotMovement()
    {
        center.transform.position = Camera.main.transform.position;
        if (isPoorPostureNotified)
        {
            dot.SetActive(true);
            dot.transform.parent = null;
            float dotStep = dotSpeed * Time.deltaTime;
            Vector3 targetPosition = new Vector3(dot.transform.position.x, 2f, dot.transform.position.z);
            dot.transform.position = Vector3.MoveTowards(dot.transform.position, targetPosition, dotStep);
        }
        else
        {
            center.transform.rotation = Camera.main.transform.rotation;
            dot.transform.parent = Camera.main.gameObject.transform;
            dot.transform.localPosition = dotInitialPosition;
            dot.SetActive(false);
        }
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
        DotMovement();
    }
}
