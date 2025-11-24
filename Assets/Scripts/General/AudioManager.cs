using UnityEngine;
using TMPro;
using UnityEngine.UI;
public class AudioManager : MonoBehaviour
{
    [SerializeField] private SettingSO audioSettings;

    [Header("Slider Music")]
    [SerializeField] private Slider sliderMaster;
    [SerializeField] private Slider sliderMusic;
    [SerializeField] private Slider sliderSFX;

    [Header("Text Music")]
    [SerializeField] private TMP_Text textMaster;
    [SerializeField] private TMP_Text textMusic;
    [SerializeField] private TMP_Text textSFX;
    private void Reset()
    {
        gameObject.name="AudioManager";
    }

    private void Awake()
    {
        audioSettings.LoadVolumes();
    }
    private void OnDestroy()
    {
        audioSettings.SaveVolumes();
    }
    private void Start()
    {

        sliderMaster.value = audioSettings.GetMasterVolume();
        sliderMaster.onValueChanged.AddListener(UpdateMasterVolume);

        sliderMusic.value = audioSettings.GetMusicVolume();
        sliderMusic.onValueChanged.AddListener(UpdateMusicVolume);

        sliderSFX.value = audioSettings.GetSFXVolume();
        sliderSFX.onValueChanged.AddListener(UpdateSFXVolume);

        UpdateMasterVolume(sliderMaster.value);
        UpdateMusicVolume(sliderMusic.value);
        UpdateSFXVolume(sliderSFX.value);

    }
    private void UpdateMasterVolume(float value)
    {
        textMaster.text = Mathf.RoundToInt(value * 100).ToString();
        audioSettings.SetMasterVolume(value);
    }

    private void UpdateMusicVolume(float value)
    {
        textMusic.text = Mathf.RoundToInt(value * 100).ToString();
        audioSettings.SetMusicVolume(value);
    }

    private void UpdateSFXVolume(float value)
    {
        textSFX.text = Mathf.RoundToInt(value * 100).ToString();
        audioSettings.SetSFXVolume(value);
    }

}