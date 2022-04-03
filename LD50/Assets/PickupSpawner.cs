using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickupSpawner : MonoBehaviour
{
    [SerializeField]
    private List<GameObject> pickups;


    public void SpawnNewPickups()
    {
        foreach(GameObject pickup in pickups)
        {
            pickup.GetComponent<Pickup>().Restart();
        }
    }
}
