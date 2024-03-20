using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum IngredientType
{
    LETTUCE,
    TOMATOE,
    MEAT,
    BACON,
    PICKLES
}

public class Ingredient : MonoBehaviour
{
    public IngredientType ingredientType;
    public Vector3 anchorPosition;
}
