using UnityEngine;
using UnityEngine.UI;

public class UIContainer : CustomUIComponent
{
    public ViewSO viewData;
    public Style style;

    private Image image;

    private VerticalLayoutGroup verticalLayoutGroup;
    private HorizontalLayoutGroup horizontalLayoutGroup;

    public override void Setup()
    {
        if (viewData == null)
        {
            var parentView = GetComponentInParent<View>();
            if (parentView != null)
            {
                viewData = parentView.viewData;
            }
        }

        TryGetComponent(out verticalLayoutGroup);
        TryGetComponent(out horizontalLayoutGroup);
        image = GetComponent<Image>();
    }

    public override void Configure()
    {   
        if (verticalLayoutGroup != null)
        {
            verticalLayoutGroup.padding = viewData.padding;
            verticalLayoutGroup.spacing = viewData.spacing;
        }
        else if (horizontalLayoutGroup != null)
        {
            horizontalLayoutGroup.padding = viewData.padding;
            horizontalLayoutGroup.spacing = viewData.spacing;
        }
        
        if (image != null && viewData != null && viewData.theme != null)
        {
            image.color = viewData.theme.GetBackgroundColor(style);
        }
    }
}
