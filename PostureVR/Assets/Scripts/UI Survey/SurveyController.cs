using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;

public class SurveyController : MonoBehaviour
{
    [SerializeField] GameObject surveyPanel;
    public GameObject RightHandRay;
    public SaveSurveyData saveData;
    public InputActionReference toggleSurvey = null;

    private void Awake() 
    {
        toggleSurvey.action.started += ToggleUISurvey;
    }

    private void OnDestroy() 
    {
        toggleSurvey.action.started -= ToggleUISurvey;
    }

    void ToggleUISurvey(InputAction.CallbackContext context)
    {
        bool isActive = !surveyPanel.activeSelf;
        surveyPanel.SetActive(isActive);
        bool isRayActive = !RightHandRay.activeSelf;
        RightHandRay.SetActive(isActive);
    }

    void CleanOldSurveyData()
    {
        Slider[] listOfSliders = surveyPanel.GetComponentsInChildren<Slider>();
        for (int i = 0; i < listOfSliders.Length; ++i)
        {
            listOfSliders[i].value = 0f;
        }
        RightHandRay.SetActive(false);
        surveyPanel.SetActive(false);
    }

    public void OnClickSaveButton()
    {
        // Loop through sliders and send data over to SaveSurveyData.cs
        Slider[] listOfSliders = surveyPanel.GetComponentsInChildren<Slider>();
        int scoreArrayLength = listOfSliders.Length;
        float[] scores = new float[scoreArrayLength];
        for (int i = 0; i < scoreArrayLength; ++i)
        {
            scores[i] = listOfSliders[i].value;
        }

        Dropdown interventionType = surveyPanel.GetComponentInChildren<Dropdown>();
        saveData.WriteCSV(scores, interventionType.options[interventionType.value].text);
        CleanOldSurveyData();

    }

}
