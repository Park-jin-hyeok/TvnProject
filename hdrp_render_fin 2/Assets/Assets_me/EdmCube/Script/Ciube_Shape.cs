using UnityEngine;

public class NeonEdges : MonoBehaviour
{
    // �׿� ȿ���� ���Ǵ� ��Ƽ����
    public Material neonMaterial;

    // ť���� ����
    public Color cubeColor = Color.blue;

    // ť���� ����
    [Range(0.0f, 1.0f)]
    public float cubeAlpha = 0.5f;

    // ť���� �𼭸� ��¦�� ����
    [Range(0.0f, 1.0f)]
    public float edgeIntensity = 0.8f;

    // ť���� �𼭸� ��¦�� �ֱ�
    public float edgeSpeed = 1.0f;

    // ť���� ��Ƽ����� ������ �����մϴ�.
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

    // ť���� �𼭸� ��¦���� ������Ʈ�մϴ�.
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
