using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Client : MonoBehaviour
{
    [SerializeField] private IngredientsList_SO ingredientOptions;

    private Hamburguer desiredHamburguer = null;

    //Callbacks
    public Action OnClientServed;
    public Action OnClientFailed;

    [HideInInspector] public float difficultyMultiplier = 0;
    private float timeLeftForOrder;

    private const float INITIALTIMELEFT = 20;
    private const float TIMEREDUCTION = 0.1f;

    bool isPaused;

    [ContextMenu("Generate Order")]
    public void GenerateOrder()
    {
        desiredHamburguer = new();

        int desiredIngredients = UnityEngine.Random.Range(3, 6); //Change number for SerializeField variables after test

        for(int i = 0; i < desiredIngredients; i++)
        {
            int randomIngredient = UnityEngine.Random.Range(0, ingredientOptions.ingredients.Count);

            while (CountIngredientsInList(desiredHamburguer.ingredientList, ingredientOptions.ingredients[randomIngredient]) >= 2)
            {
                randomIngredient = UnityEngine.Random.Range(0, ingredientOptions.ingredients.Count);
            }

            desiredHamburguer.AddIngredientToHamburguer(ingredientOptions.ingredients[randomIngredient]);

            Debug.Log("El ingrediente es: " + desiredHamburguer.ingredientList[i].ingredientType);
        }

    }

    private int CountIngredientsInList(List<Ingredient> list, Ingredient ingredient)
    {
        int count = 0;

        foreach (Ingredient item in list)
        {
            if (item.Equals(ingredient))
            {
                count++;
            }
        }

        return count;
    }

    private void InitializeTimer()
    {

    }

    private void UpdateTimer()
    {
        if(!isPaused)
        {
            timeLeftForOrder -= TIMEREDUCTION * difficultyMultiplier * Time.deltaTime;
            //TODO Clock animation
        }
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
