using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class SaveSurveyData : MonoBehaviour
{
    string filename = "";

    public enum InterventionType
    {
        Icon,
        Dot,
        Grayscale,
        Circle
    }

    public bool isGatheringSurvey;
    public string playerName;
    public InterventionType interventionType;


    public void SaveData()
    {
        filename = Application.dataPath + "/" + "Survey" + "/" + playerName + ".csv";
    }

    public void WriteCSV(float[] scores)
    {
        TextWriter tw;
        if (!new FileInfo(filename).Exists)
        {
            tw = new StreamWriter(filename, false);
            tw.WriteLine("Height, Angle, State, IsTriggered, Intervention");
            tw.Close();
        }

        tw = new StreamWriter(filename, true);

        
        tw.WriteLine(scores[0] + "," + scores[1] + "," + scores[2] + "," + interventionType);

        tw.Close();
    }
}
