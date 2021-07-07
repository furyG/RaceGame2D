using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Hero : MonoBehaviour
{
    static public Hero H;
    [Header("Set In Inspector")]
    public float particlesCount;
    [Range(0f,10f)]
    public float speedMultiplie;
    [Range(0f,3f)]
    public float rotSpeed;

    public Text laps;
    public GameObject particlePrefab;

    [Header("Set Dynamically")]
    public checkpointScript next = null;
    public int currentEnablingK;
    public float dirtDebaff;
    public float currentLap;
    public float numOfLaps;
    public float speed;
    public Vector3 pos;
    public Vector3 rot;
    public bool slowed;
    public bool racing;

    protected Rigidbody rb;
    protected List<GameObject> particles;

    private void Awake()
    {
        H = this;
        rb = GetComponent<Rigidbody>();
        laps = GameObject.Find("laps").GetComponent<Text>();
        laps.enabled = true;
        currentLap = 0;
        particles = new List<GameObject>();
    }
    private void Start()
    {
        speedMultiplie = menuButtonsManager.M.carMultiplieSpeed;
        rotSpeed = menuButtonsManager.M.carRotSpeed;
        numOfLaps = menuButtonsManager.M.amountOfLaps;
        dirtDebaff = menuButtonsManager.M.dirtDebaff;
        currentEnablingK = EnableScore.score;
    }
    private void Update()
    {
        if (!menuButtonsManager.M.timerBegin)
        {
            Moving();

            laps.text = currentLap + "/" + numOfLaps;
        }
        else return;  
    }
    private void FixedUpdate()
    {
        if (rb.velocity != Vector3.zero) rb.velocity = Vector3.zero;
    }
    void Moving()
    {
        pos = transform.position;
        float xAxis = Input.GetAxis("Horizontal");
        float yAxis = Input.GetAxis("Vertical");
        speed = yAxis * speedMultiplie * 1.8f * Time.deltaTime;
        rot.z -= xAxis * rotSpeed * 180f * Time.deltaTime;
        transform.Translate(0, speed, 0);
        transform.rotation = Quaternion.Euler(rot);
    }
    private void OnTriggerEnter(Collider coll)
    {
        if(coll.gameObject.tag == "Grass" && !slowed)
        {
            Slowing();
        }
        if(coll.gameObject.tag == "checkpoint")
        {
            checkpointScript CS = coll.GetComponent<checkpointScript>();
            if (CS != next && CS != CS.startThis)
            {
                //Debug.Log("proigral");
                menuButtonsManager.M.ShowMessage(0);
            }
            if (CS == CS.startThis)
            {
                currentLap++;
                foreach (GameObject csgo in CS.checks)
                {
                    checkpointScript scrp = csgo.GetComponent<checkpointScript>();
                    scrp.Turn(false);
                    Debug.Log("viklu4il");
                }
                if (currentLap == numOfLaps + 1)
                {
                    Finish();
                    return;
                }
            }
        }
    }
    private void OnTriggerExit(Collider coll)
    {
        if (coll.gameObject.tag == "Grass" && slowed)
        {
            Accelerating();
        }
        if (coll.gameObject.tag == "checkpoint")
        {
            checkpointScript cs = coll.gameObject.GetComponent<checkpointScript>();
            next = cs.nextCheck;
            if (cs.startThis)
            {
                racing = true;
                //cs.gameObject.SetActive(false);
            }
        }
    }
    private void OnCollisionEnter(Collision collision)
    {
        //Debug.Log("Collided with: " + collision.gameObject.name);
        Slowing();
        Vector3 collVec = collision.contacts[0].point;
        Particles(collVec);
    }
    private void OnCollisionExit(Collision collision)
    {
        Accelerating();
    }
    public void Finish()
    {
        currentLap--;
        racing = false;
        Debug.Log("Race Done");
        menuButtonsManager.M.ShowMessage(2);
        EnableScore.score++;
        menuButtonsManager.M.CarEnabling(EnableScore.score);
    }
    void Slowing()
    {
        slowed = true;
        speedMultiplie /= dirtDebaff;
        rotSpeed /= dirtDebaff;
    }
    void Accelerating()
    {
        speedMultiplie *= dirtDebaff;
        rotSpeed *= dirtDebaff;
        slowed = false;
    }
    void Particles(Vector3 spawnVector)
    {
        for(int i =0; i< particlesCount; i++)
        {
            GameObject pSpawn = Instantiate<GameObject>(particlePrefab);
            Rigidbody pRB = pSpawn.GetComponent<Rigidbody>();
            particles.Add(pSpawn);
            Vector2 spawnPos = Random.insideUnitCircle*2f;
            pSpawn.transform.position = spawnVector;
            pRB.velocity += new Vector3(spawnPos.x, spawnPos.y, 0) * 3f;
            Invoke("destroy", 1f);
        }
    }
    void destroy()
    {
        foreach(GameObject p in particles)
        {
            Destroy(p);
        }
    }
}
