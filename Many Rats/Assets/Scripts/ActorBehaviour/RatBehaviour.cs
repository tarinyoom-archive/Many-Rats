﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RatBehaviour : MonoBehaviour
{
    [SerializeField] private float _ratRandomness = 10;
    [SerializeField] private float _ratSpeed = 1;

    //for forming rat hordes
    [SerializeField] private float _ratDetectorRadius = 20;
    [SerializeField] private int _numberOfRatsForHorde = 5;
    [SerializeField] private GameObject ratHordePrefab;

    private RatDetector _ratDetector;
    private const int MAX_DEGREES = 360;
    private Quaternion _direction;

    private void SummonHorde(List<GameObject> rats) {
        EventManager.GetInstance().InvokeRatHordeSpawnRequestEvent(transform.position);
        for (int i = 0; i < rats.Count; i++) {
            if (rats[i] != gameObject) {
                rats[i].GetComponent<RatBehaviour>().Die(); 
            }
        }
        Die(); // :(
    }

    void Start()
    {
        _ratDetector = GetComponentInChildren<RatDetector>();
        _ratDetector.GetComponent<CircleCollider2D>().radius = _ratDetectorRadius;
        _direction = Quaternion.AngleAxis(Random.Range(0, MAX_DEGREES), Vector3.forward);

    }
    private void Update()
    {
        if (_ratDetector.NumNearbyRats() >= _numberOfRatsForHorde) {
            SummonHorde(_ratDetector.NearbyRats());
        }
    }
    void FixedUpdate()
    {
        //randomize direction slightly
        _direction *= Quaternion.AngleAxis(Random.Range(-_ratRandomness, _ratRandomness), Vector3.forward);
        
        //move rat
        Vector3 directionVector = _direction * Vector3.right;
        transform.position = Vector2.MoveTowards(transform.position, transform.position + directionVector, _ratSpeed * Time.deltaTime);

        //flip orientation based on motion
        Vector3 scale = transform.localScale;
        scale.x = directionVector.x > 0 ? 1 : -1;
        transform.localScale = scale;
    }

    private void OnCollisionEnter2D(Collision2D collision) {
        _direction.SetFromToRotation(Vector3.right, transform.position - (Vector3)collision.GetContact(0).point);
    }

    public void Die() {
        _ratDetector.Die();
        Destroy(gameObject);
    }
}
