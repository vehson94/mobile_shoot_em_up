using UnityEngine;
using TMPro;
#if UNITY_EDITOR
using UnityEditor;
#endif

public class Text : CustomUIComponent
{
    public TextSO textData;
    public Style style;

    private TextMeshProUGUI textMeshPro;

    public override void Setup()
    {
    #if UNITY_EDITOR
            ScheduleRectTransformApply();
    #endif
    }

    public override void Configure()
    {
        RefreshText();
    }

    private void RefreshText()
    {
        textMeshPro = GetComponentInChildren<TextMeshProUGUI>();

        if (textMeshPro == null || textData == null || textData.theme == null)
        {
            return;
        }

        textMeshPro.font = textData.font;
        textMeshPro.fontSize = textData.size;
        textMeshPro.color = textData.theme.GetTextColor(style);
    }

#if UNITY_EDITOR
    private void ScheduleRectTransformApply()
    {
        if (Application.isPlaying)
        {
            return;
        }

        ApplyDefaultRectTransformOffsets();

        EditorApplication.delayCall -= DelayedApplyDefaultRectTransformOffsets;
        EditorApplication.delayCall += DelayedApplyDefaultRectTransformOffsets;
    }

    private void DelayedApplyDefaultRectTransformOffsets()
    {
        if (this == null || Application.isPlaying)
        {
            return;
        }

        ApplyDefaultRectTransformOffsets();
    }
#endif

    private void ApplyDefaultRectTransformOffsets()
    {
        RectTransform rectTransform = transform as RectTransform;

        if (rectTransform == null)
        {
            return;
        }

        rectTransform.anchorMin = Vector2.zero;
        rectTransform.anchorMax = Vector2.one;
        rectTransform.offsetMin = Vector2.zero;
        rectTransform.offsetMax = Vector2.zero;
    }
}
