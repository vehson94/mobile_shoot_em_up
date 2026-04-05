using UnityEngine;

[CreateAssetMenu(fileName = "ThemeSO", menuName = "CustomUI/ThemeSO")]
public class ThemeSO : ScriptableObject
{
    [Header("Primary")]
    public Color primary_bg;
    public Color primary_text;
    public Color primary_button;

    [Header("Secondary")]
    public Color secondary_bg;
    public Color secondary_text;
    public Color secondary_button;

    [Header("Tertiary")]
    public Color tertiary_bg;
    public Color tertiary_text;
    public Color tertiary_button;

    [Header("Other")]
    public Color disable;

    public Color GetTextColor(Style style)
    {
        return style switch
        {
            Style.Primary => primary_text,
            Style.Secondary => secondary_text,
            Style.Tertiary => tertiary_text,
            _ => primary_text
        };
    }

    public Color GetBackgroundColor(Style style)
    {
        return style switch
        {
            Style.Primary => primary_bg,
            Style.Secondary => secondary_bg,
            Style.Tertiary => tertiary_bg,
            _ => disable
        };
    }

    public Color GetButtonColor(Style style)
    {
        return style switch
        {
            Style.Primary => primary_button,
            Style.Secondary => secondary_button,
            Style.Tertiary => tertiary_button,
            _ => disable
        };
    }

    public void OnValidate()
	{   
        var UIcomponents = Resources.FindObjectsOfTypeAll<CustomUIComponent>();

		foreach (var component in UIcomponents)
		{
			if (component == null)
			{
				continue;
			}

			component.OnValidate();
		}
    }
}