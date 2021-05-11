using System;
using UnityEngine;

[AttributeUsage(AttributeTargets.Field)]
public class LabelOverride : PropertyAttribute {
    public string label;
    public LabelOverride(string label) {
        this.label = label;
    }
}