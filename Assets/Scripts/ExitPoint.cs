using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

public class ExitPoint : MonoBehaviour
{
    [SerializeField]
    private UnityEvent TriggerEnter = new();
    private void OnTriggerEnter(Collider other)
    {
        TriggerEnter.Invoke();
    }
}