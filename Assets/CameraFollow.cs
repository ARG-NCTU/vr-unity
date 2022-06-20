using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [SerializeField] private Vector3 offset;
    [SerializeField] private Transform target;
    [SerializeField] private Transform camera;
    [SerializeField] private float translateSpeed;
    [SerializeField] private float rotationSpeed;

    private void FixedUpdate()
    {
        HandleTranslation();
        HandleRotation();
    }
   
    private void HandleTranslation()
    {
        var targetPosition = target.TransformPoint(offset);
        camera.transform.position = Vector3.Lerp(transform.position, targetPosition, translateSpeed * Time.deltaTime);
        Debug.Log("Translation " + camera.transform.position);
    }
    private void HandleRotation()
    {
        var direction = target.position - transform.position;
        var rotation = Quaternion.LookRotation(direction, Vector3.up);
        camera.transform.rotation = Quaternion.Lerp(transform.rotation, rotation, rotationSpeed * Time.deltaTime);
        Debug.Log("Translation " + camera.transform.rotation);
    }
}
