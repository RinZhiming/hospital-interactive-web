using System;
using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using DG.Tweening;
using UnityEngine;
using UnityEngine.AI;

public class Nurse : MonoBehaviour
{
    [SerializeField] private Animator nurseAnimator;
    private void Start()
    {
        nurseAnimator.SetBool("Grounded", true);
        nurseAnimator.SetFloat("MoveSpeed", 0);
    }
}
