using System.Collections.Generic;
using UnityEngine;

public class HamburguerAssemblyHandler : MonoBehaviour
{
    private Hamburguer hamburguerInProcess; //Current hamburguer being built in the assembly

    [Header("Ingredients positioning")]
    [SerializeField] private Transform ingredientsAnchorPosition;
    [SerializeField] private float ingredientsOffsetIncrease;
    private float ingredientsOffSet = 0;

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

}
