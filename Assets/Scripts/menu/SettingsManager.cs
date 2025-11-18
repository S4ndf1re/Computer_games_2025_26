using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class SettingsManager : MonoBehaviour
{
    public GameObject settingsPanel;
    [SerializeField] private AudioMixer audioMixer;
    [SerializeField] private Slider masterSlider;

    private void Start()
    {   
        if (settingsPanel != null)
        {
            settingsPanel.SetActive(false);
            Time.timeScale = 1f;
        }

        if (masterSlider != null)
        {
            float currentVolume;
            audioMixer.GetFloat("ExposedMasterVolume", out currentVolume);
            masterSlider.value = Mathf.Pow(10, currentVolume / 20f);
        }

        masterSlider.onValueChanged.AddListener(SetMasterVolume);
    }

    public void SetMasterVolume(float value)
    {
        if (value <= 0.0001f)
            value = 0.0001f;
        Debug.Log("Volume changed to: " + Mathf.Log10(value) * 20);
        audioMixer.SetFloat("ExposedMasterVolume", Mathf.Log10(value) * 20);
    }
}
