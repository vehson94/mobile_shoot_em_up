using UnityEngine;

[CreateAssetMenu(fileName = "ViewSO", menuName = "CustomUI/ViewSO")]
public class ViewSO : ScriptableObject
{
    public ThemeSO theme;
	public RectOffset padding;
	public float spacing;

    public void OnValidate()
	{
		var views = Resources.FindObjectsOfTypeAll<View>();

		foreach (var view in views)
		{
			if (view == null || view.viewData != this)
			{
				continue;
			}

			view.Init();
		}
	}
}
