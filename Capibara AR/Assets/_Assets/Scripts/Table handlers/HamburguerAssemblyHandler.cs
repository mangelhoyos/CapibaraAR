using System.Collections.Generic;
using UnityEngine;

public class HamburguerAssemblyHandler : MonoBehaviour, IDropZone
{
    private Hamburguer hamburguerInProcess; //Current hamburguer being built in the assembly

    [Header("Ingredients positioning")]
    [SerializeField] private Transform ingredientsAnchorPosition;
    [SerializeField] private float ingredientsOffsetIncrease;
    private float ingredientsOffSet = 0;

    [field: SerializeField] //Allows properties to be serialized
    public bool IsRemovable { get; set; }

    public void AddIngredient(Ingredient ingredient)
    {
        hamburguerInProcess.AddIngredientToHamburguer(ingredient);
        Vector3 offset = Vector3.up * ingredientsOffSet;
        ingredient.transform.position = ingredientsAnchorPosition.position + offset;
        ingredientsOffSet += ingredientsOffsetIncrease;
    }

    public void DeliverHamburguer()
    {
        //TODO
    }
    
    public void ResetAssembly()
    {
        hamburguerInProcess = new Hamburguer();
        ingredientsOffSet = 0;
    }

    public void ItemReceived(IGrabbable grabbableReceived)
    {
        AddIngredient(grabbableReceived as Ingredient);
    }

    public void RemoveItem(IGrabbable grabbableRemoved)
    {
        throw new System.Exception("Should not have a remove item");
    }

}
