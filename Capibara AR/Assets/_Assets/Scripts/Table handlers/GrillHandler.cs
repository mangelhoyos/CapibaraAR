using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrillHandler : MonoBehaviour, IDropZone
{
    List<GrillableIngredient> meatCooking = new List<GrillableIngredient>();
    Coroutine grillingCoroutine = null;

    bool isGrilling = true;

    [SerializeField] private GrillSpaceData[] grillabeIngredientsPositions = new GrillSpaceData[4];

    [field: SerializeField]
    public bool IsRemovable { get; set; }

    public void AddGrillIngredient(GrillableIngredient meatToCook)
    {
        meatCooking.Add(meatToCook);
        AudioManager.instance.Play("Assemble");

        if (grillingCoroutine != null)
            return;

        grillingCoroutine = StartCoroutine(GrillIngredientsCoroutine());
    }

    public void RemoveGrillIngredient(GrillableIngredient removeIngredient)
    {
        if (!meatCooking.Find(x => x == removeIngredient))
            return;

        AudioManager.instance.SetWithFade("MeatFrying", 0.5f, false);

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

    public void ItemReceived(IGrabbable grabbableReceived)
    {
        GrillSpaceData availableSpaceData = null;

        foreach(GrillSpaceData spaceData in grillabeIngredientsPositions)
        {
            if(!spaceData.isOccupied)
            {
                availableSpaceData = spaceData;
                break;
            }
        }

        Transform grillableIngredient = (grabbableReceived as MonoBehaviour).transform;

        if (availableSpaceData == null) 
        {
            grillableIngredient.position = grabbableReceived.ReturnAnchor;
            grabbableReceived.ActualDropzone.ItemReceived(grabbableReceived);
            return;
        }

        grillableIngredient = (grabbableReceived as MonoBehaviour).transform;
        grillableIngredient.position = availableSpaceData.grillPosition.position;

        grabbableReceived.ActualDropzone = this;
        grabbableReceived.ReturnAnchor = availableSpaceData.grillPosition.position;

        availableSpaceData.indexedIngredient = (grabbableReceived as MonoBehaviour).GetComponent<GrillableIngredient>();
        availableSpaceData.isOccupied = true;
        availableSpaceData.smokeParticles.Play();

        AddGrillIngredient((grabbableReceived as MonoBehaviour).GetComponent<GrillableIngredient>());
    }

    public void RemoveItem(IGrabbable grabbableRemoved)
    {
        GrillSpaceData foundPositionForIngredient = null;

        foreach(GrillSpaceData grillSpaceData in grillabeIngredientsPositions)
        {
            if (grillSpaceData.indexedIngredient == (grabbableRemoved as MonoBehaviour).GetComponent<GrillableIngredient>())
            {
                foundPositionForIngredient = grillSpaceData; 
                break;
            }
        }

        foundPositionForIngredient.isOccupied = false;
        foundPositionForIngredient.indexedIngredient = null;
        foundPositionForIngredient.smokeParticles.Stop();

        RemoveGrillIngredient((grabbableRemoved as MonoBehaviour).GetComponent<GrillableIngredient>());
    }
}

[System.Serializable]
public class GrillSpaceData
{
    public bool isOccupied = false;
    public Transform grillPosition;
    public ParticleSystem smokeParticles;
    [HideInInspector] public GrillableIngredient indexedIngredient = null;
}
