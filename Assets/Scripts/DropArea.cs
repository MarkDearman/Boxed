using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DropArea : MonoBehaviour
{
    public Player player;
    public GameMaster GM;

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Interactable")
        {
            Transform o = player.GrabPoint.GetChild(0);
            o.SetParent(transform);
            o.GetComponent<Rigidbody>().isKinematic = false;
            o.GetComponent<Rigidbody>().useGravity = true;
            o.localPosition = new Vector3(0, 0, 0);
            player.ItemHeld = false;
            GM.SpaceLeft--;

            if (other.GetComponent<Box>().BoxTypeRec)
            {
                GM.score += 10;
            } else
            {
                GM.score -= 50;
            }
        }
    }
}
