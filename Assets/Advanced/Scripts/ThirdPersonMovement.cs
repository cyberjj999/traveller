using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ThirdPersonMovement : MonoBehaviour
{
    public CharacterController controller;
    public Transform cam;
    public GameObject winPanel;

    public float speed = 2f;

    public float turnSmoothTime = 0.1f;
    float turnSmoothVelocity;

    public float gravity = -9.81f;
    public float jumpHeight = 3;
    Vector3 jumpVector;

    // Update is called once per frame
    void Update()
    {
        // Get horizontal & vertical inputs
        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");
        // Normalize direction to avoid speeding up when character moves diagonally
        Vector3 direction = new Vector3(horizontalInput, 0f, verticalInput).normalized;

        // If there are movement
        if (direction.magnitude >= 0.1f)
        {
            float targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg + cam.eulerAngles.y;
            float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref turnSmoothVelocity, turnSmoothTime);
            transform.rotation = Quaternion.Euler(0f, angle, 0f);

            Vector3 moveDir = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward;
            controller.Move(moveDir.normalized * speed * Time.deltaTime);
        }

        // Jump logic
        if (controller.isGrounded && jumpVector.y < 0)
        {
            jumpVector.y = -1f;
        }

        if (Input.GetButtonDown("Jump") && controller.isGrounded)
        {
            jumpVector.y = Mathf.Sqrt(jumpHeight * -1f * gravity);
        }

        // Gravity
        jumpVector.y += gravity * Time.deltaTime;
        controller.Move(jumpVector * Time.deltaTime);


        // Restart Game if Player Drops
        if (transform.position.y < -5)
            RestartGame();
    }

    private void OnControllerColliderHit(ControllerColliderHit collision)
    {
        if (collision.gameObject.tag == "Win")
        {
            Debug.Log("Win");
            winPanel.SetActive(true);
        }
    }

    void RestartGame()
    {
        Scene scene = SceneManager.GetActiveScene(); 
        SceneManager.LoadScene(scene.name);
    }
}
