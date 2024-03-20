using UnityEngine;
using UnityEngine.Events;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    private int actualPoints;
    private float difficultyScale;

    [Header("Game manager params")]
    public UnityEvent<bool> OnGamePaused;
    [SerializeField] private Tutorial gameTutorial;

    private void Awake()
    {
        if(Instance != null)
        {
            Destroy(this);
            return;
        }
        Instance = this;
    }

    private void GenerateNewClient()
    {
        Client newClient = new Client();
        //TODO Client spawn visuals
    }

    private void ClientServedCorrectly()
    {
        //Animations win
        GenerateNewClient();
    }

    private void GameOver()
    {
        //TODO
    }

    public void StartTutorial()
    {
        gameTutorial.StartTutorial();
    }

    public void StartGame()
    {
        //TODO
    }

}
