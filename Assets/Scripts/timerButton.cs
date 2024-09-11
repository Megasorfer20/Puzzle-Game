using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using TMPro;

public class timerButton : MonoBehaviour
{
    public GameObject plataforma;

    public bool timerCountDown = true;
    private float timer = 100000;
    void Update()
    {
        timer -= Time.deltaTime;

        if (timer%3 == 0)
        {
            timerCountDown = !timerCountDown;
            timer -= Time.deltaTime;
        }

        if (timerCountDown == true)
        {
            plataforma.SetActive(true);

        } else if (timerCountDown == false)
        {
            plataforma.SetActive(false);
        }
    }
}
