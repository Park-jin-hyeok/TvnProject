using UnityEngine;
using UnityEngine.Video;
using System.Collections;
using UnityEngine.UI;


public class VideoController : MonoBehaviour
{
    public RawImage thumbnailImage;

    private VideoPlayer videoPlayer;

    IEnumerator Start()
    {
        videoPlayer = GetComponent<VideoPlayer>();

        // ����� �̹����� ������ ù ���������� �����մϴ�.
        videoPlayer.frame = 0;
        videoPlayer.Play();
        videoPlayer.Pause();

        while (!videoPlayer.isPrepared)
        {
            yield return null;
        }

        thumbnailImage.texture = videoPlayer.texture;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.X))
        {
            if (!videoPlayer.isPlaying)
            {
                videoPlayer.Play();
            }
            else
            {
                videoPlayer.Pause();
            }
        }
    }

}
