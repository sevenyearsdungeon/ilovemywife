using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class FallMotion : MonoBehaviour
{
    public  Vector3 direction;
    [SerializeField] private float timeOffset;
    [SerializeField] private float rotationLimit;
    [SerializeField] private float rotationPeriod;
    [SerializeField] private Gradient heartColorGradient;
    [SerializeField] private ParticleSystem particleSystem;
    [SerializeField] private Camera cam;
    private Image heartImage;

    // Start is called before the first frame update
    void Awake()
    {
        direction = 10 * new Vector2(10 * (Random.value - 0.5f), -15 + 2 * Random.value);
        rotationLimit = Random.value * 20 + 40;
        timeOffset = Random.value * 10;
        rotationPeriod = Random.value * 1 + 2;
         heartImage = GetComponentInChildren<Image>();
        heartImage.color = heartColorGradient.Evaluate(Random.value);
        transform.localScale = Vector3.one * Random.Range(1.9f, 3f);
    }

    // Update is called once per frame
    void Update()
    {
        transform.rotation = Quaternion.Euler(0, 0,
            rotationLimit * Mathf.Sin(2 * Mathf.PI * Time.time / rotationPeriod + timeOffset));
        transform.position += direction *
                              (Time.deltaTime * Mathf.Sin(2*Mathf.PI * 2 * (Time.time + timeOffset+rotationPeriod/2) / rotationPeriod) +
                               2 * Time.deltaTime / rotationPeriod);
    }

    public void OnClicked()
    {
        particleSystem.transform.SetParent(null);
        GameObject o;
        (o = particleSystem.gameObject).SetActive(true);
        o.transform.position = cam.ScreenToWorldPoint(Input.mousePosition+Vector3.forward*10);
        Destroy(o, particleSystem.main.duration);
        Destroy(this.gameObject);
    }
}