using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum GrillIngredientState
{
    RAW,
    COOKED
}

public class GrillableIngredient : Ingredient
{
    [SerializeField] private float grillValue;
    [SerializeField] private Texture2D[] meatTextures;
    [SerializeField] private MeshRenderer meatMeshRenderer;

    public void CookMeat()
    {

    }

    public GrillIngredientState GetGrillableIngredient()
    {
        return  GrillIngredientState.COOKED; //default result
    }
}
