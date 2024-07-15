using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeObjectAlpha : MonoBehaviour
{
    private Material material;
    private Color targetColor;
    private Color startColor;
    private float lerpSpeed = 1.0f;

    void Start()
    {
        Renderer renderer = GetComponent<Renderer>();
        material = renderer.material;
        startColor = material.GetColor("_Color");
        targetColor = startColor;
        targetColor = Color.black;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Z))
        {
            targetColor = Color.black;
        }
        else if (Input.GetKeyDown(KeyCode.X))
        {
            targetColor = startColor;
        }

        material.SetColor("_Color", Color.Lerp(material.GetColor("_Color"), targetColor, Time.deltaTime * lerpSpeed));
    }
}