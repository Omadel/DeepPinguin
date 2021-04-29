using UnityEngine;
using UnityEngine.Events;

public class AnimationEvent : MonoBehaviour {
    [SerializeField] private UnityEvent @event = new UnityEvent();

    public void TriggerEvent() {
        this.@event.Invoke();
    }
}
