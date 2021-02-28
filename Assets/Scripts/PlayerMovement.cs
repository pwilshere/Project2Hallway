using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PlayerMovement : MonoBehaviour
{
    
    public CharacterController controller;
    public Transform groundCheck;
    public GameObject mainCamera;
    public float groundDistance = 0.4f;
    public LayerMask groundMask;
    public float speedScale = 12f; 
    public float gravity = -9.81f;
    public float jumpHeight = 3;
    public float duckDistance;
    private float speed;
    Vector3 velocity;
    bool isGrounded;

    //Camera bobbing variables
    private Vector3 cameraOriginalPosition; //starting position for camera
    private Vector3 cameraRestPos; //current rest position for the camera
    private Vector3 cameraDuckPos;
    public float transitionSpeed = 20f; //speed to return to rest
    public float bobSpeed = 4f; //speed of head bobs
    public float bobAmount = 0.3f; //intensity of bobs, can change with speed
    float timer;
    private bool isCrouching = false;
    private bool hasMacguffin = false;

    public GameObject alertMode;
    public GameObject exitDoor;

    public Text itemText;

    public AudioSource aud;


    // Start is called before the first frame update
    void Start()
    {
        speed = speedScale;
        
        cameraOriginalPosition = mainCamera.transform.localPosition; //resting position for camera when not moving
        cameraRestPos = cameraOriginalPosition;
        cameraDuckPos = cameraRestPos - new Vector3(0f,duckDistance,0f);

        itemText.text = "";
    }

    // Update is called once per frame
    void Update()
    {
        isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);
        
        if (isGrounded && velocity.y <0)
        {
            velocity.y = -5f;
        }
        
        if (Input.GetAxis("Crouch") > 0) // maybe change to a toggle to account for crouching when not moving
        {
            speed = speedScale / 2;
            cameraRestPos = cameraDuckPos;
            mainCamera.transform.localPosition = cameraDuckPos;
            bobAmount = 0.15f;
            isCrouching = true;
        } else if (isCrouching)
        {
            speed = speedScale;
            cameraRestPos = cameraOriginalPosition;
            mainCamera.transform.localPosition = cameraOriginalPosition;
            bobSpeed = 4f;
            bobAmount = 0.3f;
            isCrouching = false;
        }
        

        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");

        Vector3 move = transform.right * x + transform.forward * z;

        controller.Move(move * speed * Time.deltaTime);
        if ((move.x != 0 || move.z != 0) && Input.GetAxis("Crouch") == 0 && isGrounded)
            CameraBob(mainCamera, move);

        if (Input.GetButtonDown("Jump") && isGrounded)
        {
            velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
        }

        velocity.y += gravity * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime);


    }
    void CameraBob(GameObject cam, Vector3 moving)
    {
        timer += bobSpeed * Time.deltaTime; //need a personal timer here

        Vector3 newPos = new Vector3 (Mathf.Cos(timer)*bobAmount, cameraRestPos.y + Mathf.Abs(Mathf.Sin(timer)*bobAmount),cameraRestPos.z);
        cam.transform.localPosition = newPos;

    }

    void OnTriggerEnter (Collider other)
    {
        if (other.CompareTag("Item"))
        {
            hasMacguffin = true;
            alertMode.SetActive(true);
            exitDoor.SetActive(false);
            itemText.text = "Got the Goods!\nNow Get Out!";
            aud.Play();
            Destroy(other.gameObject);
        } else if (other.CompareTag("Exit"))
        {
            itemText.text = "You Win!";
        }
    }
}
