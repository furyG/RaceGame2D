using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameController : MonoBehaviour
{
    public static GameController G;
    [Header("Set In Inspector")]
    public GameObject heroPrefab;

    [Header("Set Dynamically")]
    public GameObject levelSpawn;
    public GameObject heroSpawn;
    public CamFollow camF;
    public CamShake camSh;

    private void Awake()
    {
        G = this;
        camF = GetComponent<CamFollow>();
        camSh = GetComponent<CamShake>();
    }
    public void SpawnLevel(GameObject circuit, Sprite heroSprite, Vector3 spawnPos)
    {
        Destroy(heroSpawn); Destroy(levelSpawn);
        heroSpawn = Instantiate<GameObject>(heroPrefab);
        SpriteRenderer heroSpriteRend = heroSpawn.GetComponent<SpriteRenderer>();
        heroSpriteRend.sprite = heroSprite;
        heroSpawn.transform.position = spawnPos;
    
        levelSpawn = Instantiate<GameObject>(circuit);
        levelSpawn.transform.position = Vector3.zero;

        camF.SetTarget(heroSpawn);
        camSh.SetTarget(heroSpawn);
    }
}
