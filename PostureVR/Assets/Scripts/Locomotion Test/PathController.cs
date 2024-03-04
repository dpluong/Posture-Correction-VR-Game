using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathController : MonoBehaviour
{
    [SerializeField] List<Transform> points;

    public float radius = 0.5f;
    public LayerMask layerCollided;
    public DataCollection dataCollection;
    public GameObject scorePanel;

    private int score = 0;
    private int i = 0;
    
    void PathGeneration()
    {
        if (Physics.CheckSphere(points[i].position, radius, layerCollided))
        {
            if (dataCollection.startCollectingData == false)
            {
                dataCollection.startCollectingData = true;
            }
            points[i].gameObject.SetActive(false);

            if (i == points.Count - 1)
            {
                i = 0;
                points[i].gameObject.SetActive(true);
            }
            else
            {
                points[i + 1].gameObject.SetActive(true);
                i += 1;
            }

            score += 1;
            
        }
    }

    void DisplayFinalScore()
    {
        scorePanel.GetComponent<TMPro.TextMeshProUGUI>().text = score.ToString();
        scorePanel.transform.parent.gameObject.SetActive(true);
    }   

    void Update()
    {
        if (!dataCollection.endCollectingData)
        {
            PathGeneration();
        }
        else
        {
            DisplayFinalScore();
        }
    }
}
