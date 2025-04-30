using UnityEngine;
using UnityEngine.UI;

public class AutoScroll : MonoBehaviour
{
    public ScrollRect scrollRect;
    public float scrollSpeed = 0.05f;

    void Update()
    {
        if (scrollRect != null)
        {
            float newPos = scrollRect.verticalNormalizedPosition - scrollSpeed * Time.deltaTime;
            scrollRect.verticalNormalizedPosition = Mathf.Clamp01(newPos);
        }
    }
}
