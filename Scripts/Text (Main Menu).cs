using UnityEngine;
using UnityEngine.EventSystems;
using TMPro;

public class TextHoverEffectUI : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    private TextMeshProUGUI text;
    private Color originalColor;
    public Color hoverColor = Color.yellow;

    void Start()
    {
        text = GetComponent<TextMeshProUGUI>();
        originalColor = text.color;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        text.color = hoverColor;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        text.color = originalColor;
    }
}
