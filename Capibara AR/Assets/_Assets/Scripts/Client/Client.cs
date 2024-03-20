using System;
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

    public void GenerateOrder()
    {

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
