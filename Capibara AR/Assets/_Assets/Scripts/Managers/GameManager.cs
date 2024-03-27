using UnityEngine;
using UnityEngine.Events;

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
