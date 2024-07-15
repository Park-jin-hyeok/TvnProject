using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CamerMoving : MonoBehaviour
{
    public KeyCode rotateKey = KeyCode.B;

    public Transform cameraTransform;
    public Vector3 targetPosition = new Vector3(0f, 10f, 0f);
    public float speed = 1.0f;



    private void Update()
    {
        if (Input.GetKeyDown(rotateKey))
        {
            // ���� ��ġ���� ��ǥ ��ġ���� ������ �ӵ��� �̵��մϴ�.
            cameraTransform.position = Vector3.Lerp(cameraTransform.position, targetPosition, speed * Time.deltaTime);
        }
    }
}   