using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{

    public Slider slider;
    private float maxValue;

    public float MaxValue {get => maxValue; set {slider.maxValue = value;}}
    public float CurrentValue {get => slider.value; set {slider.value = value;}}

    void Awake()
    {
        slider = GetComponent<Slider>();
    }

    public void UpdateValue(float amount)
    {
        slider.value -= amount;
    }
}