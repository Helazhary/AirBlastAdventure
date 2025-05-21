using UnityEngine;
using UnityEngine.SceneManagement;

public class SpriteButton : MonoBehaviour
{
    private Vector3 originalScale;

    void Start()
    {
        originalScale = transform.localScale;
    }

    void OnMouseEnter()
    {
        // Animate hover (grow)
        transform.localScale = originalScale * 1.2f;
    }

    void OnMouseExit()
    {
        // Reset scale
        transform.localScale = originalScale;
    }

    void OnMouseDown()
    {
        // Click behavior — load next scene
        SceneManager.LoadScene("SampleScene"); // change to your scene name
    }
}
