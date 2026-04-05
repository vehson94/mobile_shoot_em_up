using UnityEngine;
using UnityEngine.UI;

public class Spinner : MonoBehaviour
{
    public Image image;
    public float rotationSpeed;
    public AnimationCurve speedOverTime = AnimationCurve.Linear(0f, 1f, 1f, 1f);
    public float speedCurveDuration = 1f;

    private float elapsedTime;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (image != null)
        {
            elapsedTime += Time.unscaledDeltaTime;

            float curveTime = speedCurveDuration > 0f
                ? Mathf.Repeat(elapsedTime, speedCurveDuration) / speedCurveDuration
                : 0f;
            float currentSpeed = rotationSpeed * speedOverTime.Evaluate(curveTime);

            image.transform.Rotate(Vector3.forward, currentSpeed * Time.unscaledDeltaTime);
        }
    }
}
