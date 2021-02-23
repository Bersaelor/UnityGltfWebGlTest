using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rotater : MonoBehaviour
{
    public float rotSpeed = 50.0f;
    private float nextTime;

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        transform.Rotate(0, rotSpeed * Time.deltaTime, 0);

        if (Time.time > nextTime) {
            // log.debug("Ping");
            nextTime = Time.time + 5;
        }
    }

}
