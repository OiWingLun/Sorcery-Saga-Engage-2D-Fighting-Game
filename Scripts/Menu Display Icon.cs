using UnityEngine;
using UnityEngine.EventSystems;

public class ButtonHoverDisplayHandler : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public GameObject hoverImage; // Reference to the hover image to display
    public GameObject iconToDisplay; // Reference to another image/icon

    private void Start()
    {
        if (hoverImage != null)
        {
            hoverImage.SetActive(false); // Hide the hover image at the start
        }

        if (iconToDisplay != null)
        {
            iconToDisplay.SetActive(false); // Hide the icon at the start
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (hoverImage != null)
        {
            hoverImage.SetActive(true); // Show the hover image when the button is hovered over
        }

        if (iconToDisplay != null)
        {
            iconToDisplay.SetActive(true); // Show the icon when the button is hovered over
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (hoverImage != null)
        {
            hoverImage.SetActive(false); // Hide the hover image when the pointer exits the button
        }

        if (iconToDisplay != null)
        {
            iconToDisplay.SetActive(false); // Hide the icon when the pointer exits the button
        }
    }
}
