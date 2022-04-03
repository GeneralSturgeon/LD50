using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pickup : MonoBehaviour
{

    [SerializeField]
    private SpriteRenderer rend;
    [SerializeField]
    private CircleCollider2D col;
   public void Kill()
    {
        gameObject.SetActive(false);
        Destroy(gameObject, 1f);
    }

    public void Hide()
    {
        rend.enabled = false;
        col.enabled = false;
    }
}
