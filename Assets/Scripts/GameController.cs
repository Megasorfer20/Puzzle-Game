using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Video;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour
{
    public enum GameMode { SinglePlayer, TwoPlayers }
    public GameMode currentGameMode;

    [Header("UI Elements")]
    public GameObject readyModal;          // Modal de "Listo"
    public GameObject winModal;            // Modal de victoria
    public GameObject loseModal;           // Modal de derrota
    public GameObject nextPlayerModal;     // Modal de siguiente jugador (dos jugadores)
    public GameObject finalResultModal;       // Modal de resultado final (dos jugadores)
    public TMP_Text timerText;             // Temporizador en la UI
    public TMP_Text playerIndicatorText;   // Indicador de Jugador 1 o 2
    public TMP_Text livesText;             // Texto para mostrar las vidas restantes (solo un jugador)
    public TMP_Text finalResultText;          // Texto que muestra el resultado final (ganador o empate)



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
    private int playerLives = 2;

    private bool playerWin = false;
    private bool player1Win = false;
    private bool player2Win = false;

    [SerializeField] private UnityEvent eventosCall1, eventosCall2;

    void Start()
    {
        // Asignar funciones a los botones
        singlePlayerButton.onClick.AddListener(() => StartGame(GameMode.SinglePlayer));
        twoPlayerButton.onClick.AddListener(() => StartGame(GameMode.TwoPlayers));

        // Asegúrate de asignar el botón de "Listo"
        readyButton.onClick.AddListener(StartTimer);
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

        // Reiniciar el temporizador a 30 segundos
        timeRemaining = 30f;
        timerText.text = timeRemaining.ToString("F0") + "s";  // Actualizar el texto del cronómetro
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
        timeRemaining = 30f;
        timerIsRunning = true;
        readyModal.SetActive(false);
        gameOver = false;
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

        if (didWin)
        {
            ShowWinModal();
            gameOver = true;  // El juego termina si se gana
        }
        else
        {
            if (currentGameMode == GameMode.SinglePlayer && playerLives > 0)
            {
                playerLives--;
                UpdateLivesText();
                ShowReadyModal();  // Si aún hay vidas, reintentar y mostrar el modal
                gameOver = false;  // El juego no ha terminado, hay más vidas
            }
            else if (currentGameMode == GameMode.SinglePlayer && playerLives == 0)
            {
                ShowLoseModal();  // Mostrar modal de derrota
                gameOver = true;  // El juego ha terminado
            }
            else if (currentGameMode == GameMode.TwoPlayers)
            {
                if (!playerOneTurn)  // Si el turno fue del jugador 2
                {
                    ShowFinalResultModal();  // Mostrar resultado final después del turno del jugador 2
                    gameOver = true;  // El juego ha terminado
                }
                else
                {
                    ShowNextPlayerModal();  // Mostrar el modal de "Siguiente Jugador"
                    gameOver = false;  // El juego no ha terminado, es el turno del siguiente jugador
                }
            }
            else
            {
                ShowLoseModal();  // Si no hay vidas o jugadores restantes, mostrar derrota
                gameOver = true;  // Ahora sí, el juego ha terminado
            }
        }
    }

    void ShowFinalResultModal()
    {
        // Lógica para determinar quién ganó
        if (player1Win && player2Win)
        {
            finalResultText.text = "¡Empate!";
        }
        else if (player1Win)  // Ejemplo de lógica, puedes ajustarlo
        {
            finalResultText.text = "¡Jugador 1 Gana!";
        }
        else if (player2Win)
        {
            finalResultText.text = "¡Jugador 2 Gana!";
        }
        else
        {
            finalResultText.text = "¡Empate!";
        }

        finalResultModal.SetActive(true);  // Mostrar modal de resultado final
    }

    // Mostrar el modal de siguiente jugador (dos jugadores)
    void ShowNextPlayerModal()
    {
        nextPlayerModal.SetActive(true);
        playerOneTurn = !playerOneTurn;  // Cambiar de jugador
        UpdatePlayerIndicator();

        // Cerrar el modal de siguiente jugador antes de reiniciar el turno
        StartCoroutine(CloseNextPlayerModalAndRestart());
    }

    // Corrutina para esperar un breve momento antes de reiniciar el turno
    IEnumerator CloseNextPlayerModalAndRestart()
    {
        yield return new WaitForSeconds(2); // Esperar 2 segundos antes de cerrar el modal
        nextPlayerModal.SetActive(false);  // Cerrar el modal
        ShowReadyModal();  // Mostrar el modal de "Listo" para el nuevo turno
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


    public void ReloadGame()
    {
        // Obtener el nombre de la escena actual
        string currentSceneName = SceneManager.GetActiveScene().name;

        // Recargar la escena actual
        SceneManager.LoadScene(currentSceneName);
    }

    // Funci�n para ir al men� principal
    public void GoHome()
    {
        homeButton.onClick.AddListener(ReloadGame);
    }
}
