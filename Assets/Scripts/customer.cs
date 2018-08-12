using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class customer : MonoBehaviour
{

    private NavMeshAgent agent;
    public GameMaster GM;
    int CusNumber;

    public Transform GrabPoint;
    public GameObject Body;

    public Color[] colors;
    public int currentpos = -1;

    public float annoyance = 0;
    public float annoyanceMax = 10;

    bool moduleenable = true;

    public ParticleSystem Annoyed;

    public Transform sweatpos;
    public GameObject SwearObj;


    private void Start()
    {
        GM = GameObject.FindGameObjectWithTag("GM").GetComponent<GameMaster>();
        agent = GetComponent<NavMeshAgent>();
        Body.GetComponent<Renderer>().material.color = colors[Random.Range(0, colors.Length)];
        UpdateQueuePosition();
    }

    public void Update()
    {
        if (currentpos == 0)
        {
            annoyance += Time.deltaTime;

            if (annoyance > 5)
                Annoyed.gameObject.SetActive(true);

            if (annoyance > annoyanceMax)
            {
                GM.score -= 5;
                GM.Upsetcustom++;
                ExitWH();

                for (int x = 0; x < GM.Queue.Length; x++)
                {
                    GM.Queue[x].Ready = true;
                }
                for (int i = 0; i < GM.customers.Count; i++)
                {
                    GM.customers[i].GetComponent<customer>().UpdateQueuePosition();
                }

                if (GM.customers.Count == 0)
                {
                    GM.FinishLevel();
                }

            }
        }
    }

    public void Sweat()
    {
        GameObject swea = Instantiate(SwearObj, sweatpos.transform.position, Quaternion.identity);
        swea.transform.parent = sweatpos;
    }

    public void UpdateQueuePosition()
    {
        for (int i = 0; i < GM.Queue.Length; i++)
        {
            if (GM.Queue[i].Ready)
            {
                GotoPoint(GM.Queue[i].transform.position);
                GM.Queue[i].Ready = false;
                currentpos = i;
                return;
            }
        }
    }

    public void GotoPoint(Vector3 point)
    {
        agent.SetDestination(point);
    }

    public void ExitWH()
    {
        GM.Queue[currentpos].Ready = true;
        currentpos = -1;
        GM.customers.Remove(transform);
        GotoPoint(GM.exitpoint.position);
    }

}
