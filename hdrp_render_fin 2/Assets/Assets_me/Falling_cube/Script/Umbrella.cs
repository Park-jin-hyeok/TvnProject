using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Umbrella : MonoBehaviour
{
    Vector3 target = new Vector3(-6, 5, 0);
    bool k = false;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            k = true;
        }


        if (k == true)
        {
            transform.position = Vector3.Lerp(transform.position, target, 0.001f);
        }


    }
}
