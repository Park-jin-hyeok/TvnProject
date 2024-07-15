using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectController : MonoBehaviour
{
    private Vector3 targetSize;
    private float duration = 5.0f;
    private bool scaling = false;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Z))
        {
            // 크기 값 설정
            targetSize = new Vector3(65f, 65f, 65f);
            scaling = true;
            StartCoroutine(ScaleObject());
        }

        if (Input.GetKeyDown(KeyCode.B))
        {
            // 크기 값 설정
            targetSize = new Vector3(200f, 200f, 200f);
            scaling = true;
            StartCoroutine(ScaleObject());
        }
    }

    private IEnumerator ScaleObject()
    {
        Vector3 startSize = transform.localScale;
        float elapsedTime = 0f;
        while (elapsedTime < duration)
        {
            float t = Mathf.Clamp01(elapsedTime / duration);
            transform.localScale = Vector3.Lerp(startSize, targetSize, t);
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        scaling = false;
    }
}
