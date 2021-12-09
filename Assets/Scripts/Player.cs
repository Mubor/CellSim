using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    // Start is called before the first frame update
    int previousDirection;
    public int direction;
    Vector3 currentPosition;
    Vector3 previousPosition;
    Quaternion startRotation;
   
    void Start()
    {
        GameObject obj = this.gameObject;
        obj.AddComponent<Eukaryote>().SetParams(obj, tag, name, 50, 5, false, false, 0, 5, 0);
        previousDirection = -1;

        currentPosition = transform.position;
        previousPosition = currentPosition;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (Input.GetKey(KeyCode.W) && Input.GetKey(KeyCode.D))
        {
            transform.position = new Vector3(transform.position.x + 2, transform.position.y, transform.position.z + 2);
            direction = 1;
        }
        else if (Input.GetKey(KeyCode.W) && Input.GetKey(KeyCode.A))
        {
            transform.position = new Vector3(transform.position.x - 2, transform.position.y, transform.position.z + 2);
            direction = 2;
        }
        else if (Input.GetKey(KeyCode.S) && Input.GetKey(KeyCode.A))
        {
            transform.position = new Vector3(transform.position.x - 2, transform.position.y, transform.position.z - 2);
            direction = 3;
        }
        else if (Input.GetKey(KeyCode.S) && Input.GetKey(KeyCode.D))
        {
            transform.position = new Vector3(transform.position.x + 2, transform.position.y, transform.position.z - 2);
            direction = 4;
        }
        else if (Input.GetKey(KeyCode.S))
        {
            transform.position = new Vector3(transform.position.x, transform.position.y, transform.position.z - 2);
            direction = 5;
        }
        else if (Input.GetKey(KeyCode.W))
        {
            transform.position = new Vector3(transform.position.x, transform.position.y, transform.position.z + 2);
            direction = 6;
        }
        else if (Input.GetKey(KeyCode.D))
        {
            transform.position = new Vector3(transform.position.x + 2, transform.position.y, transform.position.z);
            direction = 7;
        }
        else if (Input.GetKey(KeyCode.A))
        {
            transform.position = new Vector3(transform.position.x - 2, transform.position.y, transform.position.z);
            direction = 8;
        }
    }

    private void Update()
    {
        if (previousDirection != direction && direction != null)
            ExecuteRotation();
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
}
