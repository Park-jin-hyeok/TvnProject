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

        // ���̴��� _ColorSpeed �Ӽ��� �����մϴ�.
        rend.material.SetFloat("_ColorSpeed", colorSpeed);
    }
}
