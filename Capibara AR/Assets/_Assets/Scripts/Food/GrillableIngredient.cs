using System.Collections;
using UnityEngine;

public enum GrillIngredientState
{
    RAW,
    COOKED
}

public class GrillableIngredient : Ingredient
{
    private float grillValue;

    [SerializeField] private Texture2D cookedTexture;
    [SerializeField] private MeshRenderer[] meatMeshRenderer;
    [SerializeField] private Texture2D burntTexture;

    private const float GRILLINCREMENTVALUE = 0.5f;
    private const float COOKTHRESHOLD = 4f;
    private const float BURNTTHRESHHOLD = COOKTHRESHOLD * 2;

    [HideInInspector] public bool isCooked = false;
    [HideInInspector] public bool isBurnt = false;

    public void CookMeat()
    {

        grillValue += GRILLINCREMENTVALUE * Time.deltaTime;
        if(!isCooked && grillValue >= COOKTHRESHOLD)
        {
            AudioManager.instance.Play("MeatFrying");
            isCooked = true;
            foreach(MeshRenderer meshRenderer in meatMeshRenderer)
            {
                meshRenderer.material.mainTexture = cookedTexture;
            }
        }
        else if(!isBurnt && grillValue >= BURNTTHRESHHOLD)
        {
            AudioManager.instance.Play("MeatBurnt");
            isBurnt = true;
            foreach (MeshRenderer meshRenderer in meatMeshRenderer)
            {
                meshRenderer.material.mainTexture = burntTexture;
            }
        }
    }


    public GrillIngredientState GetGrillableIngredient()
    {
        GrillIngredientState actualFoodState = grillValue < COOKTHRESHOLD ? GrillIngredientState.RAW : GrillIngredientState.COOKED;
        return actualFoodState;
    }
}
