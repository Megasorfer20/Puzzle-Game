using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class tiempo : MonoBehaviour
{
    [SerializeField] private UnityEvent Final;

    public float timeStart;
    public TMP_Text textBoxing;
    bool timerActive = false;

    void Start()
    {
        textBoxing.text = timeStart.ToString("F0");
    }

    void Update()
    {
        if (timerActive)
        {
            timeStart -= Time.deltaTime;
            textBoxing.text = timeStart.ToString("F0");
        }

        if (timeStart <= 0)
        {
            Final.Invoke();
            timerActive = false;       
        } 
    }

    public void timerButton()
    {
        timerActive = !timerActive;
        textBoxing.text = timerActive ? "Count" : timeStart.ToString("F0");
    }

    public void pararTiempo()
    {
        timerActive = false;
    }

    public void reiniciarNivel()
    {
        SceneManager.LoadScene("Escenario1");
    }
}
