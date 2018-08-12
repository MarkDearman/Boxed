using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sweat : MonoBehaviour {

    Vector3 endpos = new Vector3(0, -1, 0);
    float time = 0;

    private void Start()
    {
        GetComponent<SpriteRenderer>().enabled = false;
    }

    void Update ()
    {
        
        time += Time.deltaTime;

        if (time > 1)
        {
            GetComponent<SpriteRenderer>().enabled = true;
            transform.localPosition = Vector3.MoveTowards(transform.localPosition, endpos, Time.deltaTime);
            if (transform.localPosition == endpos)
            {
                Destroy(gameObject);
            }
        }
	}
}
