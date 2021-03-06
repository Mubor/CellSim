using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreateNewObj : MonoBehaviour
{
    int rand_obj;

    float[,] newCoord;

    GameObject[] simpleCells;
    GameObject[] detailCells;
    GameObject[] detailDeadCells;
    GameObject[] simpleDeadCells;
    GameObject[] infectedCells;
    GameObject[] eukaryotesEvo;


    private GameObject inst_obj;

    private void Awake()
    {
        simpleCells = Resources.LoadAll<GameObject>("SimpleCells");
        detailCells = Resources.LoadAll<GameObject>("DetailedCells");
        infectedCells = Resources.LoadAll<GameObject>("InfectedCells");
        detailDeadCells = Resources.LoadAll<GameObject>("DetailedDeadCells");
        simpleDeadCells = Resources.LoadAll<GameObject>("SimpleDeadCells");
        eukaryotesEvo = Resources.LoadAll<GameObject>("EukariothEvolution");
    }

    void Start()
    {
        newCoord = GenerateArray();
        // var tmp;
        //Создание префабов
        for (int i = 0; i < 100; i++)
        {
            rand_obj = Random.Range(0, 2);
            inst_obj = Instantiate(simpleCells[rand_obj], new Vector3(newCoord[i, 0], 0, newCoord[i, 1]), Quaternion.identity) as GameObject;

            switch (rand_obj)
            {
                case 0:
                    {
                        inst_obj.AddComponent<Bacteria>().SetParams(inst_obj, "bacteria", "bacteria_" + PlayerPrefs.GetInt("CountBacteria"), Random.Range(-10f, 101f), 5f, false, true, 0, 0);
                        PlayerPrefs.SetInt("CountBacteria", PlayerPrefs.GetInt("CountBacteria") + 1);
                    }
                    break;
                case 1:
                    {
                        inst_obj.AddComponent<CianoBacteria>().SetParams(inst_obj, "cianobacteria", "cianobacteria_" + PlayerPrefs.GetInt("CountCianobacteria"), Random.Range(-10f, 81f), 3f, false, true, 0, 0);
                        PlayerPrefs.SetInt("CountCianobacteria", PlayerPrefs.GetInt("CountCianobacteria") + 1);
                    }
                    break;
                case 2:
                    {
                        inst_obj.AddComponent<Virus>().SetParams(inst_obj, "virus", "virus_" + PlayerPrefs.GetInt("CountVirus"), Random.Range(50f, 100f), true, 0);
                        PlayerPrefs.SetInt("CountVirus", PlayerPrefs.GetInt("CountVirus") + 1);
                    }
                    break;
                default:
                    print("Unknown thing");
                    break;
            }
        }
    }

    public void RecreateObj(GameObject oldObj, string name, string tag, int numprefab, bool zoom, float hp, bool hasDaughter, int direction, int daughtercnt)
    {
        Transform transform = oldObj.transform;
        Destroy(oldObj.gameObject);

        oldObj = Instantiate(zoom == true ? detailCells[numprefab] : simpleCells[numprefab], new Vector3(transform.position.x, 0, transform.position.z), transform.rotation) as GameObject;
        switch (tag)
        {
            case "bacteria":
                {
                    oldObj.AddComponent<Bacteria>().SetParams(oldObj, tag, name, hp, transform.localScale.x, hasDaughter, false, direction, daughtercnt);
                }
                break;
            case "bacteriaDetailed":
                {
                    oldObj.AddComponent<Bacteria>().SetParams(oldObj, tag, name, hp, transform.localScale.x, hasDaughter, false, direction, daughtercnt);
                }
                break;
            case "cianobacteria":
                {
                    oldObj.AddComponent<CianoBacteria>().SetParams(oldObj, tag, name, hp, transform.localScale.x, hasDaughter, false, direction, daughtercnt);
                }
                break;
            case "cianobacteriaDetailed":
                {
                    oldObj.AddComponent<CianoBacteria>().SetParams(oldObj, tag, name, hp, transform.localScale.x, hasDaughter, false, direction, daughtercnt);
                }
                break;
            case "virus":
                {
                    oldObj.AddComponent<Virus>().SetParams(oldObj, tag, name, hp, false, direction);
                }
                break;
            case "virusDetailed":
                {
                    oldObj.AddComponent<Virus>().SetParams(oldObj, tag, name, hp, false, direction);
                }
                break;
            case "eukaryote":
                {
                    oldObj.AddComponent<Eukaryote>().SetParams(oldObj, tag, name, hp, transform.localScale.x, hasDaughter, false, direction, numprefab, daughtercnt);
                }
                break;
            case "eukaryoteDetailed":
                {
                    oldObj.AddComponent<Eukaryote>().SetParams(oldObj, tag, name, hp, transform.localScale.x, hasDaughter, false, direction, numprefab, daughtercnt);
                }
                break;
            default:
                print("U'do some mistake!");
                break;
        }
    }

    //Замена префабов для инфицированных клеток
    public void RecreateObjInfected(GameObject oldObj, Transform transform, string name, string tag, int speed, int dir, bool zoom, int infLvl, int direction)
    {
        Destroy(oldObj.gameObject);

        oldObj = Instantiate(zoom == true ? infectedCells[infLvl] : simpleCells[infLvl <= 6 ? 0 : infLvl <= 11 ? 1 : 3], new Vector3(transform.position.x, 0, transform.position.z), transform.rotation) as GameObject;

        oldObj.AddComponent<infectedCell>().setParams(oldObj, tag, name, transform.localScale.x, speed, dir, infLvl, direction);

    }

    //Замена префабов мертвой клетки
    public void RecreateObjDead(GameObject oldObj, string name, string tag, int numprefab, bool zoom)
    {
        Transform transform = oldObj.transform;
        Destroy(oldObj.gameObject);

        if ((tag == "preEukaryoteDead" || tag == "preEukaryoteDeadDetailed"))
            numprefab = numprefab < 5 ? 0 : 3;

        oldObj = Instantiate(zoom == true ? detailDeadCells[numprefab] : simpleDeadCells[numprefab], new Vector3(transform.position.x, 0, transform.position.z), transform.rotation) as GameObject;
        oldObj.AddComponent<DeadCell>().SetParams(oldObj, tag, name, transform.localScale.x, numprefab);
       
    }
   
    public void CreateSubsidiaryCell(GameObject parent, string tag, int numprefab, bool zoom, float scalebuffer)
    {
        GameObject subsidiary = Instantiate(zoom == true ? detailCells[numprefab] : simpleCells[numprefab], parent.transform.position, parent.transform.rotation) as GameObject;

        subsidiary.tag = tag;

        subsidiary.AddComponent<SubsidiaryCell>().SetParams(parent.name, subsidiary, numprefab, scalebuffer, true);
    }

    float[,] GenerateArray()
    {
        float[,] Coordinates = new float[PlayerPrefs.GetInt("CountSpawn"), 2];

        for (int i = 0; i < PlayerPrefs.GetInt("CountSpawn"); i++)
        {
            Coordinates[i, 0] = Random.Range(-1213.0f, 1213.0f);
            Coordinates[i, 1] = Random.Range(-560.0f, 560.0f);

            if (!CheckDistanceForArray(i, Coordinates) && i != 0)
                i--;
        }

        return Coordinates;
    }

    bool CheckDistanceForArray(int currentIndex, float[,] arr)
    {
        for (int i = 0; i < currentIndex; i++)
        {
            if (CheckDistance(arr[i, 0], arr[i, 1], arr[currentIndex, 0], arr[currentIndex, 1]))
                continue;
            else
                return false;
        }

        return true;
    }

    bool CheckDistance(float xa, float za, float xb, float zb)
    {
        if (Mathf.Sqrt(Mathf.Pow(xb - xa, 2) + Mathf.Pow(zb - za, 2)) > 5)
            return true;
        return false;
    }
}