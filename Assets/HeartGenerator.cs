using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;

public class HeartGenerator : MonoBehaviour
{
    public FallMotion heartPrefab;
    [SerializeField] private RectTransform heartParent;

    private IEnumerator Start()
    {
        while (true)
        {
            Rect rect = heartParent.rect;
            float height = rect.height;
            float width = rect.width;
            FallMotion newHeart = Instantiate(heartPrefab, Vector3.Lerp(heartParent.TransformPoint(Vector3.zero),heartParent.TransformPoint(Vector3.right*width),Random.value)+Vector3.up*400,
                quaternion.identity, heartParent);
            newHeart.gameObject.SetActive(true);
            Destroy(newHeart.gameObject, 2*height / newHeart.direction.magnitude);
            yield return new WaitForSeconds(Random.Range(0.25f, 1f));
        }
    }
}