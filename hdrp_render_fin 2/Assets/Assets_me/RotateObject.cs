using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;

public class RotateAndPlayVideo : MonoBehaviour
{
    private bool isRotating = false;
    public float rotateSpeed = 2000.0f; // ��ü�� �ʱ� ȸ�� �ӵ�
    public float rotateDeceleration = 500.0f; // ȸ�� ���ӵ�
    public KeyCode rotateKey = KeyCode.X;

    private float currentRotateSpeed = 2000; // ���� ȸ�� �ӵ�

    void Update()
    {
        if (Input.GetKeyDown(rotateKey))
        {
            isRotating = true;
        }

        if (isRotating)
        {
            // ��ü�� y���� �������� ȸ����ŵ�ϴ�.
            transform.Rotate(new Vector3(0, currentRotateSpeed * Time.deltaTime, 0));

            // ȸ�� �ӵ� ����
            currentRotateSpeed -= rotateDeceleration * Time.deltaTime;

            // ȸ�� �ӵ��� 0 �����̸� ȸ���� ����ϴ�.
            if (currentRotateSpeed <= 0)
            {
                isRotating = false;
            }
        }
    }
}