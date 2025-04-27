using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Font_size : MonoBehaviour
{
    [SerializeField] private Slider slider;
    [SerializeField] private TextMeshProUGUI sliderText;

    private const string FontSizeKey = "FontSize";

    void Start()
    {
        float savedFontSize = PlayerPrefs.GetFloat(FontSizeKey, slider.minValue);
        slider.value = savedFontSize;

        OnSliderValueChanged(slider.value);

        slider.onValueChanged.AddListener(OnSliderValueChanged);
    }

    private void OnSliderValueChanged(float value)
    {
        if (sliderText != null)
        {
            sliderText.fontSize = value + 80;
        }

        PlayerPrefs.SetFloat(FontSizeKey, value);
        PlayerPrefs.Save();
    }
}
