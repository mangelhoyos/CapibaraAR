using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "IngredientList", menuName = "Ingredients List")]
public class IngredientsList_SO : ScriptableObject
{
    public List<Ingredient> ingredients = new List<Ingredient>();
}
