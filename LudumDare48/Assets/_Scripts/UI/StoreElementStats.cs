using UnityEngine;

[CreateAssetMenu(fileName = "Stats", menuName = "Store Element/Stats")]
public class StoreElementStats : ScriptableObject {
    public string Name { get => this.name; }
    public BonusTypes BonusTypes { get => this.bonusType; }
    public int Amount { get => this.amount; }
    public int Cost { get => this.cost; }

    [SerializeField] private new string name = "Element";
    [SerializeField] private BonusTypes bonusType = BonusTypes.DigStrenght;
    [SerializeField] private int amount = 1, cost = 1;
}

public enum BonusTypes { BreathTime, Money, DigStrenght, UnlockAutoClick, AutoClicksDamage, AutoClickFrequency, SwimSpeed }