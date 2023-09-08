// Created by dylan@hathora.dev

using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.Serialization;

namespace Hathora.Demos.Shared.InputSystem
{
    /// <summary>
    /// [OLD INPUT SYSTEM] Minimal player controller, only requiring a RigidBody for advanced:
    /// - Move forward, back, left, right (WSAD)
    /// - Jump (space):
    ///   * Requires RigidBody and "Ground" layer set
    /// </summary>
    public class HathoraSimpleOldInputPlayerCtrl : MonoBehaviour
    {
        [Header("Debug")]
        [SerializeField, Tooltip("Having some issues? Log when we collide with the Floor layer!")]
        private bool verboseLogs = true;
        
        [SerializeField, Tooltip("Debug gravity issues by hurling your player down towards the ground")]
        private bool addVertForceDown;
        
        [Header("Settings")]
        [SerializeField]
        private float speed = 5.0f;

        [SerializeField]
        private float jumpForce = 10.0f;
        
        
        private bool canJump = true;
        private Rigidbody rb;


        private void Start()
        {
            rb = GetComponent<Rigidbody>();
            Assert.IsNotNull(rb);
            
            if (verboseLogs)
                Debug.Log($"[HathoraSimpleOldInputPlayerCtrl.Start] RigidBody found on '{rb.gameObject.name}");
        }

        void Update()
        {
            // Get vertical input ("w" or "s")
            float verticalInput = Input.GetAxis("Vertical");
            
            // Get horizontal input ("a" or "d")
            float horizontalInput = Input.GetAxis("Horizontal");

            // Move the capsule forward based on input
            Transform _transform = transform;
            Vector3 forwardMovement = _transform.forward * (verticalInput * speed * Time.deltaTime);
            Vector3 sidewaysMovement = _transform.right * (horizontalInput * speed * Time.deltaTime);

            // Apply the movement
            transform.Translate(forwardMovement + sidewaysMovement);

            // Jump
            if (Input.GetButtonDown("Jump") && canJump)
            {
                rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
                canJump = false;
            }
        }
        
        void FixedUpdate()
        {
            // Hurl straight down to the ground!
            if (addVertForceDown)
                rb.AddForce(Vector3.down * 10);
        }

        private void OnCollisionEnter(Collision collision)
        {
            if (verboseLogs)
                Debug.Log($"[HathoraSimpleOldInputPlayerCtrl.OnCollisionEnter]  Collision with {collision.gameObject.layer}");
    
            if (collision.gameObject.layer == LayerMask.NameToLayer("Ground"))
                canJump = true;
        }

    }
}
