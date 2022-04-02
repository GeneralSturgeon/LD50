using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Parallax : MonoBehaviour
{
    [SerializeField]
    private Transform mainObj;

    [SerializeField]
    private float parallaxValue;

    [SerializeField]
    private Vector2 startPos;

    Vector2 travel => (Vector2)Camera.main.transform.position - startPos;

    private void FixedUpdate()
    {
        transform.position = startPos + travel * parallaxValue;
    }
}
