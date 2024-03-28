using Lean.Touch;
using UnityEngine;

/// <summary>
/// Each element that should be grabbed must have a LeanDragTranslate to function and a Collider
/// due to the script using raycast to detect this objects. This script HIGHLY depends on LeanTouch
/// </summary>
public class GrabHandler : MonoBehaviour
{
    [SerializeField] private LayerMask grabbableMask;
    [SerializeField] private LayerMask dropzoneMask;

    private LeanDragTranslate actualGrabbedItem;
    private Camera mainCamera;

    private void Start()
    {
        mainCamera = Camera.main;
    }

    private void OnEnable()
    {
        LeanTouch.OnFingerDown += TryGrabItem;
        LeanTouch.OnFingerUp += ReleaseItem;  
    }

    private void OnDisable()
    {
        LeanTouch.OnFingerDown -= TryGrabItem;
        LeanTouch.OnFingerUp -= ReleaseItem;
    }

    private void TryGrabItem(LeanFinger touchData)
    {
        Ray cameraCastPosition = mainCamera.ScreenPointToRay(touchData.ScreenPosition);

        if(Physics.Raycast(cameraCastPosition, out RaycastHit hitInfo, 30f, grabbableMask))
        {
            if(hitInfo.transform.TryGetComponent(out LeanDragTranslate translateComponent))
            {
                IGrabbable grabbableItem = translateComponent.GetComponent<IGrabbable>();
                if (!grabbableItem.ActualDropzone.IsRemovable)
                    return;

                translateComponent.enabled = true;
                actualGrabbedItem = translateComponent;
                
                grabbableItem.ActualDropzone.RemoveItem(grabbableItem);
            }
        }
    }

    private void ReleaseItem(LeanFinger touchData)
    {
        if (LeanTouch.Fingers.Count > 1)
            return;

        if (actualGrabbedItem != null)
        {
            actualGrabbedItem.enabled = false;

            bool hitDropZone = false;
            float radius = 0.2f;
            Vector3 origin = actualGrabbedItem.transform.position;
            Debug.DrawRay(actualGrabbedItem.transform.position, actualGrabbedItem.transform.up * 3, Color.red, 5);
            Collider[] dropZoneList = Physics.OverlapSphere(origin, radius, dropzoneMask);
            if (dropZoneList.Length > 0)
            {
                IDropZone dropZone = dropZoneList[0].GetComponent<IDropZone>();
                if (dropZone != null)
                {
                    IGrabbable grabbableItem = actualGrabbedItem.GetComponent<IGrabbable>();
                    if (grabbableItem.AcceptedDropZones().Contains(dropZone))
                    {
                        hitDropZone = true;
                        dropZone.ItemReceived(grabbableItem);
                    }
                }
            }

            if (!hitDropZone)
            {   
                // Código para cuando no se encuentra un IDropZone adecuado
                IGrabbable grabbableItem = actualGrabbedItem.GetComponent<IGrabbable>();
                (grabbableItem as MonoBehaviour).transform.position = grabbableItem.ReturnAnchor;
                grabbableItem.ActualDropzone.ItemReceived(grabbableItem);
            }

            actualGrabbedItem = null;

        }
    }
}
