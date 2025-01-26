using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class GentleRotation : MonoBehaviour
{
    // Parameters to control the floaty movement
    public float floatAmplitude = 0.025f;
    public float floatSpeed = 1.0f;   
    public float rotationSpeed = 20f; 
    
    private Vector3 startPosition;

    void Start()
    {
        startPosition = transform.position;
    }

    void Update()
    {
        float floatY = Mathf.Sin(Time.time * floatSpeed) * floatAmplitude;
        
        transform.position = new Vector3(startPosition.x, startPosition.y + floatY, startPosition.z);
        
        transform.Rotate(Vector3.up * rotationSpeed * Time.deltaTime, Space.World);
    }
}
