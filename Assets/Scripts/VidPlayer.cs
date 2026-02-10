// // using System.ComponentModel.DataAnnotations;
// using UnityEngine;
// using UnityEngine.Video;

// using System.Collections;

// public class VidPlayer : MonoBehaviour
// {
//     [SerializeField] string videoFileName;
//     [SerializeField] private VideoPlayer videoPlayer;
//     [SerializeField] private string nextSceneName = "Scene"; // Change this to your next scene

//     // Start is called once before the first execution of Update after the MonoBehaviour is created
//     void Start()
//     {
//         videoPlayer.loopPointReached += OnVideoFinished;
//         StartCoroutine(PlayVideo());

//     }
    
//     void Update()
//     {
//         // //if spacebar is pressed play video
//         // if (Input.GetKeyDown(KeyCode.Space))
//         // {
//         //     PlayVideo();
//         // }
//     }

// IEnumerator PlayVideo()
// {
//     // Use a direct HTTPS link to the hosted video
//     videoPlayer.source = VideoSource.Url;
//     videoPlayer.url = "https://dgdp-8eca9.web.app/credits_scene_game.mp4";

//     // Mute the audio to allow autoplay on WebGL
//     videoPlayer.SetDirectAudioMute(0, true);

//     // Prepare video before playback
//     videoPlayer.Prepare();
//     while (!videoPlayer.isPrepared)
//     {
//         yield return null;
//     }

//     videoPlayer.Play();
//     Debug.Log("Video is now playing.");
// }

    
//      private void OnVideoFinished(VideoPlayer vp)
//     {
//         // Load the next scene when the video ends
//         SceneManager.LoadScene(nextSceneName);
//     }
   
// }


  
   