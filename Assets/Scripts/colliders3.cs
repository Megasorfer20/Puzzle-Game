using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class colliders3 : MonoBehaviour
{
    [SerializeField] private UnityEvent entradas1, salidas1;
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag.Equals("Player"))
        {
            entradas1.Invoke();
            // FindObjectOfType<GameController>().PlayerWins();  // Invocar la victoria desde el GameController
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag.Equals("Player"))
        {
            salidas1.Invoke();
        }
    }
}
