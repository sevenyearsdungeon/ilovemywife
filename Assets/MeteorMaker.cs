using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class MeteorMaker : MonoBehaviour, IPointerDownHandler
{
    [SerializeField] Rigidbody prefabAsteroid;
    [SerializeField] Camera mainCamera;
    [SerializeField] Image arrow;
    [SerializeField] float maxPower = 5;
    [SerializeField] float minPower = 1;
    [SerializeField] Gradient powerColorGradient;
    [SerializeField] ShotPreviewRenderer shotPreviewRenderer;


    private void Start()
    {
        arrow.enabled = false;
    }

    IEnumerator LaunchMeteorCoroutine(Vector3 initialPosition, Vector3 initialMousePosition)
    {
        Vector3 currentPosition = initialPosition;

        // wait for delta
        while ((initialPosition - currentPosition).sqrMagnitude < 0.1)
        {
            currentPosition = GetPositionFromMouse();
            if (!Input.GetMouseButton(0))
                yield break;

            yield return null;
        }

        shotPreviewRenderer.Show();

        // start
        Rigidbody newAsteroid = GameObject.Instantiate(prefabAsteroid);
        
        Collider collider = newAsteroid.GetComponent<Collider>();
        collider.enabled = false;
        newAsteroid.transform.position = initialPosition;
        newAsteroid.isKinematic = true;
        newAsteroid.gameObject.SetActive(true);
        arrow.enabled = true;
        arrow.rectTransform.anchoredPosition = initialMousePosition;
        float power = minPower;
        Vector3 delta = Vector3.zero;

        // while dragging
        while (Input.GetMouseButton(0))
        {
            Vector3 currentMousePosition = Input.mousePosition;
            currentPosition = GetPositionFromMouse();
            delta = initialPosition - currentPosition;
            power = Mathf.Clamp(delta.magnitude, minPower, maxPower);

            arrow.transform.eulerAngles = Mathf.Atan2(delta.y, delta.x) * Mathf.Rad2Deg * Vector3.forward;
            arrow.rectTransform.sizeDelta = new Vector2((initialMousePosition - currentMousePosition).magnitude - 25, 25);

            arrow.color = powerColorGradient.Evaluate(Mathf.InverseLerp(minPower, maxPower, power));
            shotPreviewRenderer.CalculatePreview(initialPosition,delta, newAsteroid.mass);
            yield return null;
        }
        // release

        newAsteroid.velocity = delta;
        newAsteroid.isKinematic = false;
        newAsteroid.GetComponent<GravitationalBody>().enabled = true;

        shotPreviewRenderer.Hide();
        collider.enabled = true;
        arrow.enabled = false;
    }


    Vector3 GetPositionFromMouse()
    {
        var pos = mainCamera.ScreenToWorldPoint(Input.mousePosition);
        pos.z = 0;
        return pos;
    }


    public void OnPointerDown(PointerEventData eventData)
    {
        StartCoroutine(LaunchMeteorCoroutine(GetPositionFromMouse(), Input.mousePosition));
    }
}
