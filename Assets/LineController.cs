using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class LineController : MonoBehaviour
{
    [SerializeField]
    private float startX = -5;

    [SerializeField]
    private float endX = 5;

    Vector3[] nominalPositions;
    Vector3[] currentPositions;

    LineRenderer myLine;

    private void Awake()
    {
        myLine = GetComponent<LineRenderer>();
    }

    public void SetInitialState(float yPosition, int vertexCount)
    {
        var lr = GetComponent<LineRenderer>();
        lr.SetVertexCount(vertexCount);
        nominalPositions = Enumerable.Range(0, vertexCount).Select(i => new Vector3(
                       Mathf.Lerp(startX, endX, (float)(i) / (vertexCount - 1))
                       , yPosition, 0)).ToArray();
        currentPositions = Enumerable.Range(0, vertexCount).Select(i => new Vector3(
                       Mathf.Lerp(startX, endX, (float)(i) / (vertexCount - 1))
                       , yPosition, 0)).ToArray();
        lr.SetPositions(nominalPositions);
    }

    internal void SetPositions(float time, float gain, float spring)
    {
        var pos = Input.mousePosition;
        for (int i = 0; i < currentPositions.Length; i++)
        {
            var x = currentPositions[i].x;
            var y = currentPositions[i].y;
            float v = Mathf.PerlinNoise(x + time, y + time);
            float dx = Mathf.PerlinNoise(x + time + 0.01f, y + time) - v;
            float dy = Mathf.PerlinNoise(x + time, y + time + 0.01f) - v;
            float v2 = Mathf.PerlinNoise(x - time, y - time);
            float dx2 = Mathf.PerlinNoise(x - time + 0.01f, y - time) - v;
            float dy2 = Mathf.PerlinNoise(x - time, y + time + 0.01f) - v;
            var springX = nominalPositions[i].x - x;
            var springY = nominalPositions[i].y - y;

            currentPositions[i] = new Vector3(x + (dx+dx2) * gain+springX*spring, y + (dy+dy2) * gain+springY*spring, 0);

        }
        myLine.SetPositions(currentPositions);
    }
}
