using UnityEngine;

public abstract class UIBase : MonoBehaviour
{
    protected Canvas canvas;
    protected CanvasGroup canvasGroup;
    
    protected virtual void Awake()
    {
        canvas = GetComponent<Canvas>();
        canvasGroup = GetComponent<CanvasGroup>();
        
        if (canvas == null)
            canvas = gameObject.AddComponent<Canvas>();
            
        if (canvasGroup == null)
            canvasGroup = gameObject.AddComponent<CanvasGroup>();
    }

    public virtual void Start()
    {
        
    }

    public virtual void Show()
    {
        canvas.enabled = true;
        canvasGroup.alpha = 1;
        canvasGroup.interactable = true;
        canvasGroup.blocksRaycasts = true;
    }

    public virtual void Hide()
    {
        canvas.enabled = false;
        canvasGroup.alpha = 0;
        canvasGroup.interactable = false;
        canvasGroup.blocksRaycasts = false;
    }
}