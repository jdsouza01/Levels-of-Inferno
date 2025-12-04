using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public class cameraControl : MonoBehaviour
{

    public GameObject cam;
    public float ogSpeed = 0.5f; // Determines how quickly the character moves left and right
    public float moveSpeed;
    public InputAction playerControls; // Creates a variable to store the information of playerInputActions
    private Vector2 moveDirection = Vector2.zero; // Sets initial vector to zero


    // Called when the game runs
    private void Awake()
    {

    }

    // Requirement for input system
    private void OnEnable()
    {
        playerControls.Enable(); // Vital do not delete
    }

    // Requirement for input system
    private void OnDisable()
    {
        playerControls.Disable(); // Vital do not delete
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (!PlayerManager.paused)
            moveDirection = playerControls.ReadValue<Vector2>(); // Reads in the input from the keyboard
    }

    void FixedUpdate()
    {
        transform.Rotate(0, moveDirection.x * moveSpeed * -1, 0); // Moves the player left and right depending on which button was pressed
        if (moveDirection.x != 0 && !AudioManager.walking)
        {
            AudioManager.instance.startWalk();
            AudioManager.walking = true;
        }
        else if (AudioManager.walking && moveDirection.x == 0)
        {
            AudioManager.instance.stopWalk();
            AudioManager.walking = false;
        }

    }

    public void gameStart()
    {
        moveSpeed = ogSpeed;
    }


}
