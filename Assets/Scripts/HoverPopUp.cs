using UnityEngine;
using UnityEngine.EventSystems;

public class HoverPopUp : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public Vector3 normalScale = Vector3.one;
    public Vector3 hoverScale = new Vector3(1.1f, 1.1f, 1.1f);
    public float animationSpeed = 5f;

    private bool isHovering = false;

    void Update()
    {
        // Smooth transition between scales
        transform.localScale = Vector3.Lerp(transform.localScale,
            isHovering ? hoverScale : normalScale,
            Time.deltaTime * animationSpeed);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        isHovering = true;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        isHovering = false;
    }
}
