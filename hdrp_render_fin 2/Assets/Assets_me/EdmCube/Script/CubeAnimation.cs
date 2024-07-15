using UnityEngine;
using System.Collections;
using UnityEngine.Audio;

public class CubeAnimation : MonoBehaviour
{
    public int sampleWindow = 64;
    private AudioClip microphoneClip;

    // ť�� ������
    public GameObject cubePrefab;

    // ť�� ũ��
    public float cubeSize = 1f;

    // �������� �ӵ�
    public float lerpSpeed = 50f;

    // �ִϸ��̼� �ð�
    public float animationDuration = 2f;

    // ����ũ �Է� ����
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
        // ����ũ ����
        /*
        micDevice = Microphone.devices[0];
        audioClip = Microphone.Start(micDevice, true, 1, 44100);
        samples = new float[sampleSize];
        */

        MicrophoneToAudioClip();

        // �׸��� ����
        for (int x = 0; x < 5; x++)
        {
            for (int y = 0; y < 5; y++)
            {
                for (int z = 0; z < 5; z++)
                {
                    // ť�� ����
                    GameObject cube = Instantiate(cubePrefab, transform);

                    // ť�� ��ġ ����
                    Vector3 cubePos = new Vector3(
                        x * cubeSize * 2,
                        y * cubeSize * 2,
                        z * cubeSize * 2
                    );
                    cube.transform.position = cubePos;

                    // ť�� ��ġ �迭�� ����
                    cubePositions[x, y, z] = cubePos;

                    // ť�� �̸� ����
                    cube.name = "Cube (" + x + ", " + y + ", " + z + ")";
                }
            }
        }
        mainCamera.transform.position = transform.Find("Cube (4, 4, 4)").transform.position;

        // �߽� ť�� ����
        Transform centerCube = transform.Find("Cube (2, 2, 2)");

        // ī�޶� �߽� ť�긦 ���ϰ� ����
        mainCamera.transform.LookAt(centerCube);

        // ī�޶�� ť���� �Ÿ� ����
        float distance = 20f;

        // ī�޶� �ڷ� �̵�
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
        // ����ũ �Է� ó��
        /*
        audioClip.GetData(samples, 0);
        float averageVolume = 0;
        for (int i = 0; i < samples.Length; i++)
        {
            averageVolume += Mathf.Abs(samples[i]);
        }
        averageVolume /= samples.Length;
        */

        // �߽� ť�� ����

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

                    // ���� �������� �̿��� �ε巯�� ��ġ ����
                    cube.position = Vector3.Lerp(cube.position, targetPos, lerpSpeed * Time.deltaTime);
                }
            }
        }

    }
}