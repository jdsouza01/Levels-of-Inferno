/*Code taken from an online postbin and then had the movement removed and changed the camera controls to use the brand new Unity Input System. Actually this is very different from where I originally took it from*/

using UnityEngine;
using UnityEngine.InputSystem;

public class cameraLook : MonoBehaviour
{
    public Camera playerCamera; // Holds the camera object
    public static cameraLook instance;

    public float lookSpeed = 2f; // How quick the player looks around. Seems to jitter above 1f
    public float lookXLimit = 45.0f; // How far up and down you can look (I know weird how its opposite)
    public float lookYLimit = 45.0f; // Hour far left and right you can look

    float rotationX = 0; // This is just for universal use later on
    float rotationY = 0; // Same with this

    public bool canMove = true; // In case the menu is open we can easily shutdown the movement

    public CameraLookControls playerInput; // Takes in the player controls that are added
    private InputAction look; // Takes in specifically the look controls
    Vector2 cameraScope = Vector2.zero; // Sets the camera vector up for universal use on updates

    // Necessary for input to work
    private void OnEnable()
    {
        look = playerInput.Player.Look; // Assigns what input to look for in the Look variable
        look.Enable();
    }

    // Necessary for input to work
    private void OnDisable()
    {
        look.Disable();
    }

    // Must be done before the code run or it will not read it in time and create a null error
    private void Awake()
    {
        instance = this;
        playerInput = new CameraLookControls();
    }

    // Sets the cursor to the normal state for fps
    void Start()
    {
        // Cursor.lockState = CursorLockMode.Locked;
        // Cursor.visible = false;
    }

    // Reads the input values from the mouse movement
    void Update()
    {
        if(!PlayerManager.paused)
            cameraScope = look.ReadValue<Vector2>();
    }

    // Updates last to prevent jitter
    private void LateUpdate()
    {

        if (!PlayerManager.paused)
        {
            // Changes the rotation with consideration of look speed
            rotationX += -cameraScope.y * lookSpeed;
            rotationY += cameraScope.x * lookSpeed;

            // Locks the looking distance depending on what we set earlier on
            rotationX = Mathf.Clamp(rotationX, -lookXLimit, lookXLimit);
            rotationY = Mathf.Clamp(rotationY, -lookYLimit, lookYLimit);

            // Rotates!
            playerCamera.transform.localRotation = Quaternion.Euler(rotationX, rotationY, 0);
        }
    }
}