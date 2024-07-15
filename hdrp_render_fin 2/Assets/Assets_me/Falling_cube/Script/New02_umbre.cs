using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class New02_umbre : MonoBehaviour
{
    public float speed = 2.0f;

    void Update()
    {
        // 왼쪽 방향키를 누르면 큐브를 왼쪽으로 이동
        if (Input.GetKey(KeyCode.A))
        {
            transform.Translate(Vector3.left * speed * Time.deltaTime);
        }

        // 오른쪽 방향키를 누르면 큐브를 오른쪽으로 이동
        if (Input.GetKey(KeyCode.D))
        {
            transform.Translate(Vector3.right * speed * Time.deltaTime);
        }
    }
}