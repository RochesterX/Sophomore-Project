using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(Collider2D))]
[RequireComponent(typeof(RespawnOnTriggerEnter))]
public class Damageable : MonoBehaviour
{
    public float force = 50f; // Force applied when hit
    public float damage = 0f;
    public float maxDamage = 1000f; // Set max health
    public int lives = 0;
    private Animator animator;
    public bool damageSelfDebug = false;
    public bool dying = false;
    public event System.Action<GameObject> OnPlayerPunched;

    private void Start()
    {
        animator = GetComponent<Animator>();
    }

    private void Update()
    {
        if (damageSelfDebug)
        {
            damageSelfDebug = false;
            Damage(gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision) // Calls Damage method when player is hit
    {
        if (collision.gameObject.CompareTag("Punch Hurtbox"))
        {
            Damage(collision.transform.parent.gameObject);
        }
    }

    private void Damage(GameObject damageSource) // Damages player
    {
        if (dying) return;

        float actualForce = damageSource.GetComponent<Damageable>().force;
        Block blockComponent = GetComponent<Block>();

        GetComponentInChildren<UseItem>().DropItem(); // Drops hat if held

        if (blockComponent != null && blockComponent.blocking) 
        {
            if (blockComponent.IsParrying()) // Player receives damage if punching a parrying player
            {
                damageSource.GetComponent<Damageable>().SuccessfulParry(gameObject, actualForce);
                return;
            }
            else // Player does less damage if punching a blocking player
            {
                actualForce /= 4;
                GetComponent<Rigidbody2D>().AddForce(((transform.position - damageSource.transform.position).normalized + Vector3.up * 2) * actualForce, ForceMode2D.Force);
            }
        }
        else // Player does full damage to a non-blocking player
        {
            GetComponent<Rigidbody2D>().AddForce(((transform.position - damageSource.transform.position).normalized + Vector3.up * 2) * actualForce * (1 + (damage / maxDamage) * 3), ForceMode2D.Force);
        }
        damage += actualForce;
        damage = Mathf.Clamp(damage, 0f, maxDamage);
        if (damage >= maxDamage)
        {
            Die();
        }
    }

    public void Damage(float damage) // Adds damage to player when hit
    {
        this.damage += damage;
        if (damage >= maxDamage)
        {
            Die();
        }
    }

    private void SuccessfulParry(GameObject damageSource, float force)
    {
        GetComponent<Rigidbody2D>().AddForce(((transform.position - damageSource.transform.position).normalized + Vector3.up * 2) * force, ForceMode2D.Force);
        damage += force;
        damage = Mathf.Clamp(damage, 0f, maxDamage);
        if (damage >= maxDamage)
        {
            Die();
        }
    }

    private void Die() // Triggers death animation and sets player to dying state
    {
        if (GameManager.Instance != null)
        {
            UseItem useItem = GetComponent<UseItem>();
            if (useItem != null)
            {
                useItem.DropItem(); // Ensure the player drops the item before the death animation
            }
            animator.SetBool("die", true);
            dying = true;
        }
    }


    public void HandleDeath() // Removes player from dying state after respawn
    {
        GameManager.Instance.PlayerDied(this);
        animator.SetBool("die", false);
        dying = false;
    }

    public void Respawn() // Respawns player to the spawnPosition and resets damage/health bar
    {
        transform.position = GameManager.Instance.spawnPosition;
        if (TryGetComponent<Rigidbody2D>(out var rb))
        {
            rb.linearVelocity = Vector2.zero;
            rb.angularVelocity = 0f;
        }
        if (TryGetComponent<Damageable>(out var damageable))
        {
            damageable.ResetDamage();
        }
    }

    public void ResetDamage()
    {
        damage = 0f;
    }
}
