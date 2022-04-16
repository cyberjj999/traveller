using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MyPlayer : MonoBehaviour
{
    // Game object containing the win panel that shows the winning message
    public GameObject winPanel;

    Rigidbody playerRigidBody;
    float horizontalInput = 0f;
    float verticalInput = 0f;
    float distToGround;
    bool toJump = false;

    // Start is called before the first frame update
    void Start()
    {
        // Getting necessary components and component values for game logic
        playerRigidBody = GetComponent<Rigidbody>();
        distToGround = GetComponent<Collider>().bounds.extents.y;
    }

    // Update is called once per frame
    void Update()
    {
        // If user press Space and is on the ground
        if (Input.GetKeyDown(KeyCode.Space) && IsGrounded())
        {
            // Jump is set to true
            toJump = true;
        }

        // Get horizontal and vertical inputs from user
        // (horizontal input are 'A' and 'D' keys, or left/right arrow keys)
        // (vertical input are 'W' and 'S' keys, or up/down arrow keys)
        horizontalInput = Input.GetAxis("Horizontal");
        verticalInput = Input.GetAxis("Vertical");
    }

    private void FixedUpdate()
    {
        // If player needs to jump
        if (toJump)
        {
            // We add a force to the player's rigid body
            playerRigidBody.AddForce(Vector3.up * 8f, ForceMode.VelocityChange);
            // Reset jump (so player cannot jump until they are grounded again)
            toJump = false;
        }

        // Modify the transform of our player, moving it by horizontal and vertical input based on our controls
        transform.position += new Vector3(horizontalInput * 0.05f, 0f, verticalInput * 0.05f);

    }

    // Check if player is on the ground
    public bool IsGrounded()
    {
        return Physics.Raycast(transform.position, -Vector3.up, distToGround + 0.1f);
    }

    // Runs everytime the player collides with something
    private void OnCollisionEnter(Collision collision)
    {
        // If player collide with object with 'Win' tag (i.e. Win Zone)
        if (collision.gameObject.tag == "Win")
        {
            Debug.Log("Win");
            // Set WinPanel UI to be Active
            winPanel.SetActive(true);
        }
        // If player collide with object with 'Lose' tag (i.e. Lose Zone)
        if (collision.gameObject.tag == "Lose")
        {
            RestartGame(); // Restart the game
        }
    }

    // Restart the Game
    void RestartGame()
    {
        Scene scene = SceneManager.GetActiveScene();
        SceneManager.LoadScene(scene.name);
    }
}
