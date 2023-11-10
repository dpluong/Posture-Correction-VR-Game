using UnityEngine;

public class Head : MonoBehaviour
{
    public PoorPostureDetection poorPostureDetection;
    public Transform xrOrigin;
    
    void Update()
    {
        this.transform.position = new Vector3 (xrOrigin.position.x, xrOrigin.position.y + poorPostureDetection.GetHeight(), xrOrigin.position.z);
        this.transform.rotation = Camera.main.transform.rotation;
    }
}
