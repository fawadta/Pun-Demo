using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Webgl : MonoBehaviour
{
    public float speed = 1f; // Frequency of movement
    public float height = 1f; // Amplitude of movement

    void Update()
    {
        // Adjust the y position using Mathf.Sin
        float newY = Mathf.Sin(Time.time * speed) * height;
        transform.position = new Vector3(transform.position.x, newY, transform.position.z);
    }
}
