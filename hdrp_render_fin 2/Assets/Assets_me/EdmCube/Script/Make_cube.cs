using UnityEngine;

public class CubeGrid : MonoBehaviour
{
    public GameObject cubePrefab; // ť�� ������
    public float cubeSize = 1f; // ť�� ũ��
    public int gridSize = 5; // �׸��� ũ��

    void Start()
    {
        // �׸��� ����
        for (int x = 0; x < gridSize; x++)
        {
            for (int y = 0; y < gridSize; y++)
            {
                for (int z = 0; z < gridSize; z++)
                {
                    // ť�� ����
                    GameObject cube = Instantiate(cubePrefab, transform);

                    // ť�� ��ġ ����
                    Vector3 cubePos = new Vector3(
                        x * cubeSize*2 - (gridSize - 1) * cubeSize / 2f,
                        y * cubeSize*2 - (gridSize - 1) * cubeSize / 2f,
                        z * cubeSize*2 - (gridSize - 1) * cubeSize / 2f
                    );
                    cube.transform.position = cubePos;

                    // ť�� �̸� ����
                    cube.name = "Cube (" + x + ", " + y + ", " + z + ")";
                }
            }
        }
    }
}
