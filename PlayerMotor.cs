using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMotor : MonoBehaviour
{
    private CharacterController controller;
    private Vector3 playerVelocity;
    private float speed = 5f;
    private bool isGrounded;
    public float gravity = -9.8f;

    public float jumpHeight = 3f;
    bool crouching = false;
    float crouchTimer = 1;
    bool lerpCrouch = false;
    bool sprinting = false;
    bool interacting = false;

    // Start is called before the first frame update
    void Start()
    {
        controller = GetComponent<CharacterController>();
    }

    // Update is called once per frame
    void Update()
    {
        isGrounded = controller.isGrounded;
        if (lerpCrouch)
        {
            crouchTimer += Time.deltaTime;
            float p = crouchTimer / 1;
            p *= p;
            if (crouching)
                controller.height = Mathf.Lerp(controller.height, 1, p);
            else
                controller.height = Mathf.Lerp(controller.height, 2, p);
            if (p > 1)
            {
                lerpCrouch = false;
                crouchTimer = 0f;
            }
        }

        if (interacting)
        {
            // Put your code here to handle the interaction, e.g., open the door.
            // You can use raycasts or trigger colliders to detect nearby interactable objects.
            // Example: If an interactable object is in front of the player and the player presses the interact button,
            // trigger the interaction logic.
        }
    }

    // Receive Input from our InputManager Script and apply them to the Character Controller
    public void ProcessMove(Vector2 input)
    {
        if (interacting)
        {
            // If the player is interacting, don't move during the interaction.
            return;
        }

        Vector3 moveDirection = Vector3.zero;
        moveDirection.x = input.x;
        moveDirection.z = input.y;
        controller.Move(transform.TransformDirection(moveDirection) * speed * Time.deltaTime);
        playerVelocity.y += gravity * Time.deltaTime;
        if (isGrounded && playerVelocity.y < 0)
            playerVelocity.y = -2f;
        controller.Move(playerVelocity * Time.deltaTime);
    }

    public void Crouch()
    {
        if (interacting)
        {
            // If the player is interacting, don't crouch during the interaction.
            return;
        }

        crouching = !crouching;
        crouchTimer = 0;
        lerpCrouch = true;
    }

    public void Sprint()
    {
        if (interacting)
        {
            // If the player is interacting, don't sprint during the interaction.
            return;
        }

        sprinting = !sprinting;
        if (sprinting)
            speed = 8;
        else
            speed = 5;
    }

    public void Jump()
    {
        if (interacting)
        {
            // If the player is interacting, don't jump during the interaction.
            return;
        }

        if (isGrounded)
        {
            playerVelocity.y = Mathf.Sqrt(jumpHeight * -3.0f * gravity);
        }
    }

    // Call this method to start the interaction.
    public void StartInteraction()
    {
        interacting = true;
    }

    // Call this method to end the interaction.
    public void EndInteraction()
    {
        interacting = false;
    }
}
