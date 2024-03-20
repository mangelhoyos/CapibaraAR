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
    private float grillValue;
    [SerializeField] private Texture2D[] meatTextures;
    [SerializeField] private MeshRenderer meatMeshRenderer;

    private const float GRILLINCREMENTVALUE = 0.3f;
    private const float COOKTHRESHOLD = 4f;
    private const float BURNTTHRESHHOLD = COOKTHRESHOLD * 2;

    public void CookMeat()
    {
        grillValue += GRILLINCREMENTVALUE * Time.deltaTime;
        if(grillValue >= BURNTTHRESHHOLD)
        {
            //TODO Burn action
        }
    }

    public GrillIngredientState GetGrillableIngredient()
    {
        GrillIngredientState actualFoodState = grillValue < COOKTHRESHOLD ? GrillIngredientState.RAW : GrillIngredientState.COOKED;
        return actualFoodState;
    }
}
