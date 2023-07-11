using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;

public class PoorPostureDetection : MonoBehaviour
{
    // Start is called before the first frame update
    public InputDeviceCharacteristics controllerCharacteristics;
    private InputDevice m_targetDevice;

    private float m_height;
    private float m_tiltAngle;
    private bool m_isHeightRecorded = false;

    public float heightThreshold;
    public float angleThreshold;

    public float holdAndReleaseTime;
    private float holdTimer;

    private bool m_isPoorPosture = false;

    [SerializeField]
    private GameObject Screen;
    

    void Start()
    {
        holdTimer = holdAndReleaseTime;
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
                m_height = Mathf.Ceil(Camera.main.transform.position.y * 100f);
                m_height = m_height / 100f;
                m_isHeightRecorded = true;
            }
        }
    }

    void PostureDetection()
    {
        float CurrentHeight = Mathf.Floor(Camera.main.transform.position.y * 100f);
        CurrentHeight = CurrentHeight / 100f;

        if (m_height - CurrentHeight < (heightThreshold * 2) && Camera.main.transform.eulerAngles.x < angleThreshold)
        {
            m_isPoorPosture = false;
            Screen.GetComponent<Renderer>().material.color = new Color(1, 1, 1);
            return;
        }

        if ((Camera.main.transform.eulerAngles.x >= angleThreshold && Camera.main.transform.eulerAngles.x <= 50f))
        {
            m_isPoorPosture = true;
        }

        if (m_height - CurrentHeight >= heightThreshold)
        {
            m_isPoorPosture = true;
            Screen.GetComponent<Renderer>().material.color = new Color(0, 0, 0);
        }
        else
        {
            m_isPoorPosture = false;
            Screen.GetComponent<Renderer>().material.color = new Color(1, 1, 1);
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
            else
            {
                PostureDetection();
            }
            
        }
    }
}
