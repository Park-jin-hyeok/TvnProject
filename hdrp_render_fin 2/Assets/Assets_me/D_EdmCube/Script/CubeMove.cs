using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class CubeMove : MonoBehaviour
{
    // Start is called before the first frame update
    public float Speed12;
    public float AngularSpeed;
    protected Rigidbody r;

    void Start()
    {
        r = GetComponent<Rigidbody>();
    }

    // Update is called once per frame

    void FixedUpdate()
    {
        Speed12 = r.velocity.magnitude;
        AngularSpeed = r.angularVelocity.magnitude;

        r.AddForce((0-transform.position.x)*3, (0-transform.position.y)*3, (0-transform.position.z)*3);
    }


}
