using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hamburguer : MonoBehaviour
{
    public List<Ingredient> ingredientList = new List<Ingredient>();
    
    public void AddIngredientToHamburguer(Ingredient ingredientToAdd)
    {
        ingredientList.Add(ingredientToAdd);
    }
}
