using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PostureRing : MonoBehaviour
{
    [SerializeField] private Transform cameraTransform;
    [SerializeField] private float distance = 3.0f;

    public float speed = 1.0f;
    public float movingSpeed = 5.0f;

    void Start()
    {
        
    }

    bool IsCentered()
    {
        if (Vector3.Angle(cameraTransform.forward, transform.position - cameraTransform.position) > 0.1f)
            return false;

        return true;
    }

    private Vector3 FindTargetPosition()
    {
        return cameraTransform.position + (cameraTransform.forward * distance);
    }

    private void FaceTowardCamera()
    {
        Vector3 targetDirection = cameraTransform.position - transform.position;

        float singleStep = speed * Time.deltaTime;

        Vector3 newDirection = Vector3.RotateTowards(transform.forward, targetDirection, singleStep, 0.0f);

        transform.rotation = Quaternion.LookRotation(newDirection);
    }

    private void MoveTowardCamera()
    {
        float step = movingSpeed * Time.deltaTime;
        transform.position = Vector3.MoveTowards(transform.position, cameraTransform.position, step);
    }
    
    void Update()
    {
        FaceTowardCamera();

        if (!IsCentered())
        {
            MoveTowardCamera();
        }
    }
}
