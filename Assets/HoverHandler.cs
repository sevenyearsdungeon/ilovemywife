using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Collections.LowLevel.Unsafe;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class HoverHandler : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerDownHandler
{
    [SerializeField]
    RectTransform cursorTransform;
    [SerializeField]
    TMP_Text label;
    bool hovering = false;
    bool clicked = false;
    [SerializeField]
    Vector3 offset;

    public void OnPointerDown(PointerEventData eventData)
    {
        clicked = true;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        hovering = true;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        hovering = false;
    }

    void Update()
    {
        if (Input.GetMouseButtonUp(0))
            clicked = false;
        if (hovering || clicked)
        {
            if (label.alpha < 1)
            {
                label.alpha = Mathf.Lerp(label.alpha, 1, 0.08f);
                if (label.alpha > 0.95f)
                    label.alpha = 1;
            }
        }
        else
        {
            if (label.alpha > 0)
            {
                label.alpha = Mathf.Lerp(label.alpha, 0, 0.08f);
                if (label.alpha < 0.01f)
                    label.alpha = 0;
            }
        }
        
        if (label.alpha>0)
        {
            label.transform.position = cursorTransform.position+offset;
        }
    }

}
