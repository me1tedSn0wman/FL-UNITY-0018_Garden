using TMPro;
using UnityEngine;
using UnityEngine.UI;
using YG;

public class SettingsUI : WindowUI
{
    [Header("Settings UI")]
    [SerializeField] private InfoUI infoUI;

    [Header("Sliders")]
    [SerializeField] private Slider slider_SoundVolume;
    [SerializeField] private Slider slider_MusicVolume;
    
    [Header("Textes")]
    [SerializeField] private TextMeshProUGUI text_SoundVolume;
    [SerializeField] private TextMeshProUGUI text_MusicVolume;

    /*
    [Header("Buttons")]
    [SerializeField] private Button button_Info;
    */

    public override void Awake()
    {
        base.Awake();

        slider_SoundVolume.onValueChanged.AddListener((value) =>
        {
            AudioControlManager.soundVolume = value;
            text_SoundVolume.text = ((int)(value * 100)).ToString();
            GameManager.Instance.soundLibrary.PlayOneShoot("clickUI");
        });

        slider_MusicVolume.onValueChanged.AddListener((value) =>
        {
            AudioControlManager.musicVolume = value;
            text_MusicVolume.text = ((int)(value * 100)).ToString();
            GameManager.Instance.soundLibrary.PlayOneShoot("clickUI");
        });

        /*

        button_Info.onClick.AddListener(() =>
        {
            GameManager.Instance.soundLibrary.PlayOneShoot("clickUI");
            infoUI.SetActive(true);
        });

        */

        /*
        infoUI.SetActive(false);
        */
        UpdateInitValues();
    }

    public void UpdateInitValues() {
        float tSoundVol = AudioControlManager.soundVolume;
        float tMusicVol = AudioControlManager.musicVolume;

        slider_SoundVolume.SetValueWithoutNotify(tSoundVol);
        slider_MusicVolume.SetValueWithoutNotify(tMusicVol);

        text_SoundVolume.text = ((int)(tSoundVol * 100)).ToString();
        text_MusicVolume.text = ((int)(tMusicVol * 100)).ToString();
    }
}
