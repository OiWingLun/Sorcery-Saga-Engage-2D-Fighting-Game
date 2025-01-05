
using UnityEngine;
using UnityEngine.UI;

public class BarScript : MonoBehaviour
{

    [SerializeField]
    private float fillAmount;
    [SerializeField]
    private float lerpSpeed;
    [SerializeField]
    private Image Content;

    public float MaxVal { get; set; }

    public float Value {
        set {
            fillAmount = Map(value, 0, MaxVal, 0, 1);
        }
    }
    void Update()
    {
        HandleBar();
    }

    void HandleBar() {
        if(fillAmount != Content.fillAmount) {
            Content.fillAmount = Mathf.Lerp(Content.fillAmount, fillAmount, Time.deltaTime * lerpSpeed);
        }
    }
    private float Map(float value, float inMin, float inMax, float outMin, float outMax) {
        return (value - inMin) * (outMax - outMin) / (inMax - inMin) + outMin;
    }
}
