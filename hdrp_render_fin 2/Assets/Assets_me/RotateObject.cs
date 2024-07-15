using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;

public class RotateAndPlayVideo : MonoBehaviour
{
    private bool isRotating = false;
    public float rotateSpeed = 2000.0f; // 물체의 초기 회전 속도
    public float rotateDeceleration = 500.0f; // 회전 감속도
    public KeyCode rotateKey = KeyCode.X;

    private float currentRotateSpeed = 2000; // 현재 회전 속도

    void Update()
    {
        if (Input.GetKeyDown(rotateKey))
        {
            isRotating = true;
        }

        if (isRotating)
        {
            // 물체를 y축을 기준으로 회전시킵니다.
            transform.Rotate(new Vector3(0, currentRotateSpeed * Time.deltaTime, 0));

            // 회전 속도 감소
            currentRotateSpeed -= rotateDeceleration * Time.deltaTime;

            // 회전 속도가 0 이하이면 회전을 멈춥니다.
            if (currentRotateSpeed <= 0)
            {
                isRotating = false;
            }
        }
    }
}