using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AITestScript : MonoBehaviour
{
    public Camera cam;
    public NavMeshAgent agent;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = cam.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit))
            {
                agent.SetDestination(hit.point); //seems to be the main thing that sends people where we want them
                //maybe make an array of waypoints and have the guard cycle through them
                //as AI gets close to a waypoint, cycle to the next one
                //add a function to temporarily change the target point to the player's distraction for a few seconds
            }
        }
    }
}
