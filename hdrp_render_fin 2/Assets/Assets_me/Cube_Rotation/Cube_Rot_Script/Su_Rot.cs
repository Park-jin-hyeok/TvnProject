using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class Su_Rot : MonoBehaviour
{
    private bool isRotating = false;
    public KeyCode rotateKey = KeyCode.X;
    public KeyCode MultirotateKey = KeyCode.M;
    public float Multi = 2f;

    private bool multi = false;
    private float startAngle; // ȸ�� ���� ����
    private float endAngle = 120.0f; // ȸ�� ���� ����
    private float AddAngle = 0f; // ȸ�� ���� ����
    private float MultiAngle = 3.5f; // ȸ�� ���� ����
    private float rotateTime = 32.0f; // ȸ���ϴ� �� �ɸ��� �ð�
    private float rotateTimer; // ȸ�� �ð� ������ Ÿ�̸�


    void Update()
    {
        if (Input.GetKeyDown(rotateKey) && !isRotating)
        {
            multi = false;
            isRotating = true;
            startAngle = transform.rotation.eulerAngles.y;
            rotateTimer = 0.0f;
        }
        else if (Input.GetKeyDown(MultirotateKey) && isRotating)
        {
            isRotating = false;

            float currentAngle = transform.rotation.eulerAngles.y;
            float remainder = 136.1f - currentAngle;
            AddAngle += remainder;

            isRotating = true;
            startAngle = transform.rotation.eulerAngles.y;
            rotateTimer = 0.0f;
            multi = true;
        }
        else if (Input.GetKeyDown(MultirotateKey) && !isRotating)
        {
            isRotating = true;
            startAngle = transform.rotation.eulerAngles.y;
            rotateTimer = 0.0f;
            multi = true;
        }

        if (isRotating)
        {
            if (multi)
            {
                rotateTimer += Time.deltaTime;
                float t = Mathf.Clamp01(rotateTimer / 1);
                transform.rotation = Quaternion.Euler(0.0f, Mathf.Lerp(startAngle, startAngle + endAngle * MultiAngle + AddAngle, t), 0.0f);

                if (t >= 1.0f)
                {
                    isRotating = false;
                    endAngle += 0f;
                }
            }

            else
            {
                rotateTimer += Time.deltaTime;
                float t = Mathf.Clamp01(rotateTimer / rotateTime);
                transform.rotation = Quaternion.Euler(0.0f, Mathf.Lerp(startAngle, startAngle + endAngle, t), 0.0f);

                if (t >= 1.0f)
                {
                    isRotating = false;
                    endAngle += 0f;
                }
            }
        }
    }
}