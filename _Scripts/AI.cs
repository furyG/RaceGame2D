using System.Collections.Generic;
using UnityEngine;

public enum BOT_TYPE
{
    HARD,
    SEMI,
    EASY
}

public class AI : MonoBehaviour
{
    [Header("Set In Inspector")]
    public BOT_TYPE type;

    [Range(0f,1f)]
    public float rotationDuration;

    public Path path;
    public Color[] colors;

    [Header("Set Dynamically")]
    public Vector3 difference;
    public float speedMultiplye, speedDifference;
    public float speed;
    public float currentLap;
    public float rotStartTime;
    public List<Vector3> nodes;
    public int currentNode = 0;

    protected SpriteRenderer sprtRend;
    protected SpriteRenderer heroSPRT;
    protected Rigidbody rb;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        sprtRend = GetComponent<SpriteRenderer>();
        heroSPRT = Hero.H.GetComponent<SpriteRenderer>();
        switch (type)
        {
            case BOT_TYPE.HARD:
                speedMultiplye = 1;
                sprtRend.color = colors[0];
                break;
            case BOT_TYPE.SEMI:
                speedMultiplye = 0;
                sprtRend.color = colors[1];
                break;
            case BOT_TYPE.EASY:
                speedMultiplye = -1;
                sprtRend.color = colors[2];
                break;
        }
    }

    private void Start()
    {
        speedDifference = menuButtonsManager.M.botDificult;
        difference += new Vector3(Random.insideUnitCircle.x, Random.insideUnitCircle.y, 0);
        speed = speedMultiplye + Hero.H.speedMultiplie + 2.5f + speedDifference;
        sprtRend.sprite = heroSPRT.sprite;
        Vector3[] pathVecs = new Vector3[path.transform.childCount];
        for(int i =0; i < path.transform.childCount; i++)
        {
            pathVecs[i] = path.transform.GetChild(i).transform.position;
        }
        nodes = new List<Vector3>();

        for (int i = 0; i < pathVecs.Length; i++)
        {
            if (pathVecs[i] != path.transform.position)
            {
                nodes.Add(pathVecs[i] + difference);
            }
        }
    }
    private void Update()
    {
        if(currentLap == menuButtonsManager.M.amountOfLaps+1)
        {
            menuButtonsManager.M.ShowMessage(1);
        }
        if (!menuButtonsManager.M.timerBegin)
        {
            Moving();
            NodeCheck();
        }
    }
    private void FixedUpdate()
    {
        if (rb.velocity != Vector3.zero) rb.velocity = Vector3.zero;
    }
    void NodeCheck()
    {
        if (Vector3.Distance(transform.position, nodes[currentNode]) < 3f)
        {
            rotStartTime = Time.time;
        }
        if (Vector3.Distance(transform.position, nodes[currentNode]) < 0.05f)
        {
            if (currentNode == nodes.Count - 1)
            {
                currentNode = 0;
            }
            else
            {
                currentNode++;
            }
        }
    }
    void Moving()
    {
        transform.position = Vector3.MoveTowards(transform.position, nodes[currentNode],
    speed * Time.deltaTime);
        Vector3 offset = nodes[currentNode] - transform.position;
        transform.rotation = Quaternion.Lerp(transform.rotation,
            Quaternion.LookRotation(Vector3.forward, offset), (Time.time - rotStartTime) / rotationDuration);
    }
    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "checkpoint")
        {
            checkpointScript CS = other.GetComponent<checkpointScript>();
            if (CS.startThis)
            {
                currentLap++;
            }
        }
    }
}
