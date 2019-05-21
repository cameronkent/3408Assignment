using System;
using UnityEngine;
using UnityEngine.Events;

public class Damageable : MonoBehaviour
{
    [Serializable]
    public class HealthEvent : UnityEvent<Damageable>
    { }

    [Serializable]
    public class DamageEvent : UnityEvent<Damager, Damageable>
    { }

    [Serializable]
    public class HealEvent : UnityEvent<int, Damageable>
    { }

    public int startingHealth;
    public bool invulnerableAfterDamage = true;
    public float invulnerabilityDuration = 3f;
    public bool disableOnDeath = false;

    public Vector2 centreOffset = new Vector2(0f, 1f);
    public HealthEvent OnHealthSet;
    public DamageEvent OnTakeDamage;
    public DamageEvent OnDie;
    public HealEvent OnGainHealth;

    protected bool m_Invulnerable;
    protected float m_InvulnerabilityTimer;
    protected int m_CurrentHealth;
    protected Vector2 m_DamageDirection;
    protected bool m_ResetHealthOnSceneReload;

    public int CurrentHealth
    {
        get {return m_CurrentHealth; }
    }

    private void OnEnable()
    {
        m_CurrentHealth = startingHealth;

        OnHealthSet.Invoke(this);

        DisableInvulnerability();
    }

    private void OnDisable()
    {
        
    }

    private void Update()
    {
        if (m_Invulnerable)
        {
            m_InvulnerabilityTimer -= Time.deltaTime;

            if (m_InvulnerabilityTimer <= 0f)
            {
                m_Invulnerable = false;
            }
        }
    }

    public void EnableInvulnerability(bool ignoreTimer = false)
    {
        m_Invulnerable = true;
        m_InvulnerabilityTimer = ignoreTimer ? float.MaxValue : invulnerabilityDuration;
    }

    public void DisableInvulnerability()
    {
        m_Invulnerable = false;
    }

    public void TakeDamage(Damager damager, bool ignoreInvincible = false)
    {
        if ((m_Invulnerable && !ignoreInvincible) || m_CurrentHealth <= 0)
        {
            return;
        }

        if (!m_Invulnerable)
        {
            m_CurrentHealth -= damager.damage;
            OnHealthSet.Invoke(this);
        }

        m_DamageDirection = transform.position + (Vector3)centreOffset - damager.transform.position;

        OnTakeDamage.Invoke(damager, this);

        if (m_CurrentHealth <=0)
        {
            OnDie.Invoke(damager, this);
            m_ResetHealthOnSceneReload = true;
            EnableInvulnerability();
            if (disableOnDeath) gameObject.SetActive(false);
        }
    }

    public void GainHealth(int amount)
    {
        m_CurrentHealth += amount;

        if (m_CurrentHealth > startingHealth) m_CurrentHealth = startingHealth;

        OnHealthSet.Invoke(this);

        OnGainHealth.Invoke(amount, this);
    }
    public void SetHealth(int amount)
    {
        m_CurrentHealth = amount;

        if (m_CurrentHealth <= 0)
        {
            OnDie.Invoke(null, this);
            m_ResetHealthOnSceneReload = true;
            EnableInvulnerability();
            if (disableOnDeath) gameObject.SetActive(false);
        }
    }
}
