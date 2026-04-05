using UnityEngine;

public abstract class CustomUIComponent : MonoBehaviour
{
    private void Awake()
    {
        Init();
    }

    public abstract void Setup();
    public abstract void Configure();

    public void Init()
    {
        Setup();
        Configure();
    }

    public void OnValidate()
    {
        Init();
    }
}
