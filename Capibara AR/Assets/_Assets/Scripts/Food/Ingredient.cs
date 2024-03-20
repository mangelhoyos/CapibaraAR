using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public enum IngredientType
{
    LETTUCE,
    TOMATOE,
    MEAT,
    BACON,
    PICKLES
}

public class Ingredient : MonoBehaviour, IGrabbable
{
    public IngredientType ingredientType;
    public Vector3 anchorPosition;

    public IDropZone ActualDropzone { get; set; }
    public Vector3 ReturnAnchor { get; set; }

    [Header("Drop zone config")]
    [SerializeField] private List<Transform> acceptedDropzones;

    public List<IDropZone> AcceptedDropZones()
    {
        List<IDropZone> dropZoneQuery = acceptedDropzones
                            .Select(x => x.GetComponent<IDropZone>())
                            .Where(x => x != null)
                            .ToList();

        return dropZoneQuery;
    }
}
