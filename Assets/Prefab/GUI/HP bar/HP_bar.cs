using UnityEngine;

public class HP_bar : MonoBehaviour
{
    public GameObject hp_icon;

    // Link to layout group of the content of the viewport, to be able to add or remove hp icons (HP bar > Viewport > Content)
    private GameObject hp_content;

    public int hp_max = 5;

    private int hp_current;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        hp_content = transform.GetChild(0).GetChild(0).gameObject;
        hp_current = hp_max;
        SetHP(hp_current);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetHP(int hp)
    {
        // Clear the current hp icons and add new ones according to the hp value
        foreach (Transform child in hp_content.transform)
        {
            Destroy(child.gameObject);
        }

        hp_current = hp;

        for (int i = 0; i < hp_max; i++)
        {
            if (i < hp_current)
            {
                Instantiate(hp_icon, hp_content.transform);
            }
        }
    }

    public void changeHP(int hp)
    {
        SetHP(hp_current + hp);
    }
    
    public int GetHP()
    {
        return hp_current;
    }

}
