using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rainwind : MonoBehaviour
{
    public GameObject myObject;
    Rigidbody rb;
    float speed = 1.0f;
    float rotationAmount = 40f;
    bool rKeyDown = false;
    bool tKeyDown = false;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            rKeyDown = true;
        }
        else if (Input.GetKeyDown(KeyCode.T))
        {
            tKeyDown = true;
        }

        if (Input.GetKey(KeyCode.R))
        {
            rb.AddForce(new Vector3(speed, 0f, speed), ForceMode.VelocityChange);
        }
        else if (Input.GetKey(KeyCode.T))
        {
            rb.AddForce(new Vector3(-speed, 0f, -speed), ForceMode.VelocityChange);
        }

        if (rKeyDown && !Input.GetKey(KeyCode.R))
        {
            myObject.transform.Rotate(new Vector3(0f, rotationAmount, 0f), Space.Self);
            rKeyDown = false;
        }
        else if (tKeyDown && !Input.GetKey(KeyCode.T))
        {
            myObject.transform.Rotate(new Vector3(0f, -rotationAmount, 0f), Space.Self);
            tKeyDown = false;
        }
    }
}