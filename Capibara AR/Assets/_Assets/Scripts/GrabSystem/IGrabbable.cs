using System.Collections.Generic;
using UnityEngine;

public interface IGrabbable
{
    public IDropZone ActualDropzone { get; set; }
    public Vector3 ReturnAnchor { get; set; }
    public List<IDropZone> acceptedDropZones { get; set; }
}
