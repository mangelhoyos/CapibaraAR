using System.Collections.Generic;
using UnityEngine;

public class HamburguerAssemblyHandler : MonoBehaviour, IDropZone
{
    private Hamburguer hamburguerInProcess = new Hamburguer(); //Current hamburguer being built in the assembly

    [Header("VFX")]
    [SerializeField] private ParticleSystem vanishPS;

    [Header("Ingredients positioning")]
    [SerializeField] private Transform ingredientsAnchorPosition;
    [SerializeField] private float ingredientsOffsetIncrease;
    private float ingredientsOffSet = 0;

    private int numberOfIngredients = 0;
    private const int MAXNUMBEROFINGREDIENTS = 6;

    [field: SerializeField] //Allows properties to be serialized
    public bool IsRemovable { get; set; }

    public void AddIngredient(Ingredient ingredient)
    {
        hamburguerInProcess.AddIngredientToHamburguer(ingredient);
        AudioManager.instance.Play("Assemble");
        Vector3 offset = Vector3.up * ingredientsOffSet;
        ingredient.transform.position = ingredientsAnchorPosition.position + offset;
        ingredientsOffSet += ingredientsOffsetIncrease;
        numberOfIngredients++;
    }

    public void DeliverHamburguer()
    {
        GameManager.Instance.ServeClient(hamburguerInProcess);
        ResetAssembly();
    }
    
    public void ResetAssembly()
    {
        foreach(Ingredient ingredient in hamburguerInProcess.ingredientList) 
        {
            Destroy(ingredient.gameObject);
        }
        AudioManager.instance.Play("Thrash");
        hamburguerInProcess = new Hamburguer();
        ingredientsOffSet = 0;
        numberOfIngredients = 0;
        vanishPS.Play();
    }

    public void ItemReceived(IGrabbable grabbableReceived)
    {
        if(numberOfIngredients == MAXNUMBEROFINGREDIENTS)
        {
            grabbableReceived.ActualDropzone.ItemReceived(grabbableReceived);
            (grabbableReceived as MonoBehaviour).transform.position = grabbableReceived.ReturnAnchor;
            return;
        }

        grabbableReceived.ActualDropzone = this;

        AddIngredient((grabbableReceived as MonoBehaviour).GetComponent<Ingredient>());
    }

    public void RemoveItem(IGrabbable grabbableRemoved)
    {
        throw new System.Exception("Should not have a remove item");
    }

}
