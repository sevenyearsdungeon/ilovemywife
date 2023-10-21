using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BinaryStarController : MonoBehaviour
{
    private float distance = 0;
    [SerializeField] Transform sun1, sun2;
    void Update()
    {
        sun1.transform.position = Vector3.Lerp(sun1.transform.position, Vector3.right * distance / 2, Time.deltaTime);
        sun2.transform.position = Vector3.Lerp(sun2.transform.position, Vector3.left * distance / 2, Time.deltaTime);
    }
    public void SetBinaryDistance(float f) => distance = f;
}
