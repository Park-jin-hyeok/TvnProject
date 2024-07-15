using UnityEngine;

public class NeonEdgeColorController : MonoBehaviour
{
    public float colorSpeed = 1.0f;
    private Renderer rend;

    void Start()
    {
        rend = GetComponent<Renderer>();
    }

    void Update()
    {
        float hue = Mathf.Repeat(Time.time * colorSpeed, 1.0f);
        Color edgeColor = Color.HSVToRGB(hue, 1, 1);
        rend.material.SetColor("_Color", edgeColor);

        // 쉐이더의 _ColorSpeed 속성을 변경합니다.
        rend.material.SetFloat("_ColorSpeed", colorSpeed);
    }
}
