using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(Collider2D))]
[RequireComponent(typeof(RespawnOnTriggerEnter))]
public class Damageable : MonoBehaviour
{
    public float force = 50f;
    public float damage = 0f;
    public float maxDamage = 1000f;
    public HealthBar healthBar;

    private void Start()
    {
        if (healthBar != null)
        {
            healthBar.SetMaxHealth(maxDamage);
            healthBar.SetHealth(maxDamage - damage);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Punch Hurtbox"))
        {
            print($"{name}: Ouch");
            Damage(collision.transform.parent.gameObject);
        }
    }

    private void Damage(GameObject damageSource)
    {
        float actualForce = force;
        Block blockComponent = GetComponent<Block>();
        if (blockComponent != null && blockComponent.blocking)
        {
            if (blockComponent.IsParrying())
            {
                damageSource.GetComponent<Damageable>().SuccessfulParry(gameObject, actualForce);
                return;
            }
            else
            {
                actualForce /= 4;
                GetComponent<Rigidbody2D>().AddForce(((transform.position - damageSource.transform.position).normalized + Vector3.up * 2) * actualForce, ForceMode2D.Force);
            }
        }
        else
        {
            GetComponent<Rigidbody2D>().AddForce(((transform.position - damageSource.transform.position).normalized + Vector3.up * 2) * actualForce, ForceMode2D.Force);
        }
        damage += actualForce;
        damage = Mathf.Clamp(damage, 0f, maxDamage);
        if (healthBar != null)
        {
            healthBar.SetHealth(maxDamage - damage);
        }
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
        if (healthBar != null)
        {
            healthBar.SetHealth(maxDamage - damage);
        }
        if (damage >= maxDamage)
        {
            Die();
        }
    }

    private void Die()
    {
        PlayerLives playerLives = GetComponent<PlayerLives>(); //add death animation trigger
        if (playerLives != null)
        {
            playerLives.PlayerDied();
        }
    }

    public void ResetDamage()
    {
        damage = 0f;
        if (healthBar != null)
        {
            healthBar.SetHealth(maxDamage);
        }
        //transform.localScale = Vector3.one;
    }
}
