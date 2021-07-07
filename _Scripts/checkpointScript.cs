using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class checkpointScript : MonoBehaviour
{
    [Header("Set in Inspector")]
    public int number;
    public static checkpointScript start;
    public bool startThis;
    public Sprite lightedCheck;
    public Sprite offCheck;

    [Header("Set Dynamically")]
    public int nextNum;
    public checkpointScript nextCheck;
    public List<GameObject> checks;
    static public GameObject checkParent;

    private void Awake()
    {
        //checks = new List<GameObject>();
        if (startThis)
        {
            start = this;
        }
        Turn(false);
    }
    public void Turn(bool enabled)
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            SpriteRenderer childSprR = transform.GetChild(i).GetComponent<SpriteRenderer>();
            if (!enabled) childSprR.sprite = offCheck;
            else childSprR.sprite = lightedCheck;
        }
    }

    private void Start()
    {
        checkParent = transform.parent.gameObject;
        for (int i = 0; i < checkParent.transform.childCount; i++)
        {
            checks.Add(checkParent.transform.GetChild(i).gameObject);
        }
        nextNum = number+1;
        foreach (GameObject ch in checks)
        {
            checkpointScript getScrpt = ch.GetComponent<checkpointScript>();
            if (getScrpt.number == nextNum && nextCheck == null) nextCheck = getScrpt;
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Hero")
        {
            Turn(true);
        }
    }
    public void ListClear()
    {
        checks.Clear();
    }
}
