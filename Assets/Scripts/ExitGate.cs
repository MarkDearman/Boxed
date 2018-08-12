using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExitGate : MonoBehaviour {

    private void OnTriggerEnter (Collider c)
    {
        if (c.transform.tag == "Customer")
        {
            Destroy(c.gameObject);
        }
    }
}
