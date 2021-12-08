using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Eukaryote : MonoBehaviour
{
    protected GameObject cell;
    protected GameObject GenerationPlace;

    protected int Speed { get; set; }
    protected int DirectionChange { get; set; }
    protected float LocalScale { get; set; }
    public float HP { get; set; }
    float hpChanger;
    float maxScale;
    public int numPrefab;
    public bool hasDaughter;
    //сохранение направления движения при смене префаба
    bool newDirection;
    int direction;
    int daughterCount;
    // int Numprefab;

    public void ChangeVariablehasDaughter()
    {
        hasDaughter = false;
        daughterCount++;
    }

    public void ChangeScale(float scaleCoef)
    {
        LocalScale += scaleCoef;
        cell.transform.localScale = new Vector3(LocalScale, LocalScale, LocalScale);
    }
    void Start()
    {
        Speed = PlayerPrefs.GetInt("SpeedEu");
        hpChanger = -0.001f;
        maxScale = 13;
        DirectionChange = 2000;
        GenerationPlace = GameObject.Find("GenerationPlace");


        cell.transform.localScale = new Vector3(LocalScale, LocalScale, LocalScale);

        if (this.name != "player")
        {
            cell.AddComponent<Motion>().Params(Speed, DirectionChange, newDirection, direction);
        }
        
        cell.AddComponent<ColliderEvent>().Params(cell, true);


    }
    private void FixedUpdate()
    {

        HP += hpChanger;

    }
    void Update()
    {
        if (Time.timeScale != 0)
        {
            if (HP <= 0)
                DeathOfCell(cell);
            if (cell.transform.localScale.x >= maxScale && !hasDaughter)
            {

                if (Camera.main.transform.position.y < 500f)
                {
                    GenerationPlace.GetComponent<CreateNewObj>().CreateSubsidiaryCell(cell, cell.tag, numPrefab, true, 0f);
                }
                else
                    GenerationPlace.GetComponent<CreateNewObj>().CreateSubsidiaryCell(cell, cell.tag, numPrefab, false, 0f);


                hasDaughter = true;
            }
        }
    }

    public void ScaleChanger(float localScale)
    {
        LocalScale += localScale;
        cell.transform.localScale = new Vector3(LocalScale, LocalScale, LocalScale);
    }
    public void ChangeHP(float hpChanger)
    {
        HP += hpChanger;
    }
    void DeathOfCell(GameObject thisObject)
    {
        if (Camera.main.transform.position.y >= 150.0f)  //Камера выше 350
            GenerationPlace.GetComponent<CreateNewObj>().RecreateObjDead(thisObject, thisObject.name + "_Dead", "eukaryoteDead", 3, false); //bacteriaDead
        else
            GenerationPlace.GetComponent<CreateNewObj>().RecreateObjDead(thisObject, thisObject.name + "_Dead", "eukaryoteDeadDetailed", 3, true);  //bacteriaDetailed   == bacteria + DeadDetailed

    }

    public void SetParams(GameObject obj, string tag, string name, float hp, float scale, bool hasdaughter, bool newdirection, int direction, int numprefab, int daughtercnt)
    {
        //setting params
        cell = obj;
        LocalScale = scale;
        cell.tag = tag;
        cell.name = name;
        HP = hp;
        hasDaughter = hasdaughter;
        newDirection = newdirection;
        this.direction = direction;
        numPrefab = numprefab;
        daughterCount = daughtercnt;
    }
}