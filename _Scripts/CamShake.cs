using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CamShake : MonoBehaviour
{
    [Header("Set In Inspector")]
    [Range(0,5)]
    public float shakeAmount = 3f;
    [Range(0,5)]
    public float shakeSpeed = 2f;

    [Header("Set Dynamically")]
    public Vector3 origPosition;

    protected Transform thisTransform = null;
    protected Transform targetTransform = null;

    private void Awake()
    {
        thisTransform = GetComponent<Transform>(); 
    }
    private void Start()
    {
        origPosition = thisTransform.position;
    }
    public void SetTarget(GameObject target)
    {
        targetTransform = target.transform;
    }

    private void Update()
    {
        origPosition = thisTransform.position;
        if (targetTransform == null) return;
            if (Hero.H.slowed)
            {
                Vector3 RandomPoint = origPosition + Random.insideUnitSphere * shakeAmount;
                thisTransform.localPosition = Vector3.Lerp(
                    thisTransform.localPosition, RandomPoint, Time.deltaTime * shakeSpeed);
            }
            else
            {
                thisTransform.position = origPosition;
            }
    }
}
