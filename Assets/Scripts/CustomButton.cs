using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using TMPro;

public class CustomButton : CustomUIComponent
{
    public ThemeSO theme;
    public Style style;

    private Button button;
    TextMeshProUGUI buttonText;

    public override void Setup()
    {
        button = GetComponentInChildren<Button>();
        buttonText = GetComponentInChildren<TextMeshProUGUI>();
    }

    public override void Configure()
    {
        ColorBlock cb = button.colors;
        cb.normalColor = theme.GetButtonColor(style);
        button.colors = cb;

        if (buttonText != null)
        {
            buttonText.color = theme.GetTextColor(style);
        }
    }
}
