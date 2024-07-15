using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cube_Van : MonoBehaviour
{
    private UnityEngine.Material material;
    private float duration = 2f;
    private float timeElapsed = 0f;

    void Start()
    {
        material = GetComponent<Renderer>().material;
    }

    void Update()
    {
        timeElapsed += Time.deltaTime;
        float t = Mathf.Clamp01(timeElapsed / duration);
        float alpha = Mathf.Lerp(1f, 0f, t);
        Color color = material.color;
        color.a = alpha;
        material.color = color;
    }
}