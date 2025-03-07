using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(Collider2D))]
[RequireComponent(typeof(RespawnOnTriggerEnter))]
public class Damageable : MonoBehaviour
{
    public float force = 50f;
    public float damage = 0f;
    public float maxDamage = 1000f;
    public int lives = 3;
    //private GameManager gameManager;
    private Animator animator;

    private void Start()
    {
        //gameManager = GameManager.Instance;
        animator = GetComponent<Animator>();
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
        if (damage >= maxDamage)
        {
            Die();
        }
    }

    public void Damage(float damage)
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

    private void Die()
    {
        //Debug.Log($"{name}: MAKE THIS WORK.");
        if (GameManager.Instance != null)
        {
            animator.SetTrigger("Die");
            StartCoroutine(HandleDeath());
        }
    }

    private IEnumerator HandleDeath()
    {
        //yield return new WaitForSeconds(animator.GetCurrentAnimatorStateInfo(0).length);
        yield return new WaitForSeconds(0.5f);
        GameManager.Instance.PlayerDied(this);
    }

    public void Respawn()
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
