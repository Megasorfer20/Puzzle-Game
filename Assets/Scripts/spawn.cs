using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class spawn : MonoBehaviour
{
    [SerializeField] private GameObject player; 
    private bool canSpawn = true;

    public void SpawnPlayer()
    {
        if (canSpawn && player != null)
        {

            player.SetActive(false);

            player.transform.position = transform.position;

 
            player.SetActive(true);

            canSpawn = false; 
        }
    }
    public void EnableSpawn()
    {
        canSpawn = true;
    }
}
