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
    private float difficultyScale;

    [Header("Game manager params")]
    public UnityEvent<bool> OnGamePaused;
    [SerializeField] private Tutorial gameTutorial;
    [SerializeField] private HamburguerAssemblyHandler hamburguerAssembly;
    [SerializeField] private GameObject pauseMenu;
    [SerializeField] private GameObject gameOverScreen;
    [SerializeField] private GameObject gameUI;
    [SerializeField] private TMP_Text gameOverPoints;

    private bool isPaused;

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

    private void GenerateNewClient()
    {
        actualClient = Instantiate(clientPrefab);

        //llama la funcion para mover el capibara al puesto
    }

    public void ServeClient(Hamburguer hamburguerReceived)
    {
        if (actualClient == null) //Agregar comprobación aqui de tiene orden
            return;

        if (actualClient.ReceiveHamburguer(hamburguerReceived))
        {
            AudioManager.instance.Play("HappyCapibara");
            //Agregar funcion de retirar capibara
        }
        else
        {
            AudioManager.instance.Play("ImpatientCapibara");
        }
    }

    private void GameOver()
    {
        TogglePauseGame(false);
        gameOverScreen.SetActive(true);
        gameOverPoints.text = actualPoints.ToString("00000");
        AudioManager.instance.Play("Lose");
    }

    public void StartTutorial()
    {
        gameTutorial.StartTutorial();
    }

    public void StartGame()
    {
        //TODO
    }

    public void TogglePauseGame()
    {
        isPaused = !isPaused;
        OnGamePaused?.Invoke(isPaused);
        gameOverScreen.SetActive(isPaused);
        gameUI.SetActive(!isPaused);
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
