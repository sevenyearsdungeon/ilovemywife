using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LineBubber : MonoBehaviour
{
    [SerializeField]
    private GameObject linePrefab;
    List<LineController> lineControllers = new List<LineController>();
    public int lineCount = 100;
    public int vertexCount = 100;
    public float minY = -3;
    public float maxY = 3;
    public float gain = 1;

    public float speed = 0.5f;

    public float spring = 0.01f;
    public float lineWidth = 1;

    private void Start()
    {
        CreateLines(lineCount, vertexCount);
    }

    int lastVertexCount = -1, lastLineCount = -1;

    private void Update()
    {
        if (lastLineCount != lineCount || lastVertexCount != vertexCount)
            CreateLines(lineCount, vertexCount);


        foreach (var line in lineControllers)
        {
            line.SetPositions(Time.time * speed, gain, spring);
        }
    }

    void CreateLines(int lineCount, int vertexCount)
    {
        float deltaWidth = lineWidth * (maxY - minY) / lineCount / 2;

        int linesToDelete = lineControllers.Count - lineCount;
        for (int i = 0; i < linesToDelete; i++)
        {
            var go = lineControllers[0].gameObject;
            Destroy(go);
            lineControllers.RemoveAt(0);
        }

        for (int i = lineControllers.Count; i < lineCount; i++)
        {
            var newLine = GameObject.Instantiate(linePrefab, transform);
            newLine.transform.localPosition = Vector3.zero;
            LineController lineController = newLine.GetComponent<LineController>();
            lineControllers.Add(lineController);
            newLine.SetActive(true);
        }

        for (int i = 0; i < lineControllers.Count; i++)
        {
            lineControllers[i].SetInitialState(Mathf.Lerp(minY, maxY, (float)(i) / (lineCount - 1)), vertexCount);
        }

        SetLineWidths();


        lastLineCount = lineCount;
        lastVertexCount = vertexCount;
    }

    public void SetLineCount(float v)
    {
        lineCount = (int)v;
    }
    public void SetVertexCount(float v)
    {
        vertexCount = (int)v;
    }
    public void SetLineWidth(float v)
    {
        lineWidth = v;

        SetLineWidths();
    }
    public void SetSpring(float v)
    {
        spring = v;
    }
    public void SetSpeed(float v)
    {
        speed = v;
    }
    public void SetGain(float v)
    {
        gain = v;
    }

    private void SetLineWidths()
    {
        float deltaWidth = lineWidth * (maxY - minY) / lineCount / 2;

        for (int i = 0; i < lineControllers.Count; i++)
        {
            LineRenderer lineRenderer = lineControllers[i].GetComponent<LineRenderer>();
            lineRenderer.startWidth = deltaWidth / 2;
            lineRenderer.endWidth = deltaWidth / 2;
        }
    }
}
