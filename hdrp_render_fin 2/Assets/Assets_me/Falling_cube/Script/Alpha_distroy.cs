using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Alpha_distroy : MonoBehaviour
{
    public GameObject Main_Cube;

    //큐브 없애
    private float startTime;
    public float shrinkSpeed = 0.2f;

    //라이트 없애기
    public float intensityDelta = 2000f;

    // 인텐시티 감소 시간 (초)
    public float duration = 2.0f;

    private float targetIntensity;
    private float currentIntensity;
    private float elapsedTime;

    void Start()
    {

    }

    void Update()
    {
        if (Time.time - startTime > 15f) // 15초 이후에 실행
        {
            // 실행할 코드 작성
            Main_Cube.transform.localScale = Vector3.Lerp(Main_Cube.transform.localScale, Vector3.zero, shrinkSpeed * Time.deltaTime);
            if (currentIntensity > targetIntensity)
            {
                elapsedTime += Time.deltaTime;
                float t = elapsedTime / duration;
            } 
        }

    }
}