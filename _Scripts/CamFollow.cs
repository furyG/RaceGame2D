using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CamFollow : MonoBehaviour
{
    [Header("Set in Inspector")]
    public Transform targetTransform = null;
    public Transform thisTransform = null;
    public float distanceFromTarget;
    public float camHeight;
    public float rotationDamp;
    public float posDamp;

    [Header("Set Dynamically")]
    public Camera cam;
    public float originCamSize;

    private void Awake()
    {
        cam = GetComponent<Camera>();
        thisTransform = GetComponent<Transform>();
    }

    private void Start()
    {
        originCamSize = cam.orthographicSize;
    }
    public void SetTarget(GameObject target)
    {
        targetTransform = target.transform;
    }
    private void Update()
    {
        if (targetTransform == null) return;

            Vector3 velocity = Vector3.zero;
            Vector3 Dest = thisTransform.position = Vector3.SmoothDamp(thisTransform.position,
                targetTransform.position, ref velocity, posDamp * Time.deltaTime);
            thisTransform.position = Dest - thisTransform.forward; //*DistanceFromTarget; - слишом далеко     
            thisTransform.position = new Vector3(thisTransform.position.x, thisTransform.position.y, thisTransform.position.z); 
        //float VertAxis = Input.GetAxis("Vertical");
        //cam.orthographicSize = Mathf.Abs(VertAxis) * 1 + 5f;
    }
}
