using UnityEngine;
using UnityEngine.AI;

public class Player : MonoBehaviour
{
    private NavMeshAgent agent;
    public GameMaster GM;

    public Transform GrabPoint;
    public bool ItemHeld;

    public GameObject DropButtonArea;

    private void Start()
    {
        agent = GetComponent<NavMeshAgent>();
    }

    private void Update()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        //Check if the UI is active

        if (ItemHeld)
        {
            DropButtonArea.SetActive(true);
        }
        else
        {
            DropButtonArea.SetActive(false);
        }

        if (Input.GetMouseButtonDown(0))
        {
            if (Physics.Raycast(ray, out hit))
            {
                Move(hit.point);

                if(hit.transform.tag == "DropBox")
                {
                    DropObject();
                }

                if (hit.transform.tag == "NextLevel")
                {
                    GM.NextLevel();
                }
            }

        }


        if (Input.GetMouseButtonDown(1))
        {
            if (Physics.Raycast(ray, out hit))
            {


                if (hit.transform.tag == "Interactable")
                {
                    if(!ItemHeld)
                        PickupObject(hit.transform);
                }
                if (hit.transform.tag == "Customer")
                    if (hit.transform.GetComponent<customer>().currentpos == 0)
                    {
                        if(Vector3.Distance(transform.position, hit.transform.position) < GM.MaxPickupDistance)
                        {
                            giveObjectToCustomer(hit.transform.GetComponent<customer>());
                        }
                    }
            }
        }
    }

    //Movement
    private void Move(Vector3 Destination)
    {
        agent.SetDestination(Destination);
    }

    //Actions

    public void PickupObject (Transform Object)
    {
        if(Vector3.Distance(transform.position, Object.position) <= GM.MaxPickupDistance)
        {
            Object.GetComponent<Rigidbody>().isKinematic = true;
            Object.GetComponent<Rigidbody>().useGravity = false;
            Object.SetParent(GrabPoint);
            Object.localPosition = Vector3.zero;
            Object.rotation = GrabPoint.transform.localRotation;
            ItemHeld = true;
            GM.score += 5;
        }
    }

    public void DropObject ()
    {
        Transform o = GrabPoint.GetChild(0);
        o.GetComponent<Rigidbody>().isKinematic = false;
        o.GetComponent<Rigidbody>().useGravity = true;
        GrabPoint.GetChild(0).parent = null;
        //o.localPosition = new Vector3(o.localPosition.x, 0, o.localPosition.z);
        ItemHeld = false;
    }

    void giveObjectToCustomer(customer cus)
    {
        //TEST
        if (ItemHeld)
        {
            Transform o = GrabPoint.GetChild(0);
            Transform c = cus.GetComponent<Transform>().GetChild(1);
            GM.boxes.Remove(o);
            o.SetParent(c);
            o.transform.localPosition = Vector3.zero;
            ItemHeld = false;
            cus.ExitWH();
            GM.HappyCustom ++;
            GM.SpaceLeft--;

            if(GM.CustomersSpawned < GM.LevelCustomers[GM.level])
            {
                GM.SpawnCustomer();
            }

            if (o.GetComponent<Box>().BoxTypeRec)
            {
                GM.score -= 50;
                GM.Upsetcustom++;
                cus.Annoyed.gameObject.SetActive(true);
            }
            else
            {
                GM.score += 20;
            }
            for (int x = 0; x < GM.Queue.Length; x++)
            {
                GM.Queue[x].Ready = true;
            }
            for (int i = 0; i < GM.customers.Count; i++)
            {
                GM.customers[i].GetComponent<customer>().UpdateQueuePosition();
            }

            if(GM.customers.Count == 0)
            {
                GM.FinishLevel();
            }
        }
        //END TEST
    }

}
