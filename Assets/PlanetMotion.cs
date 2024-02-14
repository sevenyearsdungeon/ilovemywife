using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlanetMotion : MonoBehaviour
{
    public Transform orbitCenter;
    public float radius;
    public float speed;

    private void OnEnable()
    {
        UpdatePosition();
        GetComponent<TrailRenderer>().Clear();
    }
    // Update is called once per frame
    void Update()
    {
        UpdatePosition();
    }

    private void UpdatePosition()
    {
        Vector3 origin = Vector3.zero;
        if (orbitCenter != null)
            origin = orbitCenter.position;

        transform.position = origin + new Vector3(Mathf.Cos(Time.time * speed), Mathf.Sin(Time.time * speed)) * radius;
    }
}
