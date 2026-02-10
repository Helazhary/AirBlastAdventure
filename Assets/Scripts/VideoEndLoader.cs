using UnityEngine;
using UnityEngine.Video;
using UnityEngine.SceneManagement;

public class VideoEndLoader : MonoBehaviour
{
    [SerializeField] private VideoPlayer videoPlayer;
    [SerializeField] private string nextSceneName = "Scene1"; // Change this to your next scene

    private void Start()
    {
        // Hook into the video end event
        videoPlayer.loopPointReached += OnVideoFinished;
        PlayVideo();
    }
    public void PlayVideo()
    {
        videoPlayer.Play(); // Only works after user interaction
    }
    private void OnVideoFinished(VideoPlayer vp)
    {
        // Load the next scene when the video ends
        SceneManager.LoadScene(nextSceneName);
    }
}
