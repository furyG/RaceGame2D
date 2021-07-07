using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class lineBehindCar : MonoBehaviour
{
    public List<Vector3> vecs;
    public Vector3 pos;
    public bool lineCreated;
    protected LineRenderer lr;

    private void Awake()
    {
        lr = GetComponent<LineRenderer>();
    }
    void Update()
    {
        pos = transform.position;
        float xAxis = Input.GetAxis("Horizontal");
        if(Mathf.Abs(xAxis) >0.3f)
        {
            if (!lineCreated) CreateLine();
            Vector3 tempVector = pos;
            if(Vector3.Distance(tempVector, vecs[vecs.Count -1]) > 0.7f)
            {
                AddLine(tempVector);
            }
        }
        else
        {
            DestroyLine();
        }
    }
    void CreateLine()
    {
        lineCreated = true;
        vecs.Clear();
        vecs.Add(pos);
        lr.SetPosition(0, vecs[0]);
        lr.SetPosition(1, vecs[0]);
    }
    void AddLine(Vector3 newVector)
    {
        vecs.Add(newVector);
        lr.positionCount++;
        lr.SetPosition(lr.positionCount - 1, newVector);
    }
    void DestroyLine()
    {
        lineCreated = false;
        lr.positionCount = 2;
        lr.SetPosition(0, pos);
        lr.SetPosition(1, pos);
    }
}
