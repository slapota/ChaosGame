using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Artist : MonoBehaviour
{
    public Transform particle, pivot, canvas;
    public List<Transform> pivots = new List<Transform>();
    //public float speed;
    Vector2 lastPoint;
    public GameObject startButton, stopButton;
    Transform lastPivot;
    bool run;
    int iter;
    public int maxIter;

    private void Start()
    {
        run = false;
        StartCoroutine(SetPivots());
    }
    private void Update()
    {
        if (!run || iter > maxIter*pivots.Count+1)
        {
            return;
        }
        for (int i = 0; i < 40; i++)
        {
            Transform newPoint = Instantiate(particle, (lastPoint + (Vector2)pivots[Random.Range(0, pivots.Count)].position) / Mathf.PI, Quaternion.identity, canvas);
            lastPoint = newPoint.position;
            iter++;
        }
    }
    IEnumerator SetPivots()
    {
        yield return new WaitUntil(() => Input.GetMouseButtonDown(1));
        yield return new WaitForSecondsRealtime(0.1f);
        Transform newPivot = Instantiate(pivot, Input.mousePosition, Quaternion.identity, canvas);
        pivots.Add(newPivot);
        StartCoroutine(SetPivots());
    }
    /*IEnumerator Tick()
    {
        yield return new WaitForSeconds(1/speed);
        Transform newPoint = Instantiate(particle, (lastPoint + PickPivot())/2, Quaternion.identity, canvas);
        lastPoint = newPoint.position;
        StartCoroutine(Tick());
    }*/
    Vector2 PickPivot()
    {
        Transform tempPivot = lastPivot;
        while (tempPivot == lastPivot)
        {
            tempPivot = pivots[Random.Range(0, pivots.Count)];
        }
        lastPivot = tempPivot;
        return (Vector2)tempPivot.position;
    }
    public void StartSim()
    {
        iter = 0;
        lastPoint = pivots[Random.Range(0, pivots.Count)].position;
        StopAllCoroutines();
        run = true;
        stopButton.SetActive(true);
        startButton.SetActive(false);
    }
    public void Stop()
    {
        run = false;
        foreach (GameObject points in GameObject.FindGameObjectsWithTag("particle"))
        {
            Destroy(points);
        }
        foreach (Transform basePoint in pivots)
        {
            Destroy(basePoint.gameObject);
        }
        pivots = new List<Transform>();
        StartCoroutine(SetPivots());
        startButton.SetActive(true);
        stopButton.SetActive(false);
    }
}
