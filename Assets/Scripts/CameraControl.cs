using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CameraControl : MonoBehaviour
{
    bool canFollow;
    GameObject objectToFollow;
    float speed = 0.1f;
    float xR = 1215.0f;
    float zR = 570.0f;
    string Name; // переменная для сохранения имени обьекта слежения 
    float prevHeight; //предыдущая позиция камеры

    public Text currentHP;

    void Start()
    {
        PlayerPrefs.SetInt("NeedDoDetailingDead", 0);
        PlayerPrefs.SetInt("NeedDoSimplificationDead", 0);
        PlayerPrefs.SetInt("NeedDoDetailingBac", 0);
        PlayerPrefs.SetInt("NeedDoSimplificationBac", 0);
        PlayerPrefs.SetInt("NeedDoDetailingCiano", 0);
        PlayerPrefs.SetInt("NeedDoSimplificationCiano", 0);
        PlayerPrefs.SetInt("NeedDoDetailingVir", 0);
        PlayerPrefs.SetInt("NeedDoSimplificationVir", 0);
        PlayerPrefs.SetInt("NeedDoDetailingInf", 0);
        PlayerPrefs.SetInt("NeedDoSimplificationInf", 0);
        PlayerPrefs.SetInt("NeedDoDetailingEu", 0);
        PlayerPrefs.SetInt("NeedDoSimplificationEu", 0);
        PlayerPrefs.SetInt("CountBacteria", 0);
        PlayerPrefs.SetInt("CountCianobacteria", 0);
        PlayerPrefs.SetInt("CountVirus", 0);
        PlayerPrefs.SetInt("CountEukaryote", 0);

        //PlayerPrefs.SetInt("CountSpawn", 100);
        //PlayerPrefs.SetInt("SpeedBacteria", 10);
        //PlayerPrefs.SetInt("SpeedCiano", 10);
        //PlayerPrefs.SetInt("SpeedVirus", 10);
        //PlayerPrefs.SetInt("SpeedEu", 10);

        canFollow = false;
        prevHeight = transform.position.y;
    }

    void FixedUpdate()
    {
        float cameraHeight = transform.position.y;
        float speedMoveCoef;
        Vector3 moving = new Vector3(0, 0, 0);

        //Сопутсвующие переменные
        speedMoveCoef = 1 + (cameraHeight / 100);
        var frustumHeight = 2.0f * transform.position.y * Mathf.Tan(Camera.main.fieldOfView * 0.5f * Mathf.Deg2Rad);
        var frustumWidth = 2.0f * transform.position.y * Mathf.Tan(Camera.main.fieldOfView * 0.25f * Mathf.Deg2Rad);


        if (!canFollow)
        {
            if (Input.GetKey(KeyCode.A) && transform.position.x > -xR + frustumHeight)
                transform.position = new Vector3(transform.position.x - speedMoveCoef, transform.position.y, transform.position.z);
            if (Input.GetKey(KeyCode.D) && transform.position.x < xR - frustumHeight)
                transform.position = new Vector3(transform.position.x + speedMoveCoef, transform.position.y, transform.position.z);
            if (Input.GetKey(KeyCode.W) && transform.position.z < zR - frustumWidth)
                transform.position = new Vector3(transform.position.x, transform.position.y, transform.position.z + speedMoveCoef / (xR / zR));
            if (Input.GetKey(KeyCode.S) && transform.position.z > -zR + frustumWidth)
                transform.position = new Vector3(transform.position.x, transform.position.y, transform.position.z - speedMoveCoef / (xR / zR));
        }


        //Стрелка вверх   Стрелка вниз - управление камеры вертикально
        if (Input.GetKey(KeyCode.UpArrow) && cameraHeight > 5.0f)
        {
            prevHeight = transform.position.y;
            transform.position = new Vector3(transform.position.x, transform.position.y - 5.0f, transform.position.z);

        }

        if (Input.GetKey(KeyCode.DownArrow) && cameraHeight < 1000.0f)
        {
            prevHeight = transform.position.y;

            //движение камеры в с орону центра при отдалении
            if (((transform.position.x > 10 || transform.position.x < -10) || (transform.position.z > 10 || transform.position.z < -10)) && !canFollow)
            {
                if ((transform.position.x > 10 || transform.position.x < -10) && (transform.position.z < 10 || transform.position.z > -10))
                    moving = new Vector3(((transform.position.x < 0 ? 1 : -1) * frustumHeight), 0, 0);
                else if ((transform.position.x < 10 || transform.position.x > -10) && (transform.position.z > 10 || transform.position.z < -10))
                    moving = new Vector3(0, 0, ((transform.position.z < 0 ? 1 : -1) * frustumWidth));
                else
                    moving = new Vector3(((transform.position.x < 0 ? 1 : -1) * frustumHeight), 0, ((transform.position.z < 0 ? 1 : -1) * frustumWidth));

                transform.position += moving.normalized * 400.0f * Time.deltaTime;
            }

            //движение камеры вверх
            transform.position = new Vector3(transform.position.x, transform.position.y + 5.0f, transform.position.z);
        }

        // Слежение камеры за обьектом
        if (Time.timeScale != 0)
        {
            Vector3 pos = new Vector3(objectToFollow.transform.position.x, transform.position.y, objectToFollow.transform.position.z);
            transform.position = Vector3.Lerp(transform.position, pos, speed);
            currentHP.text = "Current HP: " + objectToFollow.GetComponent<Eukaryote>().HP.ToString("F1");
        }
       
    }

    void Update()
    {
        //if (Name != null)
        //    objectToFollow = GameObject.Find(Name);

        // обработка нажатия клавиши для слежения
        if (Time.timeScale != 0)
        {
          
           objectToFollow = GameObject.Find("player");
           canFollow = true;
        }
    }
}