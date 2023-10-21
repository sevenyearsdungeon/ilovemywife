using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations;

public class ShotPreviewRenderer : MonoBehaviour
{
    [SerializeField] LineRenderer lr;
    const int positionCount = 100;
    Vector3[] positions = new Vector3[positionCount];
    private void Awake()
    {
        lr.SetPositions(positions);
    }

    internal void CalculatePreview(Vector3 initialPosition, Vector3 initialVelocity, float mass)
    {
        Vector3 previousPosition = initialPosition;
        Vector3 currentPosition = initialPosition;
        Vector3 currentVelocity = initialVelocity;
        Vector3 previousForce = Vector3.zero;
        positions[0] = currentPosition;

        for (int i = 1; i < positionCount; i++)
        {
            Vector3 totalforce = previousForce;
            foreach (var body in GravitationalBody.gravitationalBodies)
            {
                totalforce += GravitationalBody.GetGravitationalForce(body.rigidbody, currentPosition, mass);
            }
            previousForce = totalforce;

            Vector3 acceleration = totalforce / mass;

            previousPosition = currentPosition;
            currentPosition = currentPosition + currentVelocity * Time.fixedDeltaTime + acceleration / 2 * Time.fixedDeltaTime * Time.fixedDeltaTime;
            if(Physics.Raycast(new Ray(previousPosition, currentPosition-previousPosition),out RaycastHit hit,(currentPosition-previousPosition).magnitude))
            {
                for (int j = i; j < positionCount; j++)
                {
                    positions[j] = hit.point;
                }
                break;
            }
            positions[i] = currentPosition;
        }
        lr.SetPositions(positions);
    }

    internal void Hide()
    {
        lr.enabled = false;
    }

    internal void Show()
    {
        lr.enabled = true;
    }
}
