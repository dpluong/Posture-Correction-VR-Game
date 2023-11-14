using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

public class GrayscaleCam : MonoBehaviour
{
    public PoorPostureDetection poorPostureDetection;
    ColorGrading colorGradingLayer = null;
    PostProcessVolume postProcessVolume;

    void Start() 
    {
        postProcessVolume = Camera.main.GetComponent<PostProcessVolume>();
    }

    void Update()
    {
        if (poorPostureDetection.m_isPoorPosture && poorPostureDetection.poorPostureTime >= poorPostureDetection.poorPostureTimeThreshold)
        {
            
            postProcessVolume.profile.TryGetSettings(out colorGradingLayer);
            colorGradingLayer.saturation.value = -100f;
        }
        else
        {
            postProcessVolume.profile.TryGetSettings(out colorGradingLayer);
            colorGradingLayer.saturation.value = 0;
        }
    }
}
