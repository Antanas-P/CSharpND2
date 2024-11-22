using UnityEngine;

/// <summary>
/// Mano generic tipas 1t
/// Pritaikete savo bendraji (generic) tipa naudojant raktazodi 'where' (1 t.)
/// </summary>
public class GenericHealthComponent<T> where T : MonoBehaviour, IHealth
{
    private T entity;

    public GenericHealthComponent(T entity)
    {
        this.entity = entity;
    }

    public void ApplyDamage(int damage)
    {
        entity.TakeDamage(damage);
    }

    public void ApplyHeal(int heal)
    {
        entity.Heal(heal);
    }
}