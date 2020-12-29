﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RatBehaviour : MonoBehaviour
{
    public float RatRandomness = 10;
    public float RatSpeed = 1;

    private const int MAX_DEGREES = 360;
    private Quaternion _displacement;
    void Start()
    {
        _displacement = Quaternion.AngleAxis(Random.Range(0, MAX_DEGREES), Vector3.forward);
    }

    void FixedUpdate()
    {
        _displacement *= Quaternion.AngleAxis(Random.Range(-RatRandomness, RatRandomness), Vector3.forward);
        Vector3 dx = _displacement * Vector3.right * Time.deltaTime * RatSpeed;
        Vector3.MoveTowards(transform.position, transform.position + dx, RatSpeed * Time.deltaTime);
        transform.position = transform.position + dx;
        
        //flip orientation based on motion
        Vector3 scale = transform.localScale;
        scale.x = dx.x > 0 ? 1 : -1;
        transform.localScale = scale;
    }
    private void OnTriggerEnter2D(Collider2D collider) {
        Vector3 newDirection = transform.position - collider.transform.position;
        _displacement.SetFromToRotation(Vector2.right, newDirection);
    }
}