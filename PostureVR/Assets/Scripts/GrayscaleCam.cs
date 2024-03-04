using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

public class GrayscaleCam : MonoBehaviour
{
    public PoorPostureDetection poorPostureDetection;
    ColorGrading colorGradingLayer = null;
    PostProcessVolume postProcessVolume;

    private float poorPostureTimeThreshold;
    float time = 0f;

    void Start() 
    {
        postProcessVolume = Camera.main.GetComponent<PostProcessVolume>();
        poorPostureTimeThreshold = poorPostureDetection.poorPostureTimeThreshold;
        postProcessVolume.profile.TryGetSettings(out colorGradingLayer);
    }

    void Update()
    {
        if (poorPostureDetection.m_isPoorPosture && poorPostureDetection.poorPostureTime >= poorPostureTimeThreshold)
        {
            //postProcessVolume.profile.TryGetSettings(out colorGradingLayer);
            if (poorPostureDetection.poorPostureTime - poorPostureTimeThreshold >= 0.1f 
            && poorPostureDetection.poorPostureTime - poorPostureTimeThreshold <= 0.2f)
            {
                colorGradingLayer.saturation.value = -30f;
            }   
            else if (poorPostureDetection.poorPostureTime - poorPostureTimeThreshold > 1f)
            {
                if (colorGradingLayer.saturation.value > -100f)
                {
                    colorGradingLayer.saturation.value -= 0.01f;
                }
            }             
        }
        else
        {
            //postProcessVolume.profile.TryGetSettings(out colorGradingLayer);
            colorGradingLayer.saturation.value = 0;
        }
    }
}
