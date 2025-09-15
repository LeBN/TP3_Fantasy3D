using System;
using UnityEngine;

public class Health : MonoBehaviour
{
    public event Action<int, int> OnHealthChanged;
    public int maxHealth = 100;
    public int Current { get; private set; }

    void Awake() => Current = maxHealth;

    public bool IsAlive => Current > 0;

    public void TakeDamage(int amount)
    {
        if (!IsAlive) return;
        Current = Mathf.Max(0, Current - amount);
        OnHealthChanged?.Invoke(Current, maxHealth);
        if (Current == 0) Die();
    }

    public void Heal(int amount)
    {
        if (!IsAlive) return;
        Current = Mathf.Min(maxHealth, Current + amount);
        OnHealthChanged?.Invoke(Current, maxHealth);
    }

    void Die()
    {
        // TODO: anim mort, disable IA, etc.
        // Destroy(gameObject, 3f); // par ex.
    }
}
