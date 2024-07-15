using UnityEngine;

public class NeonEdges : MonoBehaviour
{
    // 네온 효과에 사용되는 머티리얼
    public Material neonMaterial;

    // 큐브의 색상
    public Color cubeColor = Color.blue;

    // 큐브의 투명도
    [Range(0.0f, 1.0f)]
    public float cubeAlpha = 0.5f;

    // 큐브의 모서리 반짝임 강도
    [Range(0.0f, 1.0f)]
    public float edgeIntensity = 0.8f;

    // 큐브의 모서리 반짝임 주기
    public float edgeSpeed = 1.0f;

    // 큐브의 머티리얼과 색상을 설정합니다.
    void Start()
    {
        Renderer renderer = GetComponent<Renderer>();

        if (renderer != null)
        {
            renderer.material = neonMaterial;
            renderer.material.SetColor("_BaseColor", cubeColor);
            renderer.material.SetFloat("_Alpha", cubeAlpha);
        }
    }

    // 큐브의 모서리 반짝임을 업데이트합니다.
    void Update()
    {
        Renderer renderer = GetComponent<Renderer>();

        if (renderer != null)
        {
            renderer.material.SetFloat("_EdgeIntensity", edgeIntensity);
            renderer.material.SetFloat("_EdgeSpeed", edgeSpeed);
        }
    }
}
