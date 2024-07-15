using UnityEngine;
using System.Collections;
using UnityEngine.Audio;

public class CubeAnimation : MonoBehaviour
{
    public int sampleWindow = 64;
    private AudioClip microphoneClip;

    // 큐브 프리팹
    public GameObject cubePrefab;

    // 큐브 크기
    public float cubeSize = 1f;

    // 선형보간 속도
    public float lerpSpeed = 50f;

    // 애니메이션 시간
    public float animationDuration = 2f;

    // 마이크 입력 관련
    /*
    private AudioClip audioClip;
    private string micDevice;
    private int sampleSize = 128;
    private float[] samples;
    */
    public Camera mainCamera;

    Vector3[,,] cubePositions = new Vector3[5, 5, 5];

    void Start()
    {
        // 마이크 설정
        /*
        micDevice = Microphone.devices[0];
        audioClip = Microphone.Start(micDevice, true, 1, 44100);
        samples = new float[sampleSize];
        */

        MicrophoneToAudioClip();

        // 그리드 생성
        for (int x = 0; x < 5; x++)
        {
            for (int y = 0; y < 5; y++)
            {
                for (int z = 0; z < 5; z++)
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
        mainCamera.transform.position = transform.Find("Cube (4, 4, 4)").transform.position;

        // 중심 큐브 설정
        Transform centerCube = transform.Find("Cube (2, 2, 2)");

        // 카메라를 중심 큐브를 향하게 설정
        mainCamera.transform.LookAt(centerCube);

        // 카메라와 큐브의 거리 설정
        float distance = 20f;

        // 카메라를 뒤로 이동
        mainCamera.transform.position += mainCamera.transform.forward * -distance;
    }

    public void MicrophoneToAudioClip()
    {
        string microphoneName = Microphone.devices[0];
        microphoneClip = Microphone.Start(microphoneName, true, 20, AudioSettings.outputSampleRate);
    }

    public float GetLoudnessFromMicrophone()
    {
        return GetLoudnessFromAudioClip(Microphone.GetPosition(Microphone.devices[0]), microphoneClip);
    }

    public float GetLoudnessFromAudioClip(int clipPosition, AudioClip clip)
    {
        int startPosition = clipPosition - sampleWindow;

        if (startPosition < 0)
            return 0;

        float[] waveData = new float[sampleWindow];
        clip.GetData(waveData, startPosition);

        float totalLoudness = 0;

        for (int i = 0; i < sampleWindow; i++)
        {
            totalLoudness += Mathf.Abs(waveData[i]);
        }

        return totalLoudness / sampleWindow;
    }

    void Update()
    {
        // 마이크 입력 처리
        /*
        audioClip.GetData(samples, 0);
        float averageVolume = 0;
        for (int i = 0; i < samples.Length; i++)
        {
            averageVolume += Mathf.Abs(samples[i]);
        }
        averageVolume /= samples.Length;
        */

        // 중심 큐브 설정

        Transform centerCube = transform.Find("Cube (2, 2, 2)");

        for (int x = 0; x < 5; x++)
        {
            for (int y = 0; y < 5; y++)
            {
                for (int z = 0; z < 5; z++)
                {
                    float randomRange = Random.Range(1, 10);

                    Transform cube = transform.Find("Cube (" + x + ", " + y + ", " + z + ")");
                    Vector3 direction = (cubePositions[x, y, z] - centerCube.position).normalized;
                    Vector3 targetPos = cubePositions[x, y, z] + direction * GetLoudnessFromMicrophone() * randomRange * 10;

                    // 선형 보간법을 이용한 부드러운 위치 조정
                    cube.position = Vector3.Lerp(cube.position, targetPos, lerpSpeed * Time.deltaTime);
                }
            }
        }

    }
}