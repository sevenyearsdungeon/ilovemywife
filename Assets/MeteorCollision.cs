using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeteorCollision : MonoBehaviour
{
    [SerializeField] GameObject trailObject;
    [SerializeField] GameObject explosionPrefab;
    private void OnCollisionEnter(Collision collision)
    {
        var explosion = Instantiate(explosionPrefab,collision.GetContact(0).point,Quaternion.LookRotation(collision.GetContact(0).normal));
        Destroy(explosion, 5);
        Destroy(this.gameObject);
        trailObject.transform.SetParent(null);
        Destroy(trailObject, trailObject.GetComponent<TrailRenderer>().time);

    }
}
