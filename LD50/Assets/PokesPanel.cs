using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PokesPanel : MonoBehaviour
{
    public GameObject[] pokes;

    public void UpdatePanel()
    {
        for(int x = 0; x < pokes.Length; x++)
        {
            if(x < GameController.instance.remainingPokes)
            {
                pokes[x].SetActive(true);
            } else
            {
                pokes[x].SetActive(false);
            }
        }
        Debug.Log(GameController.instance.remainingPokes);
    }
}
