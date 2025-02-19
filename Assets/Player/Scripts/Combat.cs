using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Combat : MonoBehaviour
{
    private Animator anim;
    public float cooldownTime;
    private float nextFireTime = 0.0f;
    public static int noOfClicks;
    float lastClickedTime;
    float maxComboDelay;

    void Start()
    {
        anim = GetComponent<Animator>();        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnClick()
    {
        lastClickedTime = Time.time;
        noOfClicks++;   
        if (noOfClicks == 1)
        {

        }
    }
}
