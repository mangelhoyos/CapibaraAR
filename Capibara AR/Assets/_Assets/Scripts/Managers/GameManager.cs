using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    [SerializeField] private Client clientPrefab;
    [SerializeField] public Transform tablePosition;

    private Client actualClient = null;
    public static GameManager Instance;

    private int actualPoints;
    private float difficultyScale = 1;

    [Header("Game manager params")]
    public UnityEvent<bool> OnGamePaused;
    [SerializeField] private Tutorial gameTutorial;
    [SerializeField] private HamburguerAssemblyHandler hamburguerAssembly;
    [SerializeField] private GameObject pauseMenu;
    [SerializeField] private GameObject gameOverScreen;
    [SerializeField] private GameObject gameUI;
    [SerializeField] private GameObject gamePauseButton;
    [SerializeField] private GameObject grabHandler;
    [SerializeField] private TMP_Text gameOverPoints;

    private bool isPaused;
    private bool isGameOver;

    private void Awake()
    {
        if(Instance != null)
        {
            Destroy(this);
            return;
        }
        Instance = this;
    }

    private void Start()
    {
        StartTutorial();
        AudioManager.instance.SetWithFade("Planning", 6, true);
    }

    [ContextMenu("Start Game")]
    private void GenerateNewClient()
    {
        actualClient = Instantiate(clientPrefab);
        actualClient.difficultyMultiplier = difficultyScale;
    }

    public void ServeClient(Hamburguer hamburguerReceived)
    {
        if (actualClient == null || !actualClient.hasOrder || isGameOver)
            return;

        if (actualClient.ReceiveHamburguer(hamburguerReceived))
        {
            AudioManager.instance.Play("HappyCapibara");
            difficultyScale += 0.15f;
            actualClient.ClientWellServed();
            actualPoints += 100;
            GenerateNewClient();
        }
        else
        {
            AudioManager.instance.Play("ImpatientCapibara");
        }
    }

    public void GameOver()
    {
        grabHandler.SetActive(false);
        TogglePauseGame(false);
        gameOverScreen.SetActive(true);
        gameOverPoints.text = actualPoints.ToString("00000");
        AudioManager.instance.Play("Lose");
    }

    public void StartTutorial()
    {
        gameTutorial.StartTutorial();
    }

    public void StartGame(GameObject placedObject)
    {
        tablePosition = placedObject.transform;
        hamburguerAssembly = placedObject.GetComponentInChildren<HamburguerAssemblyHandler>();
        GenerateNewClient();
    }

    public void TogglePauseGame()
    {
        if (isGameOver) return;

        isPaused = !isPaused;
        OnGamePaused?.Invoke(isPaused);
        gameOverScreen.SetActive(isPaused);
        gamePauseButton.SetActive(!isPaused);
        AudioManager.instance.Play("Button");
    }

    public void TogglePauseGame(bool state)
    {
        isPaused = state;
        OnGamePaused?.Invoke(isPaused);
        gameOverScreen.SetActive(isPaused);
        gameUI.SetActive(!isPaused);
        AudioManager.instance.Play("Button");
    }

    public void ResetGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
