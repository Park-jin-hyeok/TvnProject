using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public Transform target; // ī�޶� �ٶ� ��� (ť��)
    public Vector3 offset; // ī�޶� ��ġ���� �������� ������

    // Start is called before the first frame update
    void Start()
    {
        // ī�޶��� ��ġ�� �ʱ�ȭ�ϰ� ����� �ٶ󺸵��� �մϴ�.
        transform.position = target.position + offset;
        transform.LookAt(target);
    }

    // Update is called once per frame
    void Update()
    {
        // ī�޶� ����� ���� �����Դϴ�.
        transform.position = target.position + offset;
    }
}
