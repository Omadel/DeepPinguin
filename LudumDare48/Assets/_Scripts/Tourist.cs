using DG.Tweening;
using UnityEngine;

public class Tourist : MonoBehaviour {
    [SerializeField] private WalkingPath walkingPath = null;

    [SerializeField] private float pathDuration = 20f;
    [SerializeField] private PathType pathType = PathType.Linear;
    [SerializeField] private PathMode pathMode = PathMode.TopDown2D;
    [Header("Loop")]
    [Tooltip("-1 for unlimited")]
    [SerializeField] private int loops = -1;
    [SerializeField] private LoopType pathLoopType = LoopType.Yoyo;
    [Header("Ease")]
    [SerializeField] private Ease pathEase = Ease.InOutSine;
    [Tooltip("The percentage of lookAhead to use (0 to 1)")]
    [SerializeField] private float lookAhead = .1f;

    private void Start() {
        if(this.walkingPath == null) {
            return;
        }
        this.path = this.walkingPath.Waypoints;
        this.transform.DOPath(this.path, this.pathDuration, this.pathType, this.pathMode)
            .SetLoops(this.loops, this.pathLoopType)
            .SetEase(this.pathEase)
            .SetLookAt(this.lookAhead)
            ;
        GameObject.Destroy(this.walkingPath.gameObject);
    }

    private Vector3[] path;
}
