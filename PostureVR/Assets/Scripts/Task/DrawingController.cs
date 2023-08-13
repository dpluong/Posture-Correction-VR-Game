using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;

public class DrawingController : MonoBehaviour
{
    public InputDeviceCharacteristics controllerCharacteristics;
    private InputDevice m_targetDevice;
    // Start is called before the first frame update
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

    void ShowDrawing()
    {
        m_targetDevice.TryGetFeatureValue(CommonUsages.triggerButton, out bool gripValue);
        if(gripValue)
        {
            
            gameObject.GetComponent<SpriteRenderer>().enabled = true;
        }
        else
        {
            gameObject.GetComponent<SpriteRenderer>().enabled = false;
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
            ShowDrawing();
    }
}
