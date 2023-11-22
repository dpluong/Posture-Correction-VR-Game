using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SurveyController : MonoBehaviour
{
    [SerializeField] GameObject surveyPanel;

    public SaveSurveyData saveData;

    void CleanOldSurveyData()
    {
        saveData.isGatheringSurvey = false;
    }

    void OnClickSaveButton()
    {
        // Loop through sliders and send data over to SaveSurveyData.cs
        Slider[] listOfSliders = surveyPanel.GetComponentsInChildren<Slider>();
        int scoreArrayLength = listOfSliders.Length;
        float[] scores = new float[scoreArrayLength];
        for (int i = 0; i < scoreArrayLength; ++i)
        {
            scores[i] = listOfSliders[i].value;
        }
        saveData.WriteCSV(scores);
        CleanOldSurveyData();

    }

    void Update()
    {
        if (saveData.isGatheringSurvey)
        {
            surveyPanel.SetActive(true);
        }
    }
}
