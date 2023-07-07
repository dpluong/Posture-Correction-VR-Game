using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CompassManager : MonoBehaviour
{
    // Start is called before the first frame update
    public RawImage CompassImage;
    public GameObject SafeZone;
    public GameObject Head;

    private void LateUpdate() 
    {
        UpdateCompassArrow();
    }

    private void UpdateCompassArrow()
    {
        Vector3 DistanceVectorFromSafeZone = Head.transform.position - SafeZone.transform.position;
        float angle = Vector3.Angle(DistanceVectorFromSafeZone, Vector3.forward);

        Vector2 compassUvPosition = Vector2.right * (angle / 360f);
        CompassImage.uvRect = new Rect(compassUvPosition, Vector2.one);
    }
}
