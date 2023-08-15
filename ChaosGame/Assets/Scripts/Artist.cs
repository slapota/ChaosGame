using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Artist : MonoBehaviour
{
    public Transform particle, pivot, canvas;
    public List<Transform> pivots = new List<Transform>();
    public float speed;
    Vector2 lastPoint;

    private void Start()
    {
        StartCoroutine(SetPivots());
    }
    IEnumerator SetPivots()
    {
        yield return new WaitUntil(() => Input.GetMouseButtonDown(1));
        yield return new WaitForSecondsRealtime(0.1f);
        Transform newPivot = Instantiate(pivot, Input.mousePosition, Quaternion.identity, canvas);
        pivots.Add(newPivot);
        StartCoroutine(SetPivots());
    }
    IEnumerator Tick()
    {
        yield return new WaitForSeconds(1/speed);
        Transform newPoint = Instantiate(particle, (lastPoint + (Vector2)pivots[Random.Range(0, pivots.Count)].position)/2, Quaternion.identity, canvas);
        lastPoint = newPoint.position;
        StartCoroutine(Tick());
    }
    public void StartSim(GameObject button)
    {
        lastPoint = pivots[Random.Range(0, pivots.Count)].position;
        button.SetActive(false);
        StopAllCoroutines();
        StartCoroutine(Tick());
    }
}
