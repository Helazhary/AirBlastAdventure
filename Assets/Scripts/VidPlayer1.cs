using UnityEngine;
using UnityEngine.Video;


public class VidPlayer1 : MonoBehaviour {
    
    [SerializeField] string videoFileName;
    public int test = 3;

    void Start()
    {
        PlayVideo();
    }


    public void PlayVideo()
    {
        VideoPlayer videoPlayer = GetComponent<VideoPlayer>();
    
        if (videoPlayer)
        {
            string videoPath = System.IO.Path.Combine(Application.streamingAssetsPath, videoFileName);
            Debug.Log(videoPath);
            videoPlayer.url = videoPath;
            videoPlayer.Play();
        }
    }


}
