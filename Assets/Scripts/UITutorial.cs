using UnityEngine;
using System.Collections;

public class UITutorial : MonoBehaviour
{
    public UIContainer containerLeft;
    public UIContainer containerRight;

    public float animationDuration = 0.5f;

    private bool leftPressed = false;  
    private bool rightPressed = false;
    private bool tutorialDeactivated = false;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        // Skip if tutorial is already deactivated
        if (tutorialDeactivated) return;

        // Check for touch input on left side
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);
            if (touch.phase == TouchPhase.Began || touch.phase == TouchPhase.Moved || touch.phase == TouchPhase.Stationary)
            {
                if (touch.position.x < Screen.width / 2f)
                {
                    leftPressed = true;
                }
                else
                {
                    rightPressed = true;
                }
            }
        }

        // Check for mouse input (for testing)
        if (Input.GetMouseButton(0))
        {
            if (Input.mousePosition.x < Screen.width / 2f)
            {
                leftPressed = true;
            }
            else
            {
                rightPressed = true;
            }
        }

        // If both sides have been pressed, fade and deactivate
        if (leftPressed && rightPressed && !tutorialDeactivated)
        {
            StartCoroutine(FadeAndDeactivate());
            tutorialDeactivated = true;
        }
    }

    private IEnumerator FadeAndDeactivate()
    {
        // Fade out both containers simultaneously
        StartCoroutine(FadeContainer(containerLeft, animationDuration));
        StartCoroutine(FadeContainer(containerRight, animationDuration));

        // Wait for fade duration to complete
        yield return new WaitForSeconds(animationDuration);

        // Deactivate the tutorial UI
        gameObject.SetActive(false);
    }

    private IEnumerator FadeContainer(UIContainer container, float duration)
    {
        CanvasGroup canvasGroup = container.GetComponent<CanvasGroup>();
        if (canvasGroup == null)
        {
            canvasGroup = container.gameObject.AddComponent<CanvasGroup>();
        }

        float elapsed = 0f;
        float startAlpha = canvasGroup.alpha;

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            canvasGroup.alpha = Mathf.Lerp(startAlpha, 0f, elapsed / duration);
            yield return null;
        }

        canvasGroup.alpha = 0f;
    }
}
