using UnityEngine;
using UnityEngine.SceneManagement;

public class Turn_Cube : MonoBehaviour
{
    public GameObject cube1;
    public float initialSpeed = 0f; // �ʱ� ȸ�� �ӵ�
    private float currentSpeed; // ���� ȸ�� �ӵ�

    public float Lighting_Condition = 0.3f;
    private float lastLightningTime = -0.5f;
    public float CountLightningTime = 0.5f;
    private int flashCount;

    public float ABC = 5000;

    private bool isSpeedingUp = true; // �����̽��ٰ� ���ȴ��� ����
    private bool isThunder = false; // �����̽��ٰ� ���ȴ��� ����

    //    private float targetAngle = 0f; // ��ǥ ȸ�� ����
    private Quaternion targetRotation; // ��ǥ ȸ�� ������ �ش��ϴ� ���ʹϾ�

    //Thunder
    public Light lightningLight;  // ����Ʈ ����Ʈ
    public float minIntensity = 0.1f;  // �ּ� ���
    public float maxIntensity = 5000f;  // �ִ� ���
    public float fadeOutSpeed = 20f;  // ������ �ӵ�

    private bool isLightning = false;  // ������ ġ���� ����
    private float startTime;  // ���� ���� �ð�

    // Sound
    public int sampleWindow = 64;
    private AudioClip microphoneClip;

    // Start is called before the first frame update
    void Start()
    {
        MicrophoneToAudioClip();
        lightningLight.intensity = Mathf.Lerp(0, 0, 0f);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha8))
        {
            isThunder = true;
        }

        if (GetLoudnessFromMicrophone() > Lighting_Condition)
        {
            // Add a condition to check if 5 seconds have passed since the last lightning
            if (isThunder && !isLightning && Time.time - lastLightningTime > CountLightningTime)
            {
                // ����Ʈ�� �����Ÿ����� ��
                //Debug.Log(Time.time - lastLightningTime);
                InvokeRepeating("FlashLightning", 0f, 0.05f);
                isLightning = true;
                startTime = Time.time;
                lastLightningTime = Time.time;  // Update the last lightning time
            }
        }

        if (isLightning)
        {
            // ����Ʈ�� ��� �Ѵ�
            lightningLight.intensity = Mathf.Lerp(minIntensity, maxIntensity, (Time.time - startTime) / 0.5f);

            // ���� �ð��� ������ ����Ʈ�� �ٽ� ��Ӱ� �Ѵ�
            if (Time.time - startTime > 5f)
            {
                isLightning = false;
                CancelInvoke("FlashLightning");
            }
        }

        else
        {
            // ����Ʈ�� ������ ��Ӱ� �Ѵ�
            lightningLight.intensity = Mathf.Lerp(lightningLight.intensity, 0, fadeOutSpeed * Time.deltaTime);
        }

        // �����̽��ٸ� ������ ȸ�� �ӵ��� ������ŵ�ϴ�.
        /*
        if (Input.GetKeyDown(KeyCode.Space))
        {
            isSpeedingUp = true;
        }
        */

        // �����̽��ٸ� ���� ȸ�� �ӵ��� �ʱ� �ӵ��� �����ϴ�.
        /*
        if (Input.GetKeyUp(KeyCode.Space))
        {
            isSpeedingUp = false;
            float angleDiff = 125f - targetAngle % 90f;
            targetAngle = (targetAngle + angleDiff) % 360f;
            targetRotation = Quaternion.AngleAxis(targetAngle, Vector3.up);

        }
        */
        currentSpeed = GetLoudnessFromMicrophone() * 5000;//2500

        // ȸ���մϴ�.
        if (isSpeedingUp)
        {
            cube1.transform.Rotate(Vector3.up, currentSpeed * Time.deltaTime);
        }
        else
        {
            if (cube1.transform.rotation.eulerAngles.y % 90 == 35)
            {

            }
            cube1.transform.rotation = Quaternion.Slerp(cube1.transform.rotation, targetRotation, Time.deltaTime);
        }
    }
    private void FlashLightning()
    {
        if (flashCount >= 6)  // If the number of flashes is 6 or more, stop flashing
        {
            isLightning = false;
            CancelInvoke("FlashLightning");
            flashCount = 0;
            return;
        }

        if (lightningLight.intensity == minIntensity)
        {
            lightningLight.intensity = maxIntensity;
        }
        else
        {
            lightningLight.intensity = minIntensity;
        }
        flashCount++;  // Increment the flash count
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
}