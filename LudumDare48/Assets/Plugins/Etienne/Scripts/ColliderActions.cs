using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(Collider))]
public class ColliderActions : MonoBehaviour {

    [SerializeField]
    private UnityEvent onTriggerEnter = new UnityEvent(),
        onTriggerExit = new UnityEvent(),
        onTriggerStay = new UnityEvent(),
        onCollisionEnter = new UnityEvent(),
        onCollisionExit = new UnityEvent(),
        onCollisionStay = new UnityEvent();
    [SerializeField] private bool needGO, ignoreGameObject;
    public GameObject Go;
    [SerializeField] private GameObject[] ignoredGO;

    private void OnTriggerEnter(Collider other) {
        if(ignoreGameObject) {
            foreach(GameObject go in ignoredGO) {
                if(other.gameObject == go) {
                    return;
                }
            }
        }
        if(needGO) {
            if(other.gameObject == Go) {
                onTriggerEnter.Invoke();
            }
        } else {
            onTriggerEnter.Invoke();
        }
    }
    private void OnTriggerExit(Collider other) {
        if(ignoreGameObject) {
            foreach(GameObject go in ignoredGO) {
                if(other.gameObject == go) {
                    return;
                }
            }
        }
        if(needGO) {
            if(other.gameObject == Go) {
                onTriggerExit.Invoke();
            }
        } else {
            onTriggerExit.Invoke();
        }
    }
    private void OnTriggerStay(Collider other) {
        if(ignoreGameObject) {
            foreach(GameObject go in ignoredGO) {
                if(other.gameObject == go) {
                    return;
                }
            }
        }
        if(needGO) {
            if(other.gameObject == Go) {
                onTriggerStay.Invoke();
            }
        } else {
            onTriggerStay.Invoke();
        }
    }
    private void OnCollisionEnter(Collision collision) {
        if(ignoreGameObject) {
            foreach(GameObject go in ignoredGO) {
                if(collision.gameObject == go) {
                    return;
                }
            }
        }
        if(needGO) {
            if(collision.gameObject == Go) {
                onCollisionEnter.Invoke();
            }
        } else {
            onCollisionEnter.Invoke();
        }
    }
    private void OnCollisionExit(Collision collision) {
        if(ignoreGameObject) {
            foreach(GameObject go in ignoredGO) {
                if(collision.gameObject == go) {
                    return;
                }
            }
        }
        if(needGO) {
            if(collision.gameObject == Go) {
                onCollisionExit.Invoke();
            }
        } else {
            onCollisionExit.Invoke();
        }
    }
    private void OnCollisionStay(Collision collision) {
        if(ignoreGameObject) {
            foreach(GameObject go in ignoredGO) {
                if(collision.gameObject == go) {
                    return;
                }
            }
        }
        if(needGO) {
            if(collision.gameObject == Go) {
                onCollisionStay.Invoke();
            }
        } else {
            onCollisionStay.Invoke();
        }
    }
}
