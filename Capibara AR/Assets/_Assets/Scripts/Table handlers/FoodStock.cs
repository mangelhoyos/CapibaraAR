using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FoodStock : MonoBehaviour, IDropZone
{
    [SerializeField] private Ingredient ingredientPrefab;
    [SerializeField] private Transform initialPosition;
    [field: SerializeField]
    public bool IsRemovable { get; set; }

    private void Start()
    {
        GenerateIngredient();
    }

    public void ItemReceived(IGrabbable grabbableReceived)
    {
        Debug.Log("Destroy");
        if (grabbableReceived as Object)
        {
            
            Destroy(grabbableReceived as Object);
        }
    }

    public void RemoveItem(IGrabbable grabbableRemoved)
    {
        GenerateIngredient();
    }

    private void GenerateIngredient()
    {
        Ingredient instantiatedIngredient = Instantiate(ingredientPrefab, initialPosition.position, Quaternion.identity, transform);
        instantiatedIngredient.gameObject.SetActive(true);
        IGrabbable ingredientGrabbable = instantiatedIngredient.GetComponent<IGrabbable>();
        ingredientGrabbable.ActualDropzone = this;
        ingredientGrabbable.ReturnAnchor = initialPosition.position;
    }
}
