using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuPanel : MonoBehaviour
{
    [SerializeField]
    private Animator anim;

    public void PanelOut()
    {
        anim.SetTrigger("Out");
    }

}
