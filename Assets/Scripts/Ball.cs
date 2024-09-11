using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ball : MonoBehaviour
{
    public float speed = 5f;

    void Update()
    {
        Vector3 movement = new Vector3(Input.acceleration.x, Input.acceleration.y, 0);
        transform.position += movement * Time.deltaTime * speed;
    }
}