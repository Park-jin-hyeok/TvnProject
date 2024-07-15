using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public Transform target; // 카메라가 바라볼 대상 (큐브)
    public Vector3 offset; // 카메라 위치에서 대상까지의 오프셋

    // Start is called before the first frame update
    void Start()
    {
        // 카메라의 위치를 초기화하고 대상을 바라보도록 합니다.
        transform.position = target.position + offset;
        transform.LookAt(target);
    }

    // Update is called once per frame
    void Update()
    {
        // 카메라가 대상을 따라 움직입니다.
        transform.position = target.position + offset;
    }
}
