using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(Collider2D))]
[RequireComponent(typeof(RespawnOnTriggerEnter))]
public class Damageable : MonoBehaviour
{
    public float force = 50f;
    public float damage = 0f;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Punch Hurtbox"))
        {
            if (GetComponent<Block>().blocking)
            {
                collision.gameObject.GetComponent<Damageable>().Damage(gameObject);
                return;
            }
            Damage(collision.transform.parent.gameObject);
        }
    }

    private void Recoil(GameObject damageSource)
    {
        GetComponent<Rigidbody2D>().AddForce(((transform.position - damageSource.transform.position).normalized + Vector3.up) * damage, ForceMode2D.Force);
        //damageSource.transform.localScale *= 1.1f;
    }

    public void Damage(GameObject source)
    {
        damage += force;
        Recoil(source);
    }

    public void ResetDamage()
    {
        damage = 0f;
        //transform.localScale = Vector3.one;
    }
}
