using UnityEngine;

public class CubeGrid : MonoBehaviour
{
    public GameObject cubePrefab; // 큐브 프리팹
    public float cubeSize = 1f; // 큐브 크기
    public int gridSize = 5; // 그리드 크기

    void Start()
    {
        // 그리드 생성
        for (int x = 0; x < gridSize; x++)
        {
            for (int y = 0; y < gridSize; y++)
            {
                for (int z = 0; z < gridSize; z++)
                {
                    // 큐브 생성
                    GameObject cube = Instantiate(cubePrefab, transform);

                    // 큐브 위치 설정
                    Vector3 cubePos = new Vector3(
                        x * cubeSize*2 - (gridSize - 1) * cubeSize / 2f,
                        y * cubeSize*2 - (gridSize - 1) * cubeSize / 2f,
                        z * cubeSize*2 - (gridSize - 1) * cubeSize / 2f
                    );
                    cube.transform.position = cubePos;

                    // 큐브 이름 설정
                    cube.name = "Cube (" + x + ", " + y + ", " + z + ")";
                }
            }
        }
    }
}
