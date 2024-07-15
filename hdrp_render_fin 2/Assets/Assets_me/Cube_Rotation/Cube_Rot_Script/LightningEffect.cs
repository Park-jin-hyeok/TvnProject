using UnityEngine;

public class LightningEffect : MonoBehaviour
{
    public Light lightningLight;  // ����Ʈ ����Ʈ
    public float minIntensity = 0.1f;  // �ּ� ���
    public float maxIntensity = 5000f;  // �ִ� ���
    public float fadeOutSpeed = 5f;  // ������ �ӵ�

    private bool isLightning = false;  // ������ ġ���� ����
    private float startTime;  // ���� ���� �ð�

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
                // ����Ʈ�� �����Ÿ����� ��
                InvokeRepeating("FlashLightning", 0f, 0.05f);
                isLightning = true;
                startTime = Time.time;
            }
        }

        if (isLightning)
        {
            // ����Ʈ�� ��� �Ѵ�
            lightningLight.intensity = Mathf.Lerp(minIntensity, maxIntensity, (Time.time - startTime) / 0.5f);

            // ���� �ð��� ������ ����Ʈ�� �ٽ� ��Ӱ� �Ѵ�
            if (Time.time - startTime > 0.5f)
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
    }

    // ����Ʈ�� �����Ÿ��� �ϴ� �Լ�
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
