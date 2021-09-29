using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationState : MonoBehaviour
{
    public string State = "Idle";
    public float AnimSpeed = 1f;

    Animator anim;

    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();

        anim?.CrossFade(State, 0.01f);
        anim?.SetFloat("animSpeed", AnimSpeed);
    }
}
