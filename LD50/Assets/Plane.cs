using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Plane : MonoBehaviour
{
    [SerializeField]
    private Rigidbody2D rb;
    public float startForce;
    public float minAOA;
    public float baseLift;
    public AnimationCurve liftCurve;
    public AnimationCurve speedCurve;
    public float frontWheightForce;
    public float baseTorqueMultiplier;
    public float baseSpeedMultiplier;

    public float baseClickForce;
    public float baseClickTorque;
    public AnimationCurve clickCurve;
    public float baseClickXForce;


    private float currentLift;
    private float currentSpeedAddition;
    private const float maxRot = 85f;

    [SerializeField]
    private Transform spawner;
    [SerializeField]
    private Transform aim;
    [SerializeField]
    private PolygonCollider2D col;
    private float startXpos;

    private float tick = 0f;
    private const float WAIT_TIME = 0.2f;
    private bool isWaiting = false;

    [SerializeField]
    private AudioSource leavesAudio;
    [SerializeField]
    private AudioSource pokeAudio;
    [SerializeField]
    private AudioSource pokeDeniedAudio;
    [SerializeField]
    private AudioSource deathAudio;
    [SerializeField]
    private AudioSource pickupAudio;
    [SerializeField]
    private AudioSource swooshAudio;



    private void Awake()
    {
        rb.simulated = false;
        spawner = FindObjectOfType<Spawner>().transform;
        startXpos = transform.position.x;
    }

    private void Start()
    {
        startForce = GameController.instance.startingForce[GameController.instance.startingForceUpgrades];
        baseLift = GameController.instance.lift[GameController.instance.liftUpgrades];
        minAOA = GameController.instance.aOA[GameController.instance.aOAUpgrades];
        frontWheightForce = GameController.instance.frontWeight[GameController.instance.frontWeightUpgrades];
    }

    private void Update()
    {

        if (StateManager.instance.currentGameState == StateManager.GameState.Flight && Input.GetMouseButtonUp(0))
        {
            if(GameController.instance.remainingPokes > 0)
            {
                AddForceOnClick();
                GameController.instance.remainingPokes--;
                GameController.instance.UpdatePokes();
                pokeAudio.Play();
            } else
            {
                pokeDeniedAudio.Play();
            }
             
        }

        // Initialize
        if (rb.simulated == false && Input.GetMouseButtonUp(0) && StateManager.instance.currentGameState == StateManager.GameState.Ready)
        {
            rb.simulated = true;
            Vector2 force = (aim.position - spawner.position).normalized * startForce;
            rb.AddForce(force);
            StateManager.instance.currentGameState = StateManager.GameState.Flight;
            FindObjectOfType<CameraFollow>().InitiateFlightFollow();
            leavesAudio.Play();

        }


        // Calculate lift addition
        if (rb.rotation <= minAOA && rb.rotation >= -minAOA)
        {
            currentLift = liftCurve.Evaluate(Mathf.InverseLerp(-minAOA, minAOA, rb.rotation)) * baseLift * rb.velocity.magnitude * Mathf.InverseLerp(100f, -3f, transform.position.y);
            Debug.Log(Mathf.InverseLerp(100f, -3f, transform.position.y));
        }
        else
        {
            currentLift = 0f;
        }

        // Calculate speed addition

        if (rb.rotation <= minAOA && rb.rotation >= -minAOA)
        {
            currentSpeedAddition = speedCurve.Evaluate(Mathf.InverseLerp(-minAOA, minAOA, rb.rotation)) * baseSpeedMultiplier;
        }
        else
        {
            currentSpeedAddition = 0f;
        }

    }

    private void FixedUpdate()
    {

        if(StateManager.instance.currentGameState == StateManager.GameState.Flight)
        {
            // Lift + speed depending on AOA
            rb.AddForce(new Vector2(currentSpeedAddition, currentLift));


            
            // Rotation and speed depending on AOA
            if (rb.rotation <= maxRot && rb.rotation >= -maxRot)
            {
                //rb.AddTorque(baseTorqueMultiplier * rb.rotation);
            }
            else
            {
                rb.AddTorque(-baseTorqueMultiplier * 5f * rb.rotation);
            }
            
            // Rotation depending on front wheight

            rb.AddTorque(-frontWheightForce);

            GameController.instance.UpdateDistance(transform.position.x - startXpos);
        }

        if (isWaiting)
        {
            tick += Time.fixedDeltaTime;
            if(tick >= WAIT_TIME)
            {
                isWaiting = false;
            }
        }

    }

    public void AddForceOnClick()
    {

        // Adds upward rotation
        rb.AddTorque(baseClickTorque);
        // Add upward force
        rb.AddForce(new Vector2(baseClickXForce, baseClickForce));
    }

    public void AddForceOnPickup()
    {
        if (!isWaiting)
        {
            // Adds upward rotation
            //rb.AddTorque(baseClickTorque/2f);
            // Add upward force
            rb.AddForce(new Vector2(baseClickXForce/2f, baseClickForce/2f));
            isWaiting = true;
        } 
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("ground"))
        {
            StateManager.instance.currentGameState = StateManager.GameState.Crash;
            deathAudio.Play();
            col.isTrigger = false;
            GameController.instance.OpenCrashPanel();
            transform.SetParent(null);
            Destroy(this);
        }

        if (collision.gameObject.CompareTag("pickup"))
        {
            AddForceOnPickup();
            collision.gameObject.GetComponent<Pickup>().Hide();
            pickupAudio.Play();
            if(rb.velocity.y > 1f)
            {
                swooshAudio.Play();
            }
        }
    }

}
