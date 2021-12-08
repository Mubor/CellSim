using UnityEngine;

public class Motion : MonoBehaviour
{
    private int speed;
    public int direction;
    private int directionDistance;
    int previousDirection;

    float distanceX;
    float distanceZ;

    float rangeX;
    float rangeZ;
    bool WallTrigger;
    bool NewDirection;
    bool rayHit = false;

    Vector3 moving;
    Vector3 currentPosition;
    Vector3 previousPosition;

    Quaternion startRotation;

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
        WallTrigger = false;
        rangeX = 1215.0f;
        rangeZ = 570.0f;

        if(NewDirection)
            direction = Random.Range(0, 8);
        distanceX = Random.Range(100.0f, 5000.0f);
        distanceZ = Random.Range(100.0f, 5000.0f);
        previousDirection = -1;

        currentPosition = transform.position;
        previousPosition = currentPosition;
    }

    void Update()
    {
        if (Time.timeScale != 0)
        {
            if (moving != null)
            {
                Ray ray = new Ray(transform.position, moving);
                if (this.tag == "bacteria" || this.tag == "bacteriaDetailed")
                {
                    if (Physics.Raycast(ray, out RaycastHit hit, 15f))
                    {
                        switch (hit.transform.tag)
                        {
                            case "bacteria":
                                ChangeDirection(direction);
                                rayHit = true;
                                break;

                            case "bacteriaDetailed":
                                ChangeDirection(direction);
                                rayHit = true;
                                break;

                            case "cianobacteria":
                                ChangeDirection(direction);
                                rayHit = true;
                                break;

                            case "cianobacteriaDetailed":
                                ChangeDirection(direction);
                                rayHit = true;
                                break;

                            case "eukaryote":
                                ChangeDirection(direction);
                                rayHit = true;
                                break;

                            case "eukaryoteDetailed":
                                ChangeDirection(direction);
                                rayHit = true;
                                break;

                            default:

                                break;
                        }
                    }
                }

                if (this.tag == "eukaryote" || this.tag == "eukaryoteDetailed")
                {
                    if (Physics.Raycast(ray, out RaycastHit hit, 20f))
                    {
                        if (hit.transform.tag == "eukaryote" || hit.transform.tag == "eukaryoteDetailed")
                        {
                            ChangeDirection(direction);
                            rayHit = true;
                        }
                    }
                }

                if (this.tag == "bacteriaInfected" || this.tag == "bacteriaInfectedDetailed")
                    if (Physics.Raycast(ray, out RaycastHit hit, 15f))
                    {
                        ChangeDirection(direction);
                        rayHit = true;
                    }


                currentPosition = transform.position;
                if (currentPosition.x >= rangeX || currentPosition.z >= rangeZ)
                {
                    direction = 0;
                    WallTrigger = true;

                }
                else if (currentPosition.x >= rangeX || currentPosition.z <= -rangeZ)
                {
                    direction = 1;
                    WallTrigger = true;

                }
                else if (currentPosition.x <= -rangeX || currentPosition.z >= rangeZ)
                {
                    direction = 2;
                    WallTrigger = true;

                }
                else if (currentPosition.x <= -rangeX || currentPosition.z <= -rangeZ)
                {
                    direction = 3;
                    WallTrigger = true;

                }
                else if (currentPosition.x <= -rangeX)
                {
                    direction = 4;
                    WallTrigger = true;
                }
                else if (currentPosition.z <= -rangeZ)
                {
                    direction = 5;
                    WallTrigger = true;
                }
                else if (currentPosition.x >= rangeX)
                {
                    direction = 6;
                    WallTrigger = true;
                }
                else if (currentPosition.z >= rangeZ)
                {
                    direction = 7;
                    WallTrigger = true;
                }

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

                if (previousDirection != direction)
                    ExecuteRotation();
                if (WallTrigger)
                    ExecuteRotation();
                if (rayHit)
                    ExecuteRotation();

                if (Random.Range(0, directionDistance) == 0)
                {
                    previousDirection = direction;

                    direction = Random.Range(0, 8);
                    distanceX = Random.Range(0, 2) == 1 ? distanceX + speed : distanceX - speed;
                    distanceZ = Random.Range(0, 2) == 1 ? distanceZ - speed : distanceZ + speed;
                    distanceX = Random.Range(100.0f, 5000.0f);
                    distanceZ = Random.Range(100.0f, 5000.0f);
                }
            }
        }
    }
    void ExecuteRotation()
    {
        currentPosition = transform.position;
        startRotation = transform.rotation;

        if (currentPosition.x == previousPosition.x && currentPosition.z == previousPosition.z)
            goto equal;

        float azimuthMode = -1;

        float[] northVector = findVectorCoord(previousPosition.x, previousPosition.z, previousPosition.x + 10, previousPosition.z);

        float[] currentVector = findVectorCoord(previousPosition.x, previousPosition.z, currentPosition.x, currentPosition.z);

        if (currentPosition.z < previousPosition.z)
            azimuthMode = 1;

        float Azimuth = ((findAzimuth(northVector, currentVector) * 180 / Mathf.PI - 180) * azimuthMode) - startRotation.eulerAngles.y;

        Quaternion rotationY = Quaternion.AngleAxis(Azimuth, Vector3.up);
        transform.rotation = startRotation * rotationY;

        equal:
        previousPosition = currentPosition;
    }

    float findAzimuth(float[] NorthVector, float[] CurrentVector)
    {
        float skalar = NorthVector[0] * CurrentVector[0] + NorthVector[1] * CurrentVector[1];
        float moduleNorthVector = Mathf.Sqrt(NorthVector[0] * NorthVector[0] + NorthVector[1] * NorthVector[1]);
        float moduleCurrentVector = Mathf.Sqrt(CurrentVector[0] * CurrentVector[0] + CurrentVector[1] * CurrentVector[1]);

        float cosA = skalar / (moduleCurrentVector * moduleNorthVector);

        return Mathf.Acos(cosA);
    }

    float[] findVectorCoord(float xa, float za, float xb, float zb)
    {
        float[] res = { xb - xa, zb - za };
        return res;
    }

    public void Params(int spd, int distance, bool newdirection, int direction)
    {
        directionDistance = distance;
        speed = spd;
        NewDirection = newdirection;
        this.direction = direction;
        
    }
}