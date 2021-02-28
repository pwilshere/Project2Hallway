using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class DistractorScript : MonoBehaviour
{
    public Camera cam;
    //public NavMeshAgent agent;
    public GameObject noise;
    public GameObject bigNoise;
    public float cooldown;
    private float cooldownTimer = 0;
    public Text chargeText;
    private float chargePercent;

    public Text alertText;
    private float alertTimer = 0;
    public float alertMaxTime;
    public float alertLevel; //0 - 100
    private bool dangerMode;
    public bool someoneSus = false;

    public GameObject canvas;

    public GameObject gunMuzzle;

    public Image empBar;
    public Image alertBar;

    public AudioSource aud;
    
    // Start is called before the first frame update
    void Start()
    {
        AlertLevel(0);
    }

    // Update is called once per frame
    void Update()
    {
        if (cooldownTimer < cooldown)
        {
            cooldownTimer += Time.deltaTime;
            chargePercent = cooldownTimer / cooldown;
            empBar.fillAmount = chargePercent;
            
            /*chargePercent *= 100;
            chargePercent = Mathf.Round(chargePercent);
            chargeText.text = "Charge\n" + chargePercent;*/
        }

        if (someoneSus)
        {
            alertTimer += Time.deltaTime;
            alertLevel = alertTimer / alertMaxTime;
            alertLevel *= 100;
            alertLevel = Mathf.Round(alertLevel);

            if (dangerMode)
                alertBar.color = Color.red;
            else
                alertBar.color = Color.yellow;

            AlertLevel(alertLevel);
        } else if (alertLevel > 0) //and nobody is sus
        {
            alertTimer -= Time.deltaTime / 3;
            alertLevel = alertTimer / alertMaxTime;
            alertLevel *= 100;
            alertLevel = Mathf.Round(alertLevel);

            alertBar.color = Color.green;

            AlertLevel(alertLevel);
        }

        if (Input.GetMouseButtonDown(0) && cooldownTimer >= cooldown)
        {
            Ray ray = cam.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            
            if (Physics.Raycast(ray, out hit))
            {
                StartCoroutine(RenderLaser(hit.point));
                aud.Play();
                if (hit.collider.gameObject.CompareTag("Guard")) //hit a guard
                {    
                    print("hit a guard");
                    StartCoroutine(hit.collider.gameObject.GetComponentInParent<PatrolController>().Disabled());
                    SpawnNoise(hit.point, true);
                } else
                    SpawnNoise(hit.point, false);
            }
        }
    }

    public void AlertLevel(float a)
    {
        if (a > 50 && !dangerMode)
        {
            dangerMode = true;
            SpawnNoise(transform.position, true);
        } else if (a < 50 && dangerMode)
        {
            dangerMode = false;
        }
        if (a > 100)
        {
            alertBar.fillAmount = 1;
            //alertText.text = "Alert Level\n" + 100;
            canvas.GetComponent<PauseScript>().LoseGame();
        }else if (a < 0)
        {
            alertBar.fillAmount = 0;
            //alertText.text = "Alert Level\n" + 0;
        }
        else 
            alertBar.fillAmount = a/100;
            //alertText.text = "Alert Level\n" + a;
    }

    void SpawnNoise(Vector3 location, bool big)
    {
        print("spawned noise");

        GameObject sound;
                
        if (!big)
        {
            sound = Instantiate(noise, location, transform.rotation);
            cooldownTimer = 0;
        }
        else
        {
            sound = Instantiate(bigNoise, location, transform.rotation);
        }    

        
        Destroy(sound, 2.0f);
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Laser"))
        {
            print("hit laser");
                alertTimer = 0.7f * alertMaxTime;
                alertLevel = 70;
        }else if (other.CompareTag("Exit"))
        {
            canvas.GetComponent<PauseScript>().WinGame();
        }
    }
    IEnumerator RenderLaser(Vector3 target)
    {
        LineRenderer line = gameObject.GetComponent<LineRenderer>();
        List<Vector3> pos = new List<Vector3>();
        pos.Add(gunMuzzle.transform.position);
        pos.Add(target);

        line.startWidth = 0.01f;
        line.endWidth = 0.1f;
        line.startColor = Color.cyan;
        line.endColor = Color.blue;
        line.SetPositions(pos.ToArray());
        line.useWorldSpace = true;

        line.enabled = true;
        yield return new WaitForSeconds(0.1f);
        line.enabled = false;
    }
}
