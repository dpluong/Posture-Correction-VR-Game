using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Plants : MonoBehaviour
{    
    public List<Material> materials;

    public PoorPostureDetection postureDetectionMethod;

    public float duration;

    private float[] startWindStrength;

    private bool isWindStrengthIncreased = false;

    //private bool isInCoroutine = false;

    public float timeToChangePosture = 3f;

    public AudioSource audioSource;

    public Light sunLight;

    public Material skybox;

    float time;

    IEnumerator IncreaseWindStrength()
    {
        //isInCoroutine = true;
        yield return new WaitForSeconds(timeToChangePosture);
        while (time < duration)
        {
            for (int matIndex = 0; matIndex < materials.Count; ++matIndex)
            {
                float windStrength = Mathf.Lerp(startWindStrength[matIndex], 1f, time / duration);
                materials[matIndex].SetFloat("_WindStrength", windStrength);
            }
            time += Time.deltaTime * 0.5f;
            yield return new WaitForSeconds(1f);
        }

        if (time >= duration)
        {
            for (int matIndex = 0; matIndex < materials.Count; ++matIndex)
            {
                materials[matIndex].SetFloat("_WindStrength", 1f);
            }
        }

        isWindStrengthIncreased = true;
    }

    void DecreaseWindStrength()
    {
        time = 0f;
        while (time < duration)
        {
            for (int matIndex = 0; matIndex < materials.Count; ++matIndex)
            {
                float windStrength = Mathf.Lerp(1f, startWindStrength[matIndex], time / duration);
                materials[matIndex].SetFloat("_WindStrength", windStrength);
            }
            time += Time.deltaTime * 0.5f;
            //yield return new WaitForSeconds(1f);
        }
    }

    void ResetWindStrength()
    {
        for (int matIndex = 0; matIndex < materials.Count; ++matIndex)
        {
            materials[matIndex].SetFloat("_WindStrength", startWindStrength[matIndex]);
        }
    }

    void InitWindStrengthValues()
    {
        for (int matIndex = 0; matIndex < materials.Count; ++matIndex)
        {
            startWindStrength[matIndex] = 0.1f;
            materials[matIndex].SetFloat("_WindStrength", startWindStrength[matIndex]);
        }
    }

    void Start()
    {
        startWindStrength = new float[materials.Count];
        time = 0f;
        InitWindStrengthValues();
    }
    // Update is called once per frame
    void Update()
    {
        if (postureDetectionMethod.m_isPoorPosture)
        {
            StartCoroutine(IncreaseWindStrength());
        }
        else
        {
            if (isWindStrengthIncreased)
            {
                DecreaseWindStrength();
                ResetWindStrength();
                isWindStrengthIncreased = false;
            }
        }
    }
}
