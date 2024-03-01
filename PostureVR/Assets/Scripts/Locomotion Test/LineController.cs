using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LineController : MonoBehaviour
{
    [SerializeField] List<Transform> points;
    [SerializeField] Transform XROrigin;
    private LineRenderer lineRenderer;
    //private bool inSphere = false;

    public float radius = 0.5f;
    public LayerMask layerCollided;
    
    void Start()
    {
        lineRenderer = GetComponent<LineRenderer>();
    }

    void DrawLine(Vector3 current,  Vector3 next)
    {
        lineRenderer.positionCount = 2;
        lineRenderer.SetPosition(0, current);
        lineRenderer.SetPosition(1, next);
    }

    void PathController()
    {
        for (int i = 0; i < points.Count; ++i)
        {
            if (Physics.CheckSphere(points[i].position, radius, layerCollided))
            {
                if (i < points.Count - 1)
                {
                    DrawLine(points[i].position, points[i + 1].position);
                }
                else
                {
                    DrawLine(points[i].position, points[0].position);
                }
            }
        }
    }
    
    void Update()
    {
        PathController();
    }
}
