using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
public class PatrolController : MonoBehaviour
{
    public float distractedTime;
    public GameObject[] patrol;
    private int i = 0; //for incrimenting the array
    public NavMeshAgent agent;
    public float detectRadius;
    public LayerMask playerMask;
    private float distance;
    private Vector3 currentTarget;
    public Material matColor;
    public GameObject player;
    public GameObject cam;
    private Vector3 playerLoc;
    private Vector3 camLoc;


    private bool suspicious = false;
    private bool distracted = false;
    private bool disabled = false;

    
    // Start is called before the first frame update
    void Start()
    {
        GoHere(patrol[i].transform.position);
        camLoc = cam.transform.position;
        playerMask = ~playerMask;
        //print(playerMask);
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        playerLoc = player.transform.position;
        camLoc = cam.transform.position;
        if (Vector3.Distance(playerLoc, camLoc) < detectRadius) //check if the player is closer than 10 units
        {
            HeardSomething(playerLoc); //see if there's something in the way
        } else if (suspicious == true) //and the player is too far away
        {
            player.GetComponent<DistractorScript>().someoneSus = false;
            suspicious = false;
            //matColor.SetColor("_Color", Color.green);
        }
        
        

        distance = Vector3.Distance(currentTarget, transform.position);
        //print(distance);
        if (distance < 3 && !distracted && !disabled)
        {
            if (i == patrol.Length - 1) //reached the end of the list of waypoints
            {
                i = 0;
            } else
            {
                i += 1;
            }
            GoHere(patrol[i].transform.position);
        }
    }
    void GoHere(Vector3 point)
    {
        currentTarget = point;
        agent.SetDestination(currentTarget);
    }
    public IEnumerator Disabled() //for when the drone gets shot
    {        
        print("help I'm disabled");

        player.GetComponent<DistractorScript>().someoneSus = false; //make sure that player knows he's not sus

        float oldRadius = detectRadius;
        detectRadius = 0.01f; //blind
        disabled = true;
        GoHere(transform.position); //stop
        matColor.SetColor("_EmissionColor", Color.blue);
        yield return new WaitForSeconds(distractedTime);
        GoHere(patrol[i].transform.position);
        disabled = false;

        detectRadius = oldRadius; //reset
    }
    private IEnumerator Distracted(Vector3 distractedPoint)
    {
        distracted = true;
        print("heard something");

        GoHere(distractedPoint);
        yield return new WaitForSeconds(distractedTime);
        GoHere(patrol[i].transform.position);

        print("guess it was my imagination");
        distracted = false;
    }
    

    void OnTriggerEnter(Collider other)
    {
        print("Heard Something");
        if (other.gameObject.tag == "Noise" && !distracted)
        {
            StartCoroutine(Distracted(other.transform.position));
        }
    }

    public void HeardSomething(Vector3 other)
    { 
        //use a linecast to draw a line from guard to player and detect if there is an object in the way
        RaycastHit hit;
        Debug.DrawLine(camLoc, other);
        if (Physics.Linecast(camLoc, other, out hit, playerMask) || Vector3.Distance(playerLoc, camLoc) > detectRadius) //if something is in the way or player is too far away
        {
            matColor.SetColor("_EmissionColor", Color.green); //might need a better solution
            if (suspicious == true)
            {
                player.GetComponent<DistractorScript>().someoneSus = false;
                suspicious = false;
            }
        } else //nothing is blocking the line
        {
            matColor.SetColor("_EmissionColor", Color.red);
            player.GetComponent<DistractorScript>().someoneSus = true;
            suspicious = true;
        }
    }


}
