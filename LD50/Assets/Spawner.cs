using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    private float sensitivity;
    private Vector3 mouseReference;
    private Vector3 mouseOffset;
    private Vector3 rotation;
    private bool isRotating;
    [SerializeField]
    private float maxRotation;
    [SerializeField]
    private GameObject planePrefab;

    void Start()
    {
        sensitivity = 0.4f;
        rotation = Vector3.zero;
    }

    void Update()
    {
        if (isRotating && StateManager.instance.currentGameState == StateManager.GameState.Ready)
        {
            mouseOffset = (Input.mousePosition - mouseReference);
            rotation.z = (mouseOffset.x + mouseOffset.y) * sensitivity;

            transform.Rotate(rotation);
            mouseReference = Input.mousePosition;

            if(transform.rotation.eulerAngles.z > maxRotation && transform.rotation.eulerAngles.z < 180f)
            {
                transform.rotation = Quaternion.Euler(0f, 0f, maxRotation);
            }

            if(transform.rotation.eulerAngles.z < 360f-maxRotation && transform.rotation.eulerAngles.z > 180f)
            {
                transform.rotation = Quaternion.Euler(0f, 0f, 360f-maxRotation);
            }


            
        }
    }

    void OnMouseDown()
    {
        isRotating = true;
        mouseReference = Input.mousePosition;
    }

    void OnMouseUp()
    {
        isRotating = false;
    }

    public void Spawn()
    {
        transform.rotation = Quaternion.Euler(0f, 0f, 0f);
        var newPlane = Instantiate(planePrefab, transform.position, Quaternion.identity);
        newPlane.transform.parent = transform;
        FindObjectOfType<CameraFollow>().InitiateSpawnPan(newPlane.transform);
    }
}
