using System;
using UnityEngine;
using UnityEngine.UI;

public class View : CustomUIComponent
{
    public ViewSO viewData;

    public GameObject containerTop;
    public GameObject containerCenter;
    public GameObject containerBottom;

    private Image imageTop;
    private Image imageCenter;
    private Image imageBottom;

    private VerticalLayoutGroup verticalLayoutGroup;

    public override void Setup()
    {
        TryGetComponent(out verticalLayoutGroup);
        imageTop = containerTop != null ? containerTop.GetComponent<Image>() : null;
        imageCenter = containerCenter != null ? containerCenter.GetComponent<Image>() : null;
        imageBottom = containerBottom != null ? containerBottom.GetComponent<Image>() : null;
    }

    public override void Configure()
    {
        // Skip
    }
}
