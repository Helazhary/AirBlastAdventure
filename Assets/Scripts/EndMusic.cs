using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public sealed class EndSceneFlow : MonoBehaviour
{
    [SerializeField] private AudioSource musicSource;
    [SerializeField] private string startSceneName = "SampleScene" ;

    private void Start()
    {
        if (musicSource == null || musicSource.clip == null) return;

        musicSource.loop = false;
        musicSource.Play();

        StartCoroutine(ReturnWhenFinished());
    }

    private IEnumerator ReturnWhenFinished()
    {
        // Wait until the audio time reaches the clip length (small tolerance)
        while (musicSource != null &&
               musicSource.isActiveAndEnabled &&
               musicSource.clip != null &&
               musicSource.time < musicSource.clip.length - 0.05f)
        {
            yield return null;
        }
        Debug.Log($"Loading scene: {startSceneName}");

        SceneManager.LoadScene(startSceneName);
    }
    
}
