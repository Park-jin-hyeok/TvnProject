using UnityEngine;

public class LightningEffect : MonoBehaviour
{
    public Light lightningLight;  // 포인트 라이트
    public float minIntensity = 0.1f;  // 최소 밝기
    public float maxIntensity = 5000f;  // 최대 밝기
    public float fadeOutSpeed = 5f;  // 꺼지는 속도

    private bool isLightning = false;  // 번개가 치는지 여부
    private float startTime;  // 번개 시작 시간

    // Sound
    public int sampleWindow = 64;
    private AudioClip microphoneClip;

    void Start()
    {
        MicrophoneToAudioClip();
    }

    private void Update()
    {
        Debug.Log(GetLoudnessFromMicrophone());
        if (GetLoudnessFromMicrophone() > 0.3)
        {
            if (!isLightning)
            {
                // 라이트가 깜빡거리도록 함
                InvokeRepeating("FlashLightning", 0f, 0.05f);
                isLightning = true;
                startTime = Time.time;
            }
        }

        if (isLightning)
        {
            // 라이트를 밝게 한다
            lightningLight.intensity = Mathf.Lerp(minIntensity, maxIntensity, (Time.time - startTime) / 0.5f);

            // 일정 시간이 지나면 라이트를 다시 어둡게 한다
            if (Time.time - startTime > 0.5f)
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
    }

    // 라이트를 깜빡거리게 하는 함수
    private void FlashLightning()
    {
        if (lightningLight.intensity == minIntensity)
        {
            lightningLight.intensity = maxIntensity;
        }
        else
        {
            lightningLight.intensity = minIntensity;
        }
        Debug.Log("Thunder");
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
