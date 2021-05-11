using UnityEngine;

public class StoreBehaviour : MonoBehaviour {
    [ContextMenu("Refresh Size")]
    public void RefreshSize() {
        StoreElement[] activeStoreElements = GetComponentsInChildren<StoreElement>();
        RectTransform rectTransform = GetComponent<RectTransform>();
        rectTransform.sizeDelta = new Vector2(rectTransform.sizeDelta.x, 20 + (110 * (activeStoreElements.Length)));
    }
}
