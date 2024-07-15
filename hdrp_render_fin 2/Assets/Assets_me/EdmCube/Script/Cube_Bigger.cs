using UnityEngine;
using System.Collections;
using UnityEngine.Audio;

public class Cube_Bigger: MonoBehaviour
{
    // ť�� ������
    public GameObject cubePrefab;

    // ť�� ũ��
    public float cubeSize = 1f;

    // �ִϸ��̼� �ð�
    public float animationDuration = 2f;

    // ����ũ �Է� ����
    private AudioClip audioClip;
    private string micDevice;
    private int sampleSize = 128;
    private float[] samples;

    //�������� �ӵ�
    public float lerpSpeed = 5f;

    public Camera mainCamera;

    Vector3[,,] cubePositions = new Vector3[9, 9, 9];

    void Start()
    {
        // ����ũ ����
        micDevice = Microphone.devices[0];
        audioClip = Microphone.Start(micDevice, true, 1, 44100);
        samples = new float[sampleSize];

        // �׸��� ����
        for (int x = 0; x < 9; x++)
        {
            for (int y = 0; y < 9; y++)
            {
                for (int z = 0; z < 9; z++)
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
        mainCamera.transform.position = transform.Find("Cube (8, 8, 8)").transform.position;

        // �߽� ť�� ����
        Transform centerCube = transform.Find("Cube (4, 4, 4)");

        // ī�޶� �߽� ť�긦 ���ϰ� ����
        mainCamera.transform.LookAt(centerCube);

        // ī�޶�� ť���� �Ÿ� ����
        float distance = 30f;

        // ī�޶� �ڷ� �̵�
        mainCamera.transform.position += mainCamera.transform.forward * -distance;

        // �ִϸ��̼� ����
        StartCoroutine(AnimateCubes());
    }

    void Update()
    {
        // ����ũ �Է� ó��
        audioClip.GetData(samples, 0);
        float averageVolume = 0;
        for (int i = 0; i < samples.Length; i++)
        {
            averageVolume += Mathf.Abs(samples[i]);
        }
        averageVolume /= samples.Length;

        // �߽� ť�� ����
        Transform centerCube = transform.Find("Cube (4, 4, 4)");

        // ť�� ������ �Ÿ� ���� (�ִϸ��̼ǿ� ������ ���� �ʵ���, �ִϸ��̼� ���� �ƴ� ���� ����)
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
                        // !!���ǳ־����� ť��Ŀ���� ����
                        Vector3 targetPos = cubePositions[x, y, z] + direction * averageVolume * 500;

                        // ���� �������� �̿��� �ε巯�� ��ġ ����
                        cube.position = Vector3.Lerp(cube.position, targetPos, lerpSpeed * Time.deltaTime);
                    }
                }
            }
        }
    }

    IEnumerator AnimateCubes()
    {
        // �߽� ť�� ����
        Transform centerCube = transform.Find("Cube (4, 4, 4)");

        // �ִϸ��̼� ���� �ð� ����
        float animationStartTime = Time.time;

        // �ִϸ��̼� �ð��� ���ݱ��� ���� (�߽� ť��� �̵�)
        while (Time.time - animationStartTime < animationDuration / 2f)
        {
            float t = (Time.time - animationStartTime) / (animationDuration / 2f);

            // �� ť�� �̵�
            for (int x = 0; x < 9; x++)
            {
                for (int y = 0; y < 9; y++)
                {
                    for (int z = 0; z < 9; z++)
                    {
                        Transform cube = transform.Find("Cube (" + x + ", " + y + ", " + z + ")");

                        // ť�� �ʱ� ��ġ�� �߽� ť�� ��ġ�� ����
                        //Vector3 diff = cubePositions[x, y, z] - centerCube.position;

                        // ť�� ���ο� ��ġ ��� (�߽� ť��� �̵�)
                        Vector3 newPos = Vector3.Lerp(cubePositions[x, y, z], centerCube.position, t);

                        // ť�� �̵�
                        cube.position = newPos;
                    }
                }
            }

            // �� ������ ���
            yield return null;
        }

        // �ִϸ��̼� ���� �ð� ������Ʈ
        animationStartTime = Time.time;

        // �ִϸ��̼� �ð��� ���ݱ��� ���� (���� ��ġ�� ����)
        while (Time.time - animationStartTime < animationDuration / 2f)
        {
            float t = (Time.time - animationStartTime) / (animationDuration / 2f);
            // �� ť�� �̵�
            for (int x = 0; x < 9; x++)
            {
                for (int y = 0; y < 9; y++)
                {
                    for (int z = 0; z < 9; z++)
                    {
                        Transform cube = transform.Find("Cube (" + x + ", " + y + ", " + z + ")");

                        // ť�� �ʱ� ��ġ�� �߽� ť�� ��ġ�� ����
                        //Vector3 diff = cubePositions[x, y, z] - centerCube.position;

                        // ť�� ���ο� ��ġ ��� (���� ��ġ�� ����)
                        Vector3 newPos = Vector3.Lerp(centerCube.position, cubePositions[x, y, z], t);

                        // ť�� �̵�
                        cube.position = newPos;
                    }
                }
            }
            // �� ������ ���
            yield return null;
        }
    }
}