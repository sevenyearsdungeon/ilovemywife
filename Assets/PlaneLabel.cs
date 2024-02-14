using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlaneLabel : MonoBehaviour
{
    [SerializeField]
    Transform myPlaneTransform;
    [SerializeField]
    RectTransform myRectTransform;
    Camera mainCamera;

    void Start()
    {
        mainCamera = Camera.main;    
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 screenPosition = mainCamera.WorldToScreenPoint(myPlaneTransform.position);
        myRectTransform.anchoredPosition = screenPosition;
    }
}
