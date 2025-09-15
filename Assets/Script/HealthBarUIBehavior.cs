using UnityEngine;
using UnityEngine.UI;

public class HealthBarUI : MonoBehaviour
{
    public Health targetHealth;
    public Slider slider;
    public Transform followTarget;
    public Vector3 worldOffset = new Vector3(0, 2.0f, 0);
    Camera cam;

    void Awake()
    {
        cam = Camera.main;
        if (!slider) slider = GetComponentInChildren<Slider>(true);
        if (targetHealth) slider.maxValue = targetHealth.maxHealth;
    }

    void OnEnable()
    {
        if (targetHealth != null)
            targetHealth.OnHealthChanged += UpdateBar;
    }

    void OnDisable()
    {
        if (targetHealth != null)
            targetHealth.OnHealthChanged -= UpdateBar;
    }

    void LateUpdate()
    {
        if (followTarget)
        {
            transform.position = followTarget.position + worldOffset;
            if (cam) transform.rotation = Quaternion.LookRotation(transform.position - cam.transform.position);
        }
    }

    void UpdateBar(int current, int max)
    {
        slider.maxValue = max;
        slider.value = current;
    }
}
