using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GravitationalBody : MonoBehaviour
{
    public Rigidbody rigidbody;

    static List<GravitationalBody> activeBodies = new List<GravitationalBody>();
    public static IReadOnlyList<GravitationalBody> gravitationalBodies => activeBodies;

    private void OnEnable()
    {
        activeBodies.Add(this);
    }

    private void OnDisable()
    {
        activeBodies.Remove(this);
    }

    private void FixedUpdate()
    {
        if (rigidbody.isKinematic)
            return;
        foreach (var otherBody in activeBodies)
        {
            if (otherBody == this)
                continue;
            rigidbody.AddForce(GetGravitationalForce(otherBody.rigidbody, rigidbody.position,rigidbody.mass));
        }
    }

    public static Vector3 GetGravitationalForce(Rigidbody other, Vector3 position, float mass)
    {
        Vector3 direction = (other.position - position);
        return direction * other.mass * mass / direction.sqrMagnitude;
    }
}
