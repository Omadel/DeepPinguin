using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour {
    public static GameManager Instance { get; private set; }
    public PoolDepthBehaviour Pool { get => this.pool; }
    public TextMeshProUGUI Score { get => this.scoreText; }
    public Slider BreathBar { get => this.breathBar; }

    [SerializeField] private PoolDepthBehaviour pool = null;
    [SerializeField] private TMPro.TextMeshProUGUI scoreText = null;
    [SerializeField] private Slider breathBar = null;

    private void Awake() {
        if(GameManager.Instance == null) {
            GameManager.Instance = this;
        } else {
            GameObject.Destroy(this.gameObject);
        }
    }
}
