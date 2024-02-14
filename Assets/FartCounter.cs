using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class FartCounter : MonoBehaviour
{
    private TMP_Text text;
    private int counter = 1;
    private void Awake()
    {
        text = GetComponentInChildren<TMP_Text>();
    }

    public void IncrementCounter()
    {
        text.SetText($"Hearts Farted: {counter++}");
    }
}
