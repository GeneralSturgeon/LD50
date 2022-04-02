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


    private float currentLift;
    private float currentSpeedAddition;
    private const float maxRot = 80f;

    [SerializeField]
    private Transform spawner;
    [SerializeField]
    private Transform aim;
    [SerializeField]
    private PolygonCollider2D col;
    private float startXpos;

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
            AddForceOnClick();
        }

        // Initialize
        if (rb.simulated == false && Input.GetMouseButtonUp(0) && StateManager.instance.currentGameState == StateManager.GameState.Ready)
        {
            rb.simulated = true;
            Vector2 force = (aim.position - spawner.position).normalized * startForce;
            rb.AddForce(force);
            Debug.Log(force);


            StateManager.instance.currentGameState = StateManager.GameState.Flight;
            FindObjectOfType<CameraFollow>().InitiateFlightFollow();
        }


        // Calculate lift addition
        if (rb.rotation <= minAOA && rb.rotation >= -minAOA)
        {
            currentLift = liftCurve.Evaluate(Mathf.InverseLerp(-minAOA, minAOA, rb.rotation)) * baseLift * rb.velocity.magnitude;
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
                rb.AddTorque(baseTorqueMultiplier * rb.rotation);
            }
            else
            {
                rb.AddTorque(-baseTorqueMultiplier * 2f * rb.rotation);
            }

            // Rotation depending on front wheight

            rb.AddTorque(-frontWheightForce);

            GameController.instance.UpdateDistance(transform.position.x - startXpos);
        }

        

    }

    public void AddForceOnClick()
    {
        //
        //float distance = (transform.position - Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y))).magnitude - 10f;
        //float distanceMultiplier = clickCurve.Evaluate(distance);

        // Adds upward rotation
        rb.AddTorque(baseClickTorque);
        // Add upward force
        rb.AddForce(new Vector2(0f, baseClickForce));
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("ground"))
        {
            StateManager.instance.currentGameState = StateManager.GameState.Crash;
            col.isTrigger = false;
            GameController.instance.OpenCrashPanel();
            transform.SetParent(null);
            Destroy(this);
        }
    }

}
