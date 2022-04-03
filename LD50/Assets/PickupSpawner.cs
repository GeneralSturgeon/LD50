using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickupSpawner : MonoBehaviour
{
    [SerializeField]
    private GameObject pickUpPrefab;
    private List<GameObject> pickups;

    [SerializeField]
    private int number;

    [SerializeField]
    private float maxX;
    [SerializeField]
    private float minX;
    [SerializeField]
    private float maxY;
    [SerializeField]
    private float minY;

    private void Start()
    {
        pickups = new List<GameObject>();
    }

    public void SpawnNewPickups()
    {
        foreach(GameObject pickup in pickups)
        {
            pickup.GetComponent<Pickup>().Kill();
        }

        pickups.Clear();

        for(int x = 0; x < number; x++)
        {
            float randX = Random.Range(minX, maxX);
            float randY = Random.Range(minY, maxY);

            var newObj = Instantiate(pickUpPrefab, new Vector3(randX, randY), Quaternion.identity);
            pickups.Add(newObj);
        }
    }
}
