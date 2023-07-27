using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VisualNotifications : MonoBehaviour
{
    // Start is called before the first frame update

    public List<Material> materials;

    public PoorPostureDetection postureDetectionMethod;

    public float duration;

    private float[] startWindStrength;

    float time;

    void IncreaseWindStrength()
    {
        while (time < duration)
        {
            for (int matIndex = 0; matIndex < materials.Count; ++matIndex)
            {
                float windStrength = Mathf.Lerp(startWindStrength[matIndex], 1f, time / duration);
                materials[matIndex].SetFloat("_WindStrength", windStrength);
            }
            time += Time.deltaTime * 0.5f;
        }

        if (time >= duration)
        {
            for (int matIndex = 0; matIndex < materials.Count; ++matIndex)
            {
                materials[matIndex].SetFloat("_WindStrength", 1f);
            }
        }
    }

    void DecreaseWindStrength()
    {
        while (time < duration)
        {
            for (int matIndex = 0; matIndex < materials.Count; ++matIndex)
            {
                float windStrength = Mathf.Lerp(1f, startWindStrength[matIndex], time / duration);
                materials[matIndex].SetFloat("_WindStrength", windStrength);
            }
            time += Time.deltaTime * 0.5f;
        }
    }

    void ResetWindStrength()
    {
        for (int matIndex = 0; matIndex < materials.Count; ++matIndex)
        {
            materials[matIndex].SetFloat("_WindStrength", startWindStrength[matIndex]);
            time = 0f;
        }
    }

    void InitWindStrengthValues()
    {
        for (int matIndex = 0; matIndex < materials.Count; ++matIndex)
        {
            startWindStrength[matIndex] = materials[matIndex].GetFloat("_WindStrength");
            time = 0f;
        }
    }

    void Start()
    {
        startWindStrength = new float[materials.Count];
        InitWindStrengthValues();
    }
    // Update is called once per frame
    void Update()
    {
        if (postureDetectionMethod.m_isPoorPosture)
        {
            IncreaseWindStrength();
        }
        else
        {
            time = 0f;
            DecreaseWindStrength();
        }
    }

    private void OnApplicationQuit() 
    {
        ResetWindStrength();
    }
}
