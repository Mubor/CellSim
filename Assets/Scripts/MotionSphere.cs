using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MotionSphere : MonoBehaviour
{
    private int speed;
    public int direction;
    private int directionDistance; 

    float distanceX;
    float distanceZ;

    float rangeX;
    float rangeZ;

    string tag;

    Vector3 moving;
    Vector3 currentPosition;

    bool NewDirection;

    public void ChangeDirection(int direction)
    {
        //this.direction = direction;

        switch (direction)
        {
            case 0:
                this.direction = Random.Range(0, 2) == 0 ? 7 : 6;
                break;
            case 1:
                this.direction = Random.Range(0, 2) == 0 ? 5 : 6;
                break;
            case 2:
                this.direction = Random.Range(0, 2) == 0 ? 4 : 7;
                break;
            case 3:
                this.direction = Random.Range(0, 2) == 0 ? 4 : 5;
                break;
            case 4:
                this.direction = Random.Range(0, 2) == 0 ? 2 : 3;
                break;
            case 5:
                this.direction = Random.Range(0, 2) == 0 ? 3 : 1;
                break;
            case 6:
                this.direction = Random.Range(0, 2) == 0 ? 1 : 0;
                break;
            case 7:
                this.direction = Random.Range(0, 2) == 0 ? 2 : 0;
                break;
            default:
                break;
        }
    }

    void Start()
    {
        rangeX = 1215.0f;
        rangeZ = 570.0f;

        if (NewDirection)
        {
            direction = Random.Range(0, 8);
        }
        distanceX = Random.Range(100.0f, 5000.0f);
        distanceZ = Random.Range(100.0f, 5000.0f);

        currentPosition = transform.position;
    }

    void Update()
    {
        if (Time.timeScale != 0)
        {
            if (moving != null)
            {
                Ray ray = new Ray(transform.position, moving);
                
                if(this.tag == "cianobacteria" || this.tag == "cianobacteriaDetailed")
                    if (Physics.Raycast(ray, out RaycastHit hit, 10f))
                        if(hit.transform.tag != "virus" || hit.transform.tag != "virusDetailed")
                            ChangeDirection(direction);

                if(this.tag == "virus" || this.tag == "virusDetailed")
                {
                    if (Physics.Raycast(ray, out RaycastHit hit, 10f))
                    {
                        switch (hit.transform.tag)
                        {
                            case "bacteriaDead":
                                ChangeDirection(direction);
                                break;

                            case "bacteriaDeadDetailed":
                                ChangeDirection(direction);
                                break;

                            case "cianobacteriaDead":
                                ChangeDirection(direction);
                                break;

                            case "cianobacteriaDeadDetailed":
                                ChangeDirection(direction);
                                break;

                            case "virusDead":
                                ChangeDirection(direction);
                                break;

                            case "virusDeadDetailed":
                                ChangeDirection(direction);
                                break;

                            case "preEukaryoteDead":
                                ChangeDirection(direction);
                                break;

                            case "preEukaryoteDeadDetailed":
                                ChangeDirection(direction);
                                break;

                            case "eukaryoteDead":
                                ChangeDirection(direction);
                                break;

                            case "eukaryoteDeadDetailed":
                                ChangeDirection(direction);
                                break;

                            default:
                                break;
                        }
                    }  
                }

                if (this.tag == "cianobacteriaInfected" || this.tag == "cianobacteriaInfectedDetailed")
                    if (Physics.Raycast(ray, out RaycastHit hit, 15f))
                        ChangeDirection(direction);
            }

            currentPosition = transform.position;

            if (currentPosition.x <= -rangeX)
                direction = 4;
            else if (currentPosition.z <= -rangeZ)
                direction = 5;
            else if (currentPosition.x >= rangeX)
                direction = 6;
            else if (currentPosition.z >= rangeZ)
                direction = 7;

            //if (direction == 0)
            //    moving = new Vector3(-(distanceX), 0, -(distanceZ));
            //else if (direction == 1)
            //    moving = new Vector3(-(distanceX), 0, distanceZ);
            //else if (direction == 2)
            //    moving = new Vector3(distanceX, 0, -(distanceZ));
            //else if (direction == 3)
            //    moving = new Vector3(distanceX, 0, distanceZ);
            //else if (direction == 4)
            //    moving = new Vector3(distanceX, 0, 0);
            //else if (direction == 5)
            //    moving = new Vector3(0, 0, distanceZ);
            //else if (direction == 6)
            //    moving = new Vector3(-(distanceX), 0, 0);
            //else if (direction == 7)
            //    moving = new Vector3(0, 0, -(distanceZ));

            switch (direction)
            {
                case 0:
                    moving = new Vector3(-(distanceX), 0, -(distanceZ));
                    break;
                case 1:
                    moving = new Vector3(-(distanceX), 0, distanceZ);
                    break;
                case 2:
                    moving = new Vector3(distanceX, 0, -(distanceZ));
                    break;
                case 3:
                    moving = new Vector3(distanceX, 0, distanceZ);
                    break;
                case 4:
                    moving = new Vector3(distanceX, 0, 0);
                    break;
                case 5:
                    moving = new Vector3(0, 0, distanceZ);
                    break;
                case 6:
                    moving = new Vector3(-(distanceX), 0, 0);
                    break;
                case 7:
                    moving = new Vector3(0, 0, -(distanceZ));
                    break;
                default:
                    break;
            }

            transform.position += moving.normalized * speed * Time.deltaTime;

            if(Random.Range(0,100) == 0)
                transform.Rotate(0, Random.Range(0, 361), 0);

            if (Random.Range(0, directionDistance) == 0)
            {
                direction = Random.Range(0, 8);
                distanceX = Random.Range(0, 2) == 1 ? distanceX + speed : distanceX - speed;
                distanceZ = Random.Range(0, 2) == 1 ? distanceZ - speed : distanceZ + speed;
                distanceX = Random.Range(100.0f, 5000.0f);
                distanceZ = Random.Range(100.0f, 5000.0f);
            }
        }
    }

    public void Params(int spd, int distance, bool newdirection, int direction)
    {
        directionDistance = distance;
        speed = spd;
        NewDirection = newdirection;
        this.direction = direction;
    }
}