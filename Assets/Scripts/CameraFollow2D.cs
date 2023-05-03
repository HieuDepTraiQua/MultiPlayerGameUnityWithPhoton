using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow2D : MonoBehaviour
{
    public float Speed = 25f;
    public float interpVelocity;
    public float minDistance;
    public GameObject target;
    public Vector3 offset;
    Vector3 targetPos;
    public Vector2 MinBoundary;
    public Vector2 MaxBoundary;

    private void Start() {
        targetPos = transform.position;
    }

    private void FixedUpdate() {
        if (target){
            Vector3 posNoZ = transform.position;
            posNoZ.z = target.transform.position.z;
            Vector3 targetDirection = (target.transform.position - posNoZ);
            interpVelocity = targetDirection.magnitude * Speed;
            targetPos = transform.position + (targetDirection.normalized * interpVelocity * Time.deltaTime);
            transform.position = Vector3.Lerp(transform.position, targetPos + offset, 0.25f);
        }
    }
}
