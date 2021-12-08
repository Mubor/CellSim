using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class UI_Control : World
{
    public static bool GameIsPaused = true;

    public Text colony;
    public Text CameraHeight;
    public Text StartButton;
    public Text CellSim;

    public Image Day;

    public InputField CountSpawn;
    public InputField speedBacteria;
    public InputField speedCiano;
    public InputField speedVirus;
    public InputField speedEu;

    public GameObject StatusBar;
    public GameObject Menu;
    public GameObject Settings;

    protected Sprite[] sprites;
    public Material[] skyboxes;

    public Light directionLight;

    bool win = false;
    bool lose = false;

    void Start()
    {
        sprites = Resources.LoadAll<Sprite>("Icons");
        Time.timeScale = 0f;
    }

    void Update()
    {
        if (Time.timeScale != 0)
            colony.text = "Current Colony: " + colonysize.ToString();

        CameraHeight.text = "Camera Height: " + Camera.main.transform.position.y;

        if(PlayerDead())
        {
            Lose();
        }
    
        if(colonysize == 2)
        {
            Win();
        }



        if (timesOfDay)
        {
            Day.sprite = sprites[2];
            directionLight.intensity = 2.0f;
            Camera.main.GetComponent<Skybox>().material = skyboxes[0];
        }
        else
        {
            Day.sprite = sprites[1];
            directionLight.intensity = 0.5f;
            Camera.main.GetComponent<Skybox>().material = skyboxes[1];
        }

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (GameIsPaused && !win && !lose)
                Resume();
            else
                Pause();
        }
    }

    protected void UI_Params(bool menu, bool bar, bool settings)
    {
        Menu.SetActive(menu);
        StatusBar.SetActive(bar);
        Settings.SetActive(settings);
    }

    public void Resume()
    {
        UI_Params(false, true, false);

        Time.timeScale = 1f;
        GameIsPaused = false;

        if (StartButton.GetComponent<Text>().text == "Start")
            StartButton.GetComponent<Text>().text = "Continue";
    }

    public void Pause()
    {
        UI_Params(true, false, false);

        Time.timeScale = 0f;
        GameIsPaused = true;
    }

    public void Setting()
    {
        UI_Params(false, false, true);
    }

    public void Apply()
    {
        UI_Params(false, false, true);

        PlayerPrefs.SetInt("CountSpawn", int.Parse(CountSpawn.textComponent.text));
        PlayerPrefs.SetInt("SpeedBacteria", int.Parse(speedBacteria.textComponent.text));
        PlayerPrefs.SetInt("SpeedCiano", int.Parse(speedCiano.textComponent.text));
        PlayerPrefs.SetInt("SpeedVirus", int.Parse(speedVirus.textComponent.text));
        PlayerPrefs.SetInt("SpeedEu", int.Parse(speedEu.textComponent.text));
    }

    public void ResetWorld(string str)
    {
        SceneManager.LoadScene(str);
    }

    public void Exit()
    {
        Application.Quit();
    }

    public void Lose()
    {
        CellSim.text = "You die!";
        CellSim.color = Color.red;

        Time.timeScale = 0f;
        GameIsPaused = true;
    }

    public void Win()
    {
        print("Im her");
        CellSim.text = "You win!";
        CellSim.color = Color.yellow;

        Time.timeScale = 0f;
        GameIsPaused = true;
    }
}
