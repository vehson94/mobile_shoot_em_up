using UnityEngine;
using TMPro;

[CreateAssetMenu(fileName = "TextSO", menuName = "CustomUI/TextSO")]
public class TextSO : ScriptableObject
{
    public ThemeSO theme;

    public TMP_FontAsset font;
    public int size;

    public void OnValidate() 
    {
        var texts = Resources.FindObjectsOfTypeAll<Text>();

        foreach (var text in texts)
        {
            if (text == null || text.textData != this)
            {
                continue;
            }

            text.OnValidate();
        }
    }
}
