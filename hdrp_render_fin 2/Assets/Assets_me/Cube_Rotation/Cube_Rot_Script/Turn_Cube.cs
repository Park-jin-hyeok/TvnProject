using UnityEngine;
using UnityEngine.SceneManagement;

public class Turn_Cube : MonoBehaviour
{
    public GameObject cube1;
    public float initialSpeed = 0f; // 초기 회전 속도
    private float currentSpeed; // 현재 회전 속도

    public float Lighting_Condition = 0.3f;
    private float lastLightningTime = -0.5f;
    public float CountLightningTime = 0.5f;
    private int flashCount;

    public float ABC = 5000;

    private bool isSpeedingUp = true; // 스페이스바가 눌렸는지 여부
    private bool isThunder = false; // 스페이스바가 눌렸는지 여부

    //    private float targetAngle = 0f; // 목표 회전 각도
    private Quaternion targetRotation; // 목표 회전 각도에 해당하는 쿼터니언

    //Thunder
    public Light lightningLight;  // 포인트 라이트
    public float minIntensity = 0.1f;  // 최소 밝기
    public float maxIntensity = 5000f;  // 최대 밝기
    public float fadeOutSpeed = 20f;  // 꺼지는 속도

    private bool isLightning = false;  // 번개가 치는지 여부
    private float startTime;  // 번개 시작 시간

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
                // 라이트가 깜빡거리도록 함
                //Debug.Log(Time.time - lastLightningTime);
                InvokeRepeating("FlashLightning", 0f, 0.05f);
                isLightning = true;
                startTime = Time.time;
                lastLightningTime = Time.time;  // Update the last lightning time
            }
        }

        if (isLightning)
        {
            // 라이트를 밝게 한다
            lightningLight.intensity = Mathf.Lerp(minIntensity, maxIntensity, (Time.time - startTime) / 0.5f);

            // 일정 시간이 지나면 라이트를 다시 어둡게 한다
            if (Time.time - startTime > 5f)
            {
                isLightning = false;
                CancelInvoke("FlashLightning");
            }
        }

        else
        {
            // 라이트를 서서히 어둡게 한다
            lightningLight.intensity = Mathf.Lerp(lightningLight.intensity, 0, fadeOutSpeed * Time.deltaTime);
        }

        // 스페이스바를 누르면 회전 속도를 증가시킵니다.
        /*
        if (Input.GetKeyDown(KeyCode.Space))
        {
            isSpeedingUp = true;
        }
        */

        // 스페이스바를 떼면 회전 속도를 초기 속도로 돌립니다.
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

        // 회전합니다.
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