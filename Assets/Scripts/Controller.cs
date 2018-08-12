using UnityEngine;
using UnityEngine.AI;
using System.Collections;

public class Controller : MonoBehaviour
{

    public Transform GrabPoint;
    private bool itemInHand;

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit))
            {
                if (hit.transform.tag == "Interactable")
                {
                    if (!itemInHand)
                    {
                        Pickup(hit.transform);
                    }
                }
            }
        }
    }

    public void GoToPos (Vector3 position)
    {
        NavMeshAgent agent = GetComponent<NavMeshAgent>();
        agent.SetDestination(position);
    }

    public void Pickup (Transform Item)
    {

        //while (Vector3.Distance(transform.position, Item.position) > 2)
        //{
        //    GoToPos(Item.position);
        //}

        if (Vector3.Distance(transform.position, Item.position) <= 2)
        {
            Item.SetParent(GrabPoint);
            Item.localPosition = Vector3.zero;
            itemInHand = true;
        }
    }

    public void DropHeldItem()
    {
        if (GrabPoint.childCount != 0)
        {
            Transform childobj = GrabPoint.GetChild(0);
            childobj.position = new Vector3(childobj.position.x, 0, childobj.position.z);
            childobj.parent = null;
            itemInHand = false;
        }
    }
}
