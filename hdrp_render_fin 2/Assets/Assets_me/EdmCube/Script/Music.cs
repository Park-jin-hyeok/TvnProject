using UnityEngine;
using System.Collections;
using UnityEngine.Audio;

public class Music : MonoBehaviour
{
    // ť�� ������
    public GameObject cubePrefab;

    // ť�� ũ��
    public float cubeSize = 1f;

    // �������� �ӵ�
    public float lerpSpeed = 50f;

    // �ִϸ��̼� �ð�
    public float animationDuration = 2f;

    public Camera mainCamera;

    public AudioClip musicClip;
    private AudioSource audioSource;
    private int sampleSize = 128;
    private float[] samples;

    Vector3[,,] cubePositions = new Vector3[5, 5, 5];

    void Start()
    {
        // ����ũ ����
        audioSource = gameObject.AddComponent<AudioSource>();
        audioSource.clip = musicClip;
        audioSource.loop = true;
        audioSource.Play();
        samples = new float[sampleSize];

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

        // !!ī�޶� �ڷ� �̵�
        mainCamera.transform.position += mainCamera.transform.forward * -distance;

        // �ִϸ��̼� ����
        StartCoroutine(AnimateCubes());
    }

    void Update()
    {
        // ����� Ŭ�� ó��
        audioSource.GetSpectrumData(samples, 0, FFTWindow.BlackmanHarris);
        float averageVolume = 0;
        for (int i = 0; i < samples.Length; i++)
        {
            averageVolume += Mathf.Abs(samples[i]);
        }
        averageVolume /= samples.Length;

        // �߽� ť�� ����
        Transform centerCube = transform.Find("Cube (2, 2, 2)");

        // ť�� ������ �Ÿ� ���� (�ִϸ��̼ǿ� ������ ���� �ʵ���, �ִϸ��̼� ���� �ƴ� ���� ����)
        if (!IsInvoking("AnimateCubes"))
        {
            for (int x = 0; x < 5; x++)
            {
                for (int y = 0; y < 5; y++)
                {
                    for (int z = 0; z < 5; z++)
                    {
                        float randomRange = Random.Range(1, 10);

                        Transform cube = transform.Find("Cube (" + x + ", " + y + ", " + z + ")");
                        Vector3 direction = (cubePositions[x, y, z] - centerCube.position).normalized;
                        Vector3 targetPos = cubePositions[x, y, z] + direction * averageVolume * randomRange * 100;

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
        Transform centerCube = transform.Find("Cube (2, 2, 2)");

        // �ִϸ��̼� ���� �ð� ����
        float animationStartTime = Time.time;

        // �ִϸ��̼� �ð��� ���ݱ��� ���� (�߽� ť��� �̵�)
        while (Time.time - animationStartTime < animationDuration / 2f)
        {
            float t = (Time.time - animationStartTime) / (animationDuration / 2f);

            // �� ť�� �̵�
            for (int x = 0; x < 5; x++)
            {
                for (int y = 0; y < 5; y++)
                {
                    for (int z = 0; z < 5; z++)
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
            for (int x = 0; x < 5; x++)
            {
                for (int y = 0; y < 5; y++)
                {
                    for (int z = 0; z < 5; z++)
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
