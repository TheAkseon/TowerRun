using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Animator))]
public class Human : MonoBehaviour
{
    [SerializeField] private Transform _fixationPoint;

    private Animator _animator;
    public Transform FixationPoint => _fixationPoint;

    private void Awake()
    {
        _animator = GetComponent<Animator>();
    }

    public void Run()
    {
        _animator.SetBool("isRunning", true);
    }

    public void StopRun()
    {
        _animator.SetBool("isRunning", false);
    }
}
