using System;
using System.Collections;
using System.Linq;
using UnityEngine;

public class Client : MonoBehaviour
{
    [SerializeField] private OrderMessage orderMessage;
    [SerializeField] private ClientMovement clientMovement;

    private Hamburguer desiredHamburguer = null;

    //Callbacks
    public Action OnClientServed;
    public Action OnClientFailed;

    [HideInInspector] public float difficultyMultiplier = 0;
    public float timeLeftForOrder;

    private const float INITIALTIMELEFT = 20;
    private const float TIMEREDUCTION = 0.3f;

    bool isPaused;
    bool isTimerStoped;
    bool alertGiven = false;

    public bool hasOrder;

    private void Start()
    {
        GameManager.Instance.OnGamePaused.AddListener(ClientPause);
    }

    private void Update()
    {
        UpdateTimer();
    }

    public void GenerateOrder()
    {
        desiredHamburguer = new();

        int desiredIngredients = UnityEngine.Random.Range(3, 6);

        for(int i = 0; i < desiredIngredients; i++)
        {
            
            Ingredient ingredientChoosed = new();

            do
            {
                int randomIngredient = UnityEngine.Random.Range(0, Enum.GetNames(typeof(IngredientType)).Length);
                ingredientChoosed.ingredientType = (IngredientType)randomIngredient;
            }
            while (desiredHamburguer.ingredientList.Where(x => x.ingredientType == ingredientChoosed.ingredientType).ToList().Count >= 2);

            desiredHamburguer.AddIngredientToHamburguer(ingredientChoosed);
        }

        StartCoroutine(RequestOrderRoutine());
    }

    private IEnumerator RequestOrderRoutine()
    {
        InitializeTimer();
        AudioManager.instance.Play("CapibaraTalking");
        orderMessage.ShowMessage(desiredHamburguer);
        yield return new WaitForSeconds(0.5f);
        hasOrder = true;
    }

    private void InitializeTimer()
    {
        timeLeftForOrder = INITIALTIMELEFT;
        orderMessage.UpdateOrderTimer(timeLeftForOrder / INITIALTIMELEFT);
    }

    private void UpdateTimer()
    {
        if(hasOrder && !isTimerStoped && !isPaused)
        {
            timeLeftForOrder -= TIMEREDUCTION * difficultyMultiplier * Time.deltaTime;
            orderMessage.UpdateOrderTimer(timeLeftForOrder / INITIALTIMELEFT);

            if(!alertGiven && timeLeftForOrder <= INITIALTIMELEFT / 2)
            {
                AudioManager.instance.Play("ImpatientCapibara");
                alertGiven = true;
            }

            if(timeLeftForOrder <= 0)
            {
                GameManager.Instance.GameOver();
                isTimerStoped = true;
            }
        }
    }

    public void ClientWellServed()
    {
        isTimerStoped = true;
        orderMessage.HideMessage();
        clientMovement.MoveClientToExitPoint();
    }

    public bool ReceiveHamburguer(Hamburguer cookedHamburguer)
    {
        if (desiredHamburguer.ingredientList.Count != cookedHamburguer.ingredientList.Count)
            return false;

        var desiredCounts = desiredHamburguer.ingredientList
            .GroupBy(ingredient => ingredient.ingredientType)
            .ToDictionary(group => group.Key, group => group.Count());

        foreach (var ingredient in cookedHamburguer.ingredientList)
        {
            if (!desiredCounts.ContainsKey(ingredient.ingredientType) || desiredCounts[ingredient.ingredientType] == 0)
                return false;

            if (ingredient.TryGetComponent(out GrillableIngredient grillIngredient))
            {
                if (!grillIngredient.isCooked || grillIngredient.isBurnt)
                    return false;
            }

            desiredCounts[ingredient.ingredientType]--;
        }

        return desiredCounts.Values.All(count => count == 0);
    }

    public void ClientPause(bool state)
    {
        isPaused = state;
    }
}
