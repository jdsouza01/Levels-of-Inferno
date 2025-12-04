using Unity.Mathematics;
using UnityEngine;

public class Enemies : MonoBehaviour
{
    public GameObject target;
    public GameObject currSelf;
    public float rotationSpeed = 3f;

    private Quaternion lookRotation;
    private Vector3 directionTarget;

    public float health = 5f;
    public float baseSpeed = 1f; // Set this in Inspector per enemy type
    public float moveSpeed;     // Will be calculated based on level    
    public float lastMoveSpeed;
    private Vector3 movement;

    public bool isStunned = false;
    public bool isSlowed = false;
    public bool isDead = false;

    public Rigidbody rb;
    public GameObject charSprite;
    public GameObject playerMan;

    //Ghost settings
    public bool isGhost = false;
    public float ghostDodgeChance = 0.5f; //chance to ignore a colliding projectile
    public PlayerManager playerManager;

    void Start()
    {
        // Scale speed based on current level
        moveSpeed = baseSpeed + (GameManager.level * 0.3f);
    }

    void FixedUpdate()
    {
        move();
    }

    public void move()
    {
        if (isStunned)
        {
            rb.linearVelocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;
            return;
        }

        if (target == null) return;

        float distance = Vector3.Distance(transform.position, target.transform.position);
        if (distance > 5f)
        {
            rb.constraints = rb.constraints & ~RigidbodyConstraints.FreezeRotationX;
            directionTarget = (target.transform.position - transform.position).normalized;

            lookRotation = Quaternion.LookRotation(directionTarget);
            Quaternion targetRotation = Quaternion.Euler(0, lookRotation.eulerAngles.y, 0);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * rotationSpeed);
            movement = transform.forward * moveSpeed;
            rb.linearVelocity = new Vector3(movement.x, rb.linearVelocity.y, movement.z);
        }
        else
        {
            rb.constraints = RigidbodyConstraints.FreezeRotation;
            playerManager.onHit();
        }
    }

    public void Stun(float duration)
    {
        if (!isStunned)
        {
            StartCoroutine(StunCoroutine(duration));
        }
    }

    private System.Collections.IEnumerator StunCoroutine(float duration)
    {
        isStunned = true;        
        lastMoveSpeed = moveSpeed;
        moveSpeed = 0f;
        rb.linearVelocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;        

        yield return new WaitForSeconds(duration);
        
        moveSpeed = lastMoveSpeed;        
        isStunned = false;
    }

    public void Slow(float duration)
    {
        if (!isSlowed)
        {
            StartCoroutine(slowCoroutine(duration));
        }
    }

    private System.Collections.IEnumerator slowCoroutine(float duration)
    {
        isSlowed = true;        
        lastMoveSpeed = moveSpeed;
        moveSpeed *= 0.70f;      

        yield return new WaitForSeconds(duration);
        
        moveSpeed = lastMoveSpeed;        
        isSlowed = false;
    }



    public virtual bool hit(float damage)
    {
        if (isGhost)
        {
            if (UnityEngine.Random.value < ghostDodgeChance)
            {
                return false; //Ignores the hit from the projectile
            }
        }

        health -= damage;
        charSprite.GetComponent<colorFlash>().flashStart();
        return health <= 0;
    }

    public void dead()
    {
        isDead = true;
    }

    public virtual bool execute()
    {
        health = 0;
        charSprite.GetComponent<colorFlash>().flashStart();
        return health <= 0;

    }

    public bool isAlive()
    {
        return health > 0;
    }


}

