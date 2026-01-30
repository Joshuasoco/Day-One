using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections;

public class HoverBorder : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public GameObject border;
    [Header("Animation Settings")]
    public float fadeSpeed = 0.3f;
    public float scaleAmount = 1.1f;
    
    private CanvasGroup canvasGroup;
    private Vector3 originalScale;
    private Coroutine currentAnimation;

    void Start()
    {
        // Ensure border has a CanvasGroup for fade effect
        if (border != null)
        {
            canvasGroup = border.GetComponent<CanvasGroup>();
            if (canvasGroup == null)
                canvasGroup = border.AddComponent<CanvasGroup>();
            
            originalScale = border.transform.localScale;
            
            // Start with border invisible
            canvasGroup.alpha = 0f;
            border.SetActive(true); // Keep active but transparent
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (currentAnimation != null)
            StopCoroutine(currentAnimation);
            
        currentAnimation = StartCoroutine(AnimateIn());
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (currentAnimation != null)
            StopCoroutine(currentAnimation);
            
        currentAnimation = StartCoroutine(AnimateOut());
    }
    
    private IEnumerator AnimateIn()
    {
        float elapsedTime = 0f;
        float startAlpha = canvasGroup.alpha;
        Vector3 startScale = border.transform.localScale;
        Vector3 targetScale = originalScale * scaleAmount;
        
        while (elapsedTime < fadeSpeed)
        {
            elapsedTime += Time.deltaTime;
            float progress = elapsedTime / fadeSpeed;
            
            // Smooth curve for more natural animation
            float smoothProgress = Mathf.SmoothStep(0f, 1f, progress);
            
            canvasGroup.alpha = Mathf.Lerp(startAlpha, 1f, smoothProgress);
            border.transform.localScale = Vector3.Lerp(startScale, targetScale, smoothProgress);
            
            yield return null;
        }
        
        canvasGroup.alpha = 1f;
        border.transform.localScale = targetScale;
    }
    
    private IEnumerator AnimateOut()
    {
        float elapsedTime = 0f;
        float startAlpha = canvasGroup.alpha;
        Vector3 startScale = border.transform.localScale;
        
        while (elapsedTime < fadeSpeed)
        {
            elapsedTime += Time.deltaTime;
            float progress = elapsedTime / fadeSpeed;
            
            // Smooth curve for more natural animation
            float smoothProgress = Mathf.SmoothStep(0f, 1f, progress);
            
            canvasGroup.alpha = Mathf.Lerp(startAlpha, 0f, smoothProgress);
            border.transform.localScale = Vector3.Lerp(startScale, originalScale, smoothProgress);
            
            yield return null;
        }
        
        canvasGroup.alpha = 0f;
        border.transform.localScale = originalScale;
    }
}
