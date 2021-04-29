using DG.Tweening;
using UnityEngine;

[CreateAssetMenu(fileName = "Animation Parameters", menuName = "Animation Parameters")]
public class AnimationParametersScriptableObject : ScriptableObject {
    public float Delay { get => this.delay; }
    public float Duration { get => this.duration; }
    public AnimationCurve Curve { get => this.curve; }
    public Ease Ease { get => this.ease; }


    [SerializeField] private float delay = .01f;
    [SerializeField] private float duration = .2f;
    [SerializeField] private AnimationCurve curve = null;
    [SerializeField] private Ease ease = Ease.OutQuad;

}
