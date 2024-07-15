using UnityEngine;
using System.Collections;
using UnityEngine.Audio;

public class Cube_Bigger: MonoBehaviour
{
    // 큐브 프리팹
    public GameObject cubePrefab;

    // 큐브 크기
    public float cubeSize = 1f;

    // 애니메이션 시간
    public float animationDuration = 2f;

    // 마이크 입력 관련
    private AudioClip audioClip;
    private string micDevice;
    private int sampleSize = 128;
    private float[] samples;

    //선형보간 속도
    public float lerpSpeed = 5f;

    public Camera mainCamera;

    Vector3[,,] cubePositions = new Vector3[9, 9, 9];

    void Start()
    {
        // 마이크 설정
        micDevice = Microphone.devices[0];
        audioClip = Microphone.Start(micDevice, true, 1, 44100);
        samples = new float[sampleSize];

        // 그리드 생성
        for (int x = 0; x < 9; x++)
        {
            for (int y = 0; y < 9; y++)
            {
                for (int z = 0; z < 9; z++)
                {
                    // 큐브 생성
                    GameObject cube = Instantiate(cubePrefab, transform);

                    // 큐브 위치 설정
                    Vector3 cubePos = new Vector3(
                        x * cubeSize * 2,
                        y * cubeSize * 2,
                        z * cubeSize * 2
                    );
                    cube.transform.position = cubePos;

                    // 큐브 위치 배열에 저장
                    cubePositions[x, y, z] = cubePos;

                    // 큐브 이름 설정
                    cube.name = "Cube (" + x + ", " + y + ", " + z + ")";
                }
            }
        }
        mainCamera.transform.position = transform.Find("Cube (8, 8, 8)").transform.position;

        // 중심 큐브 설정
        Transform centerCube = transform.Find("Cube (4, 4, 4)");

        // 카메라를 중심 큐브를 향하게 설정
        mainCamera.transform.LookAt(centerCube);

        // 카메라와 큐브의 거리 설정
        float distance = 30f;

        // 카메라를 뒤로 이동
        mainCamera.transform.position += mainCamera.transform.forward * -distance;

        // 애니메이션 시작
        StartCoroutine(AnimateCubes());
    }

    void Update()
    {
        // 마이크 입력 처리
        audioClip.GetData(samples, 0);
        float averageVolume = 0;
        for (int i = 0; i < samples.Length; i++)
        {
            averageVolume += Mathf.Abs(samples[i]);
        }
        averageVolume /= samples.Length;

        // 중심 큐브 설정
        Transform centerCube = transform.Find("Cube (4, 4, 4)");

        // 큐브 사이의 거리 조절 (애니메이션에 영향을 주지 않도록, 애니메이션 중이 아닐 때만 실행)
        if (!IsInvoking("AnimateCubes"))
        {
            for (int x = 0; x < 9; x++)
            {
                for (int y = 0; y < 9; y++)
                {
                    for (int z = 0; z < 9; z++)
                    {
                        float randomRange = Random.Range(1, 10);

                        Transform cube = transform.Find("Cube (" + x + ", " + y + ", " + z + ")");
                        Vector3 direction = (cubePositions[x, y, z] - centerCube.position).normalized;
                        // !!음악넣었을때 큐브커지는 정도
                        Vector3 targetPos = cubePositions[x, y, z] + direction * averageVolume * 500;

                        // 선형 보간법을 이용한 부드러운 위치 조정
                        cube.position = Vector3.Lerp(cube.position, targetPos, lerpSpeed * Time.deltaTime);
                    }
                }
            }
        }
    }

    IEnumerator AnimateCubes()
    {
        // 중심 큐브 설정
        Transform centerCube = transform.Find("Cube (4, 4, 4)");

        // 애니메이션 시작 시간 설정
        float animationStartTime = Time.time;

        // 애니메이션 시간의 절반까지 실행 (중심 큐브로 이동)
        while (Time.time - animationStartTime < animationDuration / 2f)
        {
            float t = (Time.time - animationStartTime) / (animationDuration / 2f);

            // 각 큐브 이동
            for (int x = 0; x < 9; x++)
            {
                for (int y = 0; y < 9; y++)
                {
                    for (int z = 0; z < 9; z++)
                    {
                        Transform cube = transform.Find("Cube (" + x + ", " + y + ", " + z + ")");

                        // 큐브 초기 위치와 중심 큐브 위치의 차이
                        //Vector3 diff = cubePositions[x, y, z] - centerCube.position;

                        // 큐브 새로운 위치 계산 (중심 큐브로 이동)
                        Vector3 newPos = Vector3.Lerp(cubePositions[x, y, z], centerCube.position, t);

                        // 큐브 이동
                        cube.position = newPos;
                    }
                }
            }

            // 한 프레임 대기
            yield return null;
        }

        // 애니메이션 시작 시간 업데이트
        animationStartTime = Time.time;

        // 애니메이션 시간의 절반까지 실행 (원래 위치로 복귀)
        while (Time.time - animationStartTime < animationDuration / 2f)
        {
            float t = (Time.time - animationStartTime) / (animationDuration / 2f);
            // 각 큐브 이동
            for (int x = 0; x < 9; x++)
            {
                for (int y = 0; y < 9; y++)
                {
                    for (int z = 0; z < 9; z++)
                    {
                        Transform cube = transform.Find("Cube (" + x + ", " + y + ", " + z + ")");

                        // 큐브 초기 위치와 중심 큐브 위치의 차이
                        //Vector3 diff = cubePositions[x, y, z] - centerCube.position;

                        // 큐브 새로운 위치 계산 (원래 위치로 복귀)
                        Vector3 newPos = Vector3.Lerp(centerCube.position, cubePositions[x, y, z], t);

                        // 큐브 이동
                        cube.position = newPos;
                    }
                }
            }
            // 한 프레임 대기
            yield return null;
        }
    }
}