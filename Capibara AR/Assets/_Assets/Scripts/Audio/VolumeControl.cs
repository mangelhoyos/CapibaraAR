using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class VolumeControl : MonoBehaviour
{
    [SerializeField] private AudioMixer mainMixer;
    [SerializeField] private string parameter;
    [SerializeField] private Slider slider;

    private void OnEnable() => slider.onValueChanged.AddListener(ChangeValue);
    private void OnDisable() => slider.onValueChanged.RemoveListener(ChangeValue);

    private void Start() => slider.value = PlayerPrefs.GetFloat(parameter, 0.0f);

    private void ChangeValue(float value)
    {
        mainMixer.SetFloat(parameter, value);
        PlayerPrefs.SetFloat(parameter, value);
    }
}
