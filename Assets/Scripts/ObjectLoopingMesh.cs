using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectLoopingMesh : MonoBehaviour
{
    public Transform Spawn;

    private void OnTriggerEnter(Collider other)
    {
        other.transform.position = Spawn.position;
    }
}
