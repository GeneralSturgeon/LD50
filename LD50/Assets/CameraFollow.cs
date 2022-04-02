using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [SerializeField]
    private Transform objToFollow;
    [SerializeField]
    private float YposMax;
    private bool lockedOnPlane = false;
    private bool lerping = true;
    private float lerpTick;
    private float lerpSpeed;
    private Vector2 lerpStartPos;
    [SerializeField]
    private float XposOffset;

    private void FixedUpdate()
    {
        float YPos;

        if (objToFollow != null)
        {
            if (objToFollow.position.y > YposMax)
            {
                YPos = objToFollow.position.y - YposMax;
            }
            else
            {
                YPos = 0f;
            }
        } else
        {
            YPos = 0f;
        }
        
        if (StateManager.instance.currentGameState == StateManager.GameState.Flight && lockedOnPlane)
        {
            transform.position = new Vector3(objToFollow.position.x + XposOffset, YPos, -10f);
        }

        if (StateManager.instance.currentGameState == StateManager.GameState.Flight && lerping)
        {
            lerpTick += lerpSpeed * Time.fixedDeltaTime;
            transform.position = new Vector3(Mathf.Lerp(lerpStartPos.x, objToFollow.position.x + XposOffset, lerpTick), YPos, -10f);
        }


        if (StateManager.instance.currentGameState == StateManager.GameState.Menu && lerping)
        {
            lerpTick += lerpSpeed * Time.fixedDeltaTime;
            transform.position = new Vector3(Mathf.Lerp(lerpStartPos.x, objToFollow.position.x + XposOffset, lerpTick), YPos, -10f);
        }

    }

    public void InitiateFlightFollow()
    {
        StartCoroutine(LerpCameraOnStart());
    }

    IEnumerator LerpCameraOnStart()
    {
        lerpSpeed = 1f;
        lerpStartPos = transform.position;
        lerping = true;
        yield return new WaitForSeconds(1f);
        lerping = false;
        lerpTick = 0f;
        lockedOnPlane = true;
    }

    public void InitiateSpawnPan(Transform _newObj)
    {
        objToFollow = _newObj;
        lockedOnPlane = false;
        StartCoroutine(SpawnPan());
    }

    IEnumerator SpawnPan()
    {
        lerpSpeed = 2f;
        lerpStartPos = transform.position;
        lerping = true;
        yield return new WaitForSeconds(1f);
        lerping = false;
        lerpTick = 0f;
        StateManager.instance.currentGameState = StateManager.GameState.Ready;
    }
}
