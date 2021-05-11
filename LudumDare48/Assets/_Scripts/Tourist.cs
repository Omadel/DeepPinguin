using DG.Tweening;
using UnityEngine;

public class Tourist : MonoBehaviour {
    [SerializeField] private Etienne.Path walkingPath = null;

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
        if(walkingPath == null) {
            return;
        }
        path = walkingPath.WorldWaypoints;
        transform.DOPath(path, pathDuration, pathType, pathMode)
            .SetLoops(loops, pathLoopType)
            .SetEase(pathEase)
            .SetLookAt(lookAhead)
            ;
        GameObject.Destroy(walkingPath.gameObject);
    }

    private Vector3[] path;
}
