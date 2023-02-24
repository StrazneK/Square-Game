using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAnimator : MonoBehaviour
{
    public static EnemyAnimator instance;

    Animator anim;
    private void Awake()
    {
        instance = this;
    }
    private void Start()
    {
        anim = GetComponent<Animator>();
    }
    public void SetAnim(string animName, bool isTrue)
    {
        anim.SetBool(animName, isTrue);
    }
}
