using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateManager : MonoBehaviour
{
    public static StateManager instance;
    public enum GameState {Menu, Ready, Flight, Crash};
    [HideInInspector]
    public GameState currentGameState;


    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
            return;
        } else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        currentGameState = GameState.Ready;
    }


}
