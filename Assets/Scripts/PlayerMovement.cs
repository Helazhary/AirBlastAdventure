// // using System.Diagnostics;
// using UnityEngine;

// public class PlayerMovement : MonoBehaviour
// {


 

//     // Code review : encapulsate visuals in one or more classes.
//     // For example, you could have a SquashAndStretch component,
//     // a Shine component, etc etc...
//     [Header("Visual Pump Feedback")]
//     [SerializeField] private Transform visualTransform;
//     [SerializeField] private Vector3 pumpScale = new Vector3(1.2f, 0.8f, 1f);
//     [SerializeField] private float scaleLerpSpeed = 5f;

//     // private bool isPumping = false;
//     // private float pumpStartTime;
//     // private float lastPumpEndTime = -999f;
//     private Vector3 defaultScale;

//     private Rigidbody2D rb;
//     private PlayerInput input;

    

//     private void Start()
//     {
//         rb = GetComponent<Rigidbody2D>();
//         // input = GetComponent<PlayerInput>();
      

//         // // Fallback assignment
//         // if (visualTransform == null && transform.childCount > 0)
//         // {
//         //     visualTransform = transform.GetChild(0);
//         //     // Debug.LogWarning("VisualTransform was auto-assigned: " + visualTransform.name);
//         // }

//         // defaultScale = visualTransform != null ? visualTransform.localScale : Vector3.one;
//     }

//     private void Update()
//     {
//         // HandlePumpLogic();
//         // HandleVisualPumpFeedback();
//     }



//     //    private void HandleVisualPumpFeedback()
//     // {
//     //     if (visualTransform == null) return;

//     //     Vector3 targetScale = isPumping ? pumpScale : defaultScale;
//     //     visualTransform.localScale = Vector3.Lerp(visualTransform.localScale, targetScale, scaleLerpSpeed * Time.deltaTime);
//     // } 

// }