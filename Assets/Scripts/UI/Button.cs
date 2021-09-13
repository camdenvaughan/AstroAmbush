using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Button : MonoBehaviour
{
    private Animator anim;
    
    void Start()
    {
        anim = GetComponent<Animator>();
    }

    public void SetButtonAnimState(int num)
    {
        switch (num)
        {
            case 0:
                anim.Play("Normal");
                break;
            case 1:
                anim.Play("Highlighted");
                break;
            case 2:
                anim.Play("Pressed");
                break;
        }
    }


}
