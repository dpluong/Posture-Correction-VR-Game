using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class DataCollection : MonoBehaviour
{
    public PoorPostureDetection poorPostureDetection;

    string filename = "";

    public string playerName;

    public int timesPerSecond;

    [System.Serializable]

    public class Player
    {
        public float height;
        public float angle;
        public int postureState;
        public int interventionTriggered; 
    }

    [SerializeField]
    public List<Player> player;

    private float timer = 0f;

    public bool startCollectingData = false;

    public bool endCollectingData = false;
    
    void Start()
    {
        filename = Application.dataPath + "/" + playerName + ".csv";
    }

    void CollectUserData()
    {
        if (startCollectingData)
        {
            timer += Time.fixedDeltaTime;
            if (timer > 1f / timesPerSecond)
            {
                timer = 0f;
                Player playerData = new Player();
                playerData.height = Camera.main.transform.localPosition.y;
                playerData.angle = Camera.main.transform.eulerAngles.x;
                playerData.postureState = poorPostureDetection.m_isPoorPosture ? 1 : 0;
                playerData.interventionTriggered = poorPostureDetection.interventionTriggered ? 1 : 0;
                player.Add(playerData);
            }
        }
    }

    void WriteCSV()
    {
        if (player.Count > 0)
        {
            TextWriter tw = new StreamWriter(filename, false);
            tw.WriteLine("Height, Angle, State, Intervention");
            tw.Close();

            tw = new StreamWriter(filename, true);

            for (int i = 0; i < player.Count; ++i)
            {
                tw.WriteLine(player[i].height + "," + player[i].angle + "," + player[i].postureState + "," + player[i].interventionTriggered);
            }
            tw.Close();
        }
    }

    void FixedUpdate()
    {
        CollectUserData();
        if (endCollectingData)
        {
            WriteCSV();
        }
    }
}
