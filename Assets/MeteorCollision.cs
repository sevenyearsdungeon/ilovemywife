using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeteorCollision : MonoBehaviour
{
    public static List<MeteorCollision> activeMeteors = new List<MeteorCollision>();

    [SerializeField] GameObject trailObject;
    [SerializeField] GameObject explosionPrefab;

    private void OnEnable()
    {
        activeMeteors.Add(this);
    }

    private void OnDisable()
    {
        activeMeteors.Remove(this);
    }

    public void OnCollisionEnter(Collision collision)
    {
        Vector3 explosionPoint;
        Quaternion explosionDirection;
        if (collision == null)
        {
            var rb = GetComponent<Rigidbody>();
            explosionPoint = rb.position;
            explosionDirection = Quaternion.LookRotation(rb.velocity);
        }
        else
        {
            explosionPoint = collision.GetContact(0).point;
            explosionDirection = Quaternion.LookRotation(collision.GetContact(0).normal);

        }

        var explosion = Instantiate(explosionPrefab, explosionPoint, explosionDirection);
        Destroy(explosion, 5);
        Destroy(this.gameObject);
        trailObject.transform.SetParent(null);
        Destroy(trailObject, trailObject.GetComponent<TrailRenderer>().time);

    }
}
