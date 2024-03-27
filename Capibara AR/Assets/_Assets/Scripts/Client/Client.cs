using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Client : MonoBehaviour
{

    private Hamburguer desiredHamburguer = null;

    //Callbacks
    public Action OnClientServed;
    public Action OnClientFailed;

    [HideInInspector] public float difficultyMultiplier = 0;
    private float timeLeftForOrder;

    private const float INITIALTIMELEFT = 20;
    private const float TIMEREDUCTION = 0.1f;

    bool isPaused;
    bool isTimerStoped;

    [ContextMenu("Generate Order")]
    public void GenerateOrder()
    {
        desiredHamburguer = new();

        int desiredIngredients = UnityEngine.Random.Range(3, 6); //Change number for SerializeField variables after test

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

            Debug.Log("El ingrediente es: " + desiredHamburguer.ingredientList[i].ingredientType);
        }

        //Initialize Timer here InitializeTimer();
    }

    private void InitializeTimer()
    {
        timeLeftForOrder = INITIALTIMELEFT;
    }

    private void UpdateTimer()
    {
        if(!isPaused && !isTimerStoped)
        {
            timeLeftForOrder -= TIMEREDUCTION * difficultyMultiplier * Time.deltaTime;
            //TODO Clock animation
        }
    }

    public void ClientWellServed()
    {
        isTimerStoped = true;
        //Disable order message
        //Move client to exit
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

            desiredCounts[ingredient.ingredientType]--;
        }

        return desiredCounts.Values.All(count => count == 0);
    }

    public void ClientPause(bool state)
    {
        isPaused = state;
    }
}
