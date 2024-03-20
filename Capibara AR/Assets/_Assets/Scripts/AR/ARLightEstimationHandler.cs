using UnityEngine;
using UnityEngine.XR.ARFoundation;

/// <summary>
/// Script for handling light estimation with AR
/// </summary>
public class ARLightEstimationHandler : MonoBehaviour
{
    [Header("Light estimation setup")]
    [SerializeField] private ARCameraManager arCameraManager;
    [SerializeField] private Light mainDirectionalLight;

    private void OnEnable()
    {
        arCameraManager.frameReceived += LumenUpdate;
    }

    private void OnDisable()
    {
        arCameraManager.frameReceived -= LumenUpdate;
    }

    private void LumenUpdate(ARCameraFrameEventArgs eventData)
    {
        if(eventData.lightEstimation.averageBrightness.HasValue)
            mainDirectionalLight.intensity = eventData.lightEstimation.averageBrightness.Value;

        if(eventData.lightEstimation.colorCorrection.HasValue)
            mainDirectionalLight.color = eventData.lightEstimation.colorCorrection.Value;
    }
}
