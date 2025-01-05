using UnityEngine;
using UnityEngine.EventSystems;
using TMPro;  // If using TextMeshPro

public class ButtonDescription : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public TextMeshProUGUI descriptionText;  
    public string description; 

    public void OnPointerEnter(PointerEventData eventData)
    {
        descriptionText.text = description;  
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        descriptionText.text = "";  
    }
}
