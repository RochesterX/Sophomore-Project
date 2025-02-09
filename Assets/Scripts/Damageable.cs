using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(Collider2D))]
[RequireComponent(typeof(RespawnOnTriggerEnter))]
public class Damageable : MonoBehaviour
{
    public float force = 50f;
    public float damage = 0f;
    public float maxDamage = 1000f;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Punch Hurtbox"))
        {
            print($"{name}: Ouch");
            Damage(collision.transform.parent.gameObject);
            Recoil(collision.transform.parent.gameObject);
        }
    }

    private void Recoil(GameObject damageSource)
    {
        GetComponent<Rigidbody2D>().AddForce(((transform.position - damageSource.transform.position).normalized + Vector3.up * 2) * damage, ForceMode2D.Force);
        //damageSource.transform.localScale *= 1.1f;
    }

    private void Damage(GameObject damageSource)
    {
        float actualForce = force;
        // Recoil
        GetComponent<Rigidbody2D>().AddForce(((transform.position - damageSource.transform.position).normalized + Vector3.up * 2) * damage, ForceMode2D.Force);

        if (GetComponent<Block>().blocking)
        {
            damageSource.GetComponent<Damageable>().Damage(gameObject);
            actualForce /= 4;
        }
        damage += actualForce;
        damage = Mathf.Clamp(damage, 0f, maxDamage);
    }

    public void ResetDamage()
    {
        damage = 0f;
        //transform.localScale = Vector3.one;
    }
}
