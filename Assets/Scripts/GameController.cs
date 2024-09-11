using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Video;

public class GameController : MonoBehaviour
{
    public enum GameMode { SinglePlayer, TwoPlayers }
    public GameMode currentGameMode;

    [Header("UI Elements")]
    public GameObject readyModal;          // Modal de "Listo"
    public GameObject winModal;            // Modal de victoria
    public GameObject loseModal;           // Modal de derrota
    public GameObject nextPlayerModal;     // Modal de siguiente jugador (dos jugadores)
    public TMP_Text timerText;             // Temporizador en la UI
    public TMP_Text playerIndicatorText;   // Indicador de Jugador 1 o 2
    public TMP_Text livesText;             // Texto para mostrar las vidas restantes (solo un jugador)

    public Button restartButton;           // Bot�n para reiniciar
    public Button homeButton;              // Bot�n para volver al men� principal
    public Button readyButton;             // Bot�n de "Listo" en el modal

    [Header("Video Settings")]
    public VideoPlayer winVideo;           // Video de victoria
    public RawImage videoDisplay;          // RawImage donde se muestra el video

    [Header("Game Mode Buttons")]
    public Button singlePlayerButton;      // Bot�n de Un Jugador
    public Button twoPlayerButton;         // Bot�n de Dos Jugadores

    private float timeRemaining = 30f;     // Tiempo inicial
    private bool timerIsRunning = false;   // Control del temporizador
    private bool playerOneTurn = true;     // Para dos jugadores
    private bool gameOver = false;         // Control del estado del juego
    private int playerLives = 2;           // Vidas (solo un jugador)

    void Start()
    {
        // Asignar funciones a los botones
        singlePlayerButton.onClick.AddListener(() => StartGame(GameMode.SinglePlayer));
        twoPlayerButton.onClick.AddListener(() => StartGame(GameMode.TwoPlayers));
    }

    void Update()
    {
        if (timerIsRunning && !gameOver)
        {
            if (timeRemaining > 0)
            {
                timeRemaining -= Time.deltaTime;
                timerText.text = timeRemaining.ToString("F0") + "s";
            }
            else
            {
                timerIsRunning = false;
                EndGame(false);  // Si se acaba el tiempo, el jugador pierde
            }
        }
    }

    // Mostrar modal de "Listo"
    void ShowReadyModal()
    {
        readyModal.SetActive(true);
        timerIsRunning = false;
    }

    // Funci�n para iniciar el juego
    public void StartGame(GameMode mode)
    {
        currentGameMode = mode;
        timeRemaining = 30f;
        gameOver = false;

        if (currentGameMode == GameMode.SinglePlayer)
        {
            playerLives = 2;
            livesText.gameObject.SetActive(true);
            UpdateLivesText();
        }
        else
        {
            playerIndicatorText.gameObject.SetActive(true);
            UpdatePlayerIndicator();
            livesText.gameObject.SetActive(false);
        }

        ShowReadyModal();
    }

    // Iniciar el temporizador al pulsar "Listo"
    public void StartTimer()
    {
        timerIsRunning = true;
        readyModal.SetActive(false);
    }

    // Funci�n que se llama cuando el jugador gana
    public void PlayerWins()
    {
        StartCoroutine(PlayWinVideoAndShowModal());
    }

    // Reproduce el video de victoria en el RawImage y luego muestra el modal
    IEnumerator PlayWinVideoAndShowModal()
    {
        // Asigna la textura del video al RawImage
        videoDisplay.texture = winVideo.targetTexture;
        winVideo.gameObject.SetActive(true);
        winVideo.Play();

        // Espera a que termine el video
        yield return new WaitForSeconds((float)winVideo.clip.length);

        winVideo.gameObject.SetActive(false);
        ShowWinModal();
    }

    // Mostrar el modal de victoria
    void ShowWinModal()
    {
        winModal.SetActive(true);
        timerIsRunning = false;
        gameOver = true;
    }

    // Finalizar el juego (gane o pierda)
    void EndGame(bool didWin)
    {
        timerIsRunning = false;
        gameOver = true;

        if (didWin)
        {
            ShowWinModal();
        }
        else
        {
            if (currentGameMode == GameMode.SinglePlayer && playerLives > 0)
            {
                playerLives--;
                UpdateLivesText();
                ShowReadyModal();  // Si a�n hay vidas, reintentar
            }
            else if (currentGameMode == GameMode.TwoPlayers)
            {
                ShowNextPlayerModal();  // Si es modo de dos jugadores, cambiar de jugador
            }
            else
            {
                ShowLoseModal();  // Perdiste, mostrar modal de derrota
            }
        }
    }

    // Mostrar el modal de siguiente jugador (dos jugadores)
    void ShowNextPlayerModal()
    {
        nextPlayerModal.SetActive(true);
        playerOneTurn = !playerOneTurn;  // Cambiar de jugador
        UpdatePlayerIndicator();
    }

    // Mostrar el modal de derrota
    void ShowLoseModal()
    {
        loseModal.SetActive(true);
    }

    // Actualizar el texto de vidas
    void UpdateLivesText()
    {
        livesText.text = "Vidas: " + playerLives;
    }

    // Actualizar el indicador de jugador (1 o 2)
    void UpdatePlayerIndicator()
    {
        playerIndicatorText.text = playerOneTurn ? "Jugador 1" : "Jugador 2";
    }

    // Funci�n para reiniciar el juego
    public void RestartGame()
    {
        StartGame(currentGameMode);
    }

    // Funci�n para ir al men� principal
    public void GoHome()
    {
        // L�gica para ir al men� principal
        // SceneManager.LoadScene("MainMenu");
    }
}
