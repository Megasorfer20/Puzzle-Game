using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ScriptCambio : MonoBehaviour
{

    [SerializeField] private UnityEvent pression;

    private void OnMouseDown()
    {
        pression.Invoke();
    }
}
