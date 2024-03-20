using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrillHandler : MonoBehaviour, IDropZone
{
    List<GrillableIngredient> meatCooking = new List<GrillableIngredient>();
    Coroutine grillingCoroutine = null;

    bool isGrilling = false;

    [field: SerializeField]
    public bool IsRemovable { get; set; }

    public void AddGrillIngredient(GrillableIngredient meatToCook)
    {
        meatCooking.Add(meatToCook);

        if (grillingCoroutine != null)
            return;

        grillingCoroutine = StartCoroutine(GrillIngredientsCoroutine());
    }

    public void RemoveGrillIngredient(GrillableIngredient removeIngredient)
    {
        meatCooking.Remove(removeIngredient);
        if(meatCooking.Count == 0)
        {
            StopCoroutine(grillingCoroutine);
            grillingCoroutine = null;
        }
    }

    private void GrillPause(bool state)
    {
        isGrilling = state;
    }

    private IEnumerator GrillIngredientsCoroutine()
    {
        if(isGrilling)
        {
            foreach(GrillableIngredient ingredient in meatCooking)
            {
                ingredient.CookMeat();
            }
        }
        yield return null;
        grillingCoroutine = StartCoroutine(GrillIngredientsCoroutine());
    }

    public void ItemReceived()
    {
        //TODO
    }

    public void RemoveItem()
    {
        //TODO
    }
}
