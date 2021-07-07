using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class menuButtonsManager : MonoBehaviour
{
    static public menuButtonsManager M;
    [Header("Set In Inspector")]
    public carDefinitions[] cars;
    public circuitDefinitions[] circuits;
    public Button[] carButtons;
    public Text[] messages;
    public Sprite[] timerImages;
    public Image timerImage;
    public GameObject borderPref;
    public GameObject properPrefab;
    public Button startButton;
    public Button continueButton;
    public GameObject Menu;

    [Header("Set Dynamically")]
    public int beginTime;
    public bool timerBegin;
    public float timer = 5;
    public GameObject borderSpawnCar, borderSpawnCircuit;
    public GameObject chosenCircuit;
    public GameObject ProprtiesSpawn;
    public Sprite chosenCar;
    public Vector3 spawnHeroPos;
    public float carMultiplieSpeed, carRotSpeed, amountOfLaps, dirtDebaff, botDificult;
    public bool carChosen, circuitChosen;
    public bool menuActive;

    private void Awake()
    {
        M = this;
        timerImage.enabled = timerBegin;
        foreach(carDefinitions car in cars)
        {
            foreach(Button b in carButtons)
            {
                if(b.name == car.car_name)
                {
                    b.interactable = car.enabled;
                }
            }
        }
        MessagesOff();
    }
    private void Start()
    {
        CarEnabling(EnableScore.score);
    }
    public void SendCar()
    {
        GameObject GetButton = EventSystem.current.currentSelectedGameObject.gameObject;
        ChooseCar(GetButton);
    }
    public void SendCircuit()
    {
        GameObject GetButton = EventSystem.current.currentSelectedGameObject.gameObject;
        ChooseCircuit(GetButton);
    }
    public Sprite ChooseCar(GameObject button)
    {
        Destroy(borderSpawnCar);
        borderSpawnCar = Instantiate<GameObject>(borderPref);
        borderSpawnCar.transform.position = button.transform.position;
        borderSpawnCar.transform.SetParent(button.transform);
        string ButtonName = button.name;
        foreach(carDefinitions c in cars)
        {
            if(c.car_name == ButtonName && c.enabled)
            {
                chosenCar = c.car_Prefab;
                botDificult = c.botDificulty;
                carMultiplieSpeed = c.properties[0];
                carRotSpeed = c.properties[1];
                dirtDebaff = c.properties[2];

                Debug.Log("Properties Shown");
                Destroy(ProprtiesSpawn);
                ProprtiesSpawn = Instantiate<GameObject>(properPrefab);
                ProprtiesSpawn.transform.position = new Vector3(button.transform.position.x + 150, button.transform.position.y - 150);
                ProprtiesSpawn.transform.SetParent(Menu.transform);
                for(int i = 0; i<3; i++)
                {
                    GameObject property = ProprtiesSpawn.transform.GetChild(i).gameObject;
                    float ToggleNum = c.properties[i];
                    for(int j = 0; j<ToggleNum; j++)
                    {
                        Image toggle = property.transform.GetChild(j).GetComponent<Image>();
                        toggle.color = Color.red;
                    }
                }
            }
        }
        Debug.Log("button_name is: " + ButtonName);
        carChosen = true;
        return chosenCar;
    }
    public GameObject ChooseCircuit(GameObject button)
    {
        Destroy(borderSpawnCircuit);
        borderSpawnCircuit = Instantiate<GameObject>(borderPref);
        borderSpawnCircuit.transform.position = button.transform.position;
        borderSpawnCircuit.transform.SetParent(button.transform);
        string ButtonName = button.name;
        foreach (circuitDefinitions c in circuits)
        {
            if (c.circuit_name == ButtonName)
            {
                spawnHeroPos = c.HeroSpawnTransform;
                chosenCircuit = c.circuitGO;
                amountOfLaps = c.amountOfLaps;
            }
        }
        Debug.Log("button_name is: " + ButtonName);
        circuitChosen = true;
        return chosenCircuit;
    }
    private void Update()
    {
        if(carChosen && circuitChosen) startButton.interactable = true;
        if (Input.GetKeyDown(KeyCode.Escape) && !menuActive)
        {
            Menu.SetActive(true);
            MessagesOff();
            Time.timeScale = 0;
            if (Hero.H.gameObject != null) continueButton.interactable = Hero.H.racing;
            menuActive = !menuActive;
        }
        if (timerBegin)
        {
            timerImage.enabled = timerBegin;
            timer = beginTime - Time.time;
            timerImage.sprite = timerImages[(int)timer];
            if (timer <= 1) timerBegin = false; timerImage.enabled = timerBegin;
        }
    }
    public void StartButton()
    {
        Menu.SetActive(false);
        MessagesOff();
        Time.timeScale = 1;
        menuActive = !menuActive;
        StartTimer();
        GameController.G.SpawnLevel(chosenCircuit,chosenCar,spawnHeroPos);    
    }
    void StartTimer()
    {
        timerBegin = true;
        beginTime = (int)Time.time + 5;
    }
    public void ContinueButton()
    {
        Menu.SetActive(false);
        Time.timeScale = 1;
        menuActive = !menuActive;
    }
    public void Exit()
    {
        Application.Quit();
    }
    /// <summary>
    /// получает интеджер и по нему смотрит причину проигрыша или выигрыша и включает нужное 
    /// сообщение
    /// </summary>
    /// <param name="reason"> 0 - пропустил чекпоинт, 1 - проиграл боту, 2 - победил</param>
    public void ShowMessage(int reason)
    {
        switch (reason)
        {
            case (0):
                messages[0].enabled = true;
                break;
            case (1):
                messages[1].enabled = true;
                break;
            case (2):
                messages[2].enabled = true;
                break;
        }
        Hero.H.racing = false;
        Time.timeScale = 0;
    }
    public void MessagesOff()
    {
        foreach (Text t in messages) t.enabled = false;
    }
    /// <summary>
    /// получает номер включения машины и включает ее кнопку, чтоб можно было выбрать ее
    /// </summary>
    /// <param name="numberOfEnabling"> номер машины для включения в массиве cars</param>
    public void CarEnabling(int numberOfEnabling)
    {
        foreach(carDefinitions c in cars)
        {
            if(c.numberOfEnable <= numberOfEnabling)
            {
                foreach(Button b in carButtons)
                {
                    if(b.name == c.car_name)
                    {
                        b.interactable = true;
                        c.enabled = true;
                    }
                }
            }
        }
    }
}
