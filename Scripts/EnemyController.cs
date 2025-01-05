using System.Collections;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    private CharacterAnimation enemyAnim;
    private Rigidbody2D enemyRigidBody;
    private PlayerController playerController;
    private AierController aierController;
    private HaiyinController haiyinController;
    private CeciliaController ceciliaController;
    private Health myHealth;

    [SerializeField] private string enemyName;
    [SerializeField] private float movementSpeed;
    [SerializeField] private float attackDistance;
    [SerializeField] private float defaultAttackTimer;
    [SerializeField] public float punch1Damage = 5f;
    [SerializeField] public float kick1Damage = 10f;
    [SerializeField] public float punch1StunDuration;
    [SerializeField] public float kick1StunDuration;
    [SerializeField] private GameObject punch1AttackPoint;
    [SerializeField] private GameObject punch2AttackPoint;
    [SerializeField] private GameObject kick1AttackPoint;
    [SerializeField] private BarStat healthBar;
    [SerializeField] private BarStat blockBar;

    private bool isStunned = false;
    private float stunTimer = 0f;
    private Transform playerTransform;
    private bool followPlayer;
    private bool attackPlayer;
    private bool isAttacking;
    private bool isDie;
    private float currentAttackTimer;

    private void Awake()
    {
        // Cache references for performance and avoid repeated calls to FindGameObjectWithTag
        CachePlayerReferences();
        myHealth = GetComponent<Health>();
        enemyAnim = GetComponent<CharacterAnimation>();
        enemyRigidBody = GetComponent<Rigidbody2D>();

        // Initialize health bar
        healthBar.bar = GameObject.FindGameObjectWithTag(Tags.Right_Health_Bar).GetComponent<BarScript>();
        healthBar.Initialize();
        blockBar.bar = GameObject.FindGameObjectWithTag(Tags.Right_Block_Bar).GetComponent<BarScript>();
        blockBar.Initialize();
    }

    void Start()
    {
        // Initialize game state
        healthBar.MaxVal = myHealth.maxHealth;
        followPlayer = true;
        attackPlayer = false;
        isAttacking = false;
        isDie = false;
        currentAttackTimer = defaultAttackTimer;
        GameController.gameController.enemyName.text = enemyName;
    }

    void Update()
    {
        if (isStunned)
    {
        stunTimer -= Time.deltaTime;
        if (stunTimer <= 0f)
        {
            isAttacking = false;
            isStunned = false;
        }
        return; // Skip other actions while stunned
    }
        if (isDie) return;
        healthBar.CurrentVal = myHealth.health;
        blockBar.CurrentVal = myHealth.block;
        FacingToTarget();
        DeadChecker();
        GameController.gameController.enemyHealth = myHealth.health;
    }

    void FixedUpdate()
{
    if (isDie) return;

    // Check if player references are still valid
    if (playerTransform == null || playerController == null)
    {
        CachePlayerReferences(); // Attempt to re-cache references
    }

    if (playerTransform != null)
    {
        FollowPlayer();
        AttackPlayer();
    }
}

    private void CachePlayerReferences()
{
    // Cache references for all potential player components
    GameObject playerObject = GameObject.FindGameObjectWithTag(Tags.Player_Tag);
    if (playerObject != null)
    {
        playerTransform = playerObject.transform;
        playerController = playerObject.GetComponent<PlayerController>();
        aierController = playerObject.GetComponent<AierController>();
        ceciliaController = playerObject.GetComponent<CeciliaController>();
        haiyinController = playerObject.GetComponent<HaiyinController>();
    }
    else
    {
        playerTransform = null;
        playerController = null;
        aierController = null;
        ceciliaController = null;
        haiyinController = null;
    }
}

    void FollowPlayer()
{
    if (isDie || isStunned || isAttacking) return;

    if (playerTransform != null && Mathf.Abs(transform.position.x - playerTransform.position.x) > attackDistance)
    {
        float moveDirection = playerTransform.position.x < transform.position.x ? -1 : 1;
        enemyRigidBody.velocity = new Vector2(moveDirection * movementSpeed, enemyRigidBody.velocity.y);
        enemyAnim.Walk(1);
    }
    else
    {
        enemyRigidBody.velocity = Vector2.zero;
        enemyAnim.Walk(0);
        followPlayer = false;
        attackPlayer = true;
    }
}

    void AttackPlayer()
{
    if (isDie || !attackPlayer || isAttacking || isStunned) return;

    currentAttackTimer -= Time.deltaTime;

    if (currentAttackTimer <= 0)
    {
        int randomAttackIndex = Random.Range(0, 5); // Generate a random attack index
        Attack(randomAttackIndex);
        currentAttackTimer = defaultAttackTimer; // Reset attack timer
    }

    if (playerTransform != null && Mathf.Abs(transform.position.x - playerTransform.position.x) > attackDistance)
    {
        attackPlayer = false;
        followPlayer = true;
    }
}

    void FacingToTarget()
{
    if (isAttacking || playerTransform == null)
    {
        return;
    }

    // Flip the enemy based on player position
    Vector3 theScale = transform.localScale;
    theScale.x = playerTransform.position.x < transform.position.x ? -1 : 1;
    transform.localScale = theScale;
}

    void DeadChecker()
{
    if (isDie) return; 

    if (myHealth.health <= 0)
    {
        isDie = true;
        enemyAnim.Die(isDie);
        enemyRigidBody.velocity = Vector2.zero;
        enemyRigidBody.isKinematic = true;
        DeactivateAllAttack();
        CapsuleCollider2D collider = GetComponent<CapsuleCollider2D>();
        if (collider != null)
        {
            collider.enabled = false;
        }

        GameController.gameController.isEnemyDead = isDie;
    }
}

    void Attack(int attackIndex)
    {
        if (isDie || isAttacking) return; // Prevent overlapping attacks

        isAttacking = true;

        switch (attackIndex)
        {
            case 0:
                StartCoroutine(PerformAttack(Punch_1(0.3f)));
                break;
            case 1:
                StartCoroutine(PerformAttack(Punch_2(0.3f)));
                break;
            case 2:
                StartCoroutine(PerformAttack(Kick(0.3f)));
                break;
            case 3:
                StartCoroutine(PerformAttack(ComboAttack()));
                break;
            case 4:
                StartCoroutine(PerformAttack(Kick(0.6f)));
                break;
        }
    }

    IEnumerator PerformAttack(IEnumerator attackRoutine)
    {
        yield return attackRoutine; // Wait for the attack to finish
        isAttacking = false; // Reset isAttacking flag
    }

    IEnumerator Punch_1(float time) { yield return new WaitForSeconds(time); enemyAnim.Punch1(); }
    IEnumerator Punch_2(float time) { yield return new WaitForSeconds(time); enemyAnim.Punch2(); }
    IEnumerator Kick(float time) { yield return new WaitForSeconds(time); enemyAnim.Kick1(); }
    IEnumerator ComboAttack() { yield return Punch_1(0.3f); yield return Punch_2(0.3f); yield return Kick(0.6f); }

    private void OnTriggerEnter2D(Collider2D collision)
{
    // List of attack tags and their corresponding damage, stun, and pushback values
    AttackData attackData = GetAttackData(collision.tag);

    // If attack data is found, apply damage, stun, and pushback
    if (attackData != null)
    {
        AudioController.audioController.PlaySound("HURT");
        enemyAnim.Hurt();
        myHealth.health -= attackData.damage;
        ApplyStun(attackData.stunDuration);
        ApplyPushback(attackData.position, attackData.damage);
    }
}

private AttackData GetAttackData(string attackTag)
{
    // Match tag with the correct controller
    switch (attackTag)
    {
        case Tags.Punch_Attack_Tag:
            return new AttackData(playerController.punch1Damage, playerController.punch1StunDuration, playerController.transform.position);
        case Tags.Kick_Attack_Tag:
            return new AttackData(playerController.kick1Damage, playerController.kick1StunDuration, playerController.transform.position);
        case Tags.Special1_Attack_Tag:
            return new AttackData(playerController.special1Damage, playerController.special1StunDuration, playerController.transform.position);
        case Tags.Special2_Attack_Tag:
            return new AttackData(playerController.special2Damage, playerController.special2StunDuration, playerController.transform.position);
        case Tags.JumpAttack1_Attack_Tag:
            return new AttackData(playerController.jumpAttack1Damage, playerController.jumpAttack1StunDuration, playerController.transform.position);
        case Tags.JumpAttack2_Attack_Tag:
            return new AttackData(playerController.jumpAttack2Damage, playerController.jumpAttack2StunDuration, playerController.transform.position);
        case Tags.CrouchAttack1_Attack_Tag:
            return new AttackData(playerController.crouchAttack1Damage, playerController.crouchAttack1StunDuration, playerController.transform.position);
        case Tags.CrouchAttack2_Attack_Tag:
            return new AttackData(playerController.crouchAttack2Damage, playerController.crouchAttack2StunDuration, playerController.transform.position);
        case Tags.SwaySword1_Attack_Tag:
            return new AttackData(playerController.swaysword1Damage, playerController.swaysword1StunDuration, playerController.transform.position);
        case Tags.SwaySword2_Attack_Tag:
            return new AttackData(playerController.swaysword2Damage, playerController.swaysword2StunDuration, playerController.transform.position);
        case Tags.AierPunch_Attack_Tag:
            return new AttackData(aierController.punch1Damage, aierController.punch1StunDuration, aierController.transform.position);
        case Tags.AierKick_Attack_Tag:
            return new AttackData(aierController.kick1Damage, aierController.kick1StunDuration, aierController.transform.position);
        case Tags.AierSpecial1_Attack_Tag:
            return new AttackData(aierController.special1Damage, aierController.special1StunDuration, aierController.transform.position);
        case Tags.AierSpecial2_Attack_Tag:
            return new AttackData(aierController.special2Damage, aierController.special2StunDuration, aierController.transform.position);
        case Tags.AierJumpAttack1_Attack_Tag:
            return new AttackData(aierController.jumpAttack1Damage, aierController.jumpAttack1StunDuration, aierController.transform.position);
        case Tags.AierJumpAttack2_Attack_Tag:
            return new AttackData(aierController.jumpAttack2Damage, aierController.jumpAttack2StunDuration, aierController.transform.position);
        case Tags.AierCrouchAttacker1_Attack_Tag:
            return new AttackData(aierController.crouchAttack1Damage, aierController.crouchAttack1StunDuration, aierController.transform.position);
        case Tags.AierCrouchAttacker2_Attack_Tag:
            return new AttackData(aierController.crouchAttack2Damage, aierController.crouchAttack2StunDuration, aierController.transform.position);
        case Tags.HaiyinPunch_Attack_Tag:
            return new AttackData(haiyinController.punch1Damage, haiyinController.punch1StunDuration, haiyinController.transform.position);
        case Tags.HaiyinKick_Attack_Tag:
            return new AttackData(haiyinController.kick1Damage, haiyinController.kick1StunDuration, haiyinController.transform.position);
        case Tags.HaiyinSpecial1_Attack_Tag:
            return new AttackData(haiyinController.special1Damage, haiyinController.special1StunDuration, haiyinController.transform.position);
        case Tags.HaiyinSpecial2_Attack_Tag:
            return new AttackData(haiyinController.special2Damage, haiyinController.special2StunDuration, haiyinController.transform.position);
        case Tags.HaiyinJumpAttack1_Attack_Tag:
            return new AttackData(haiyinController.jumpAttack1Damage, haiyinController.jumpAttack1StunDuration, haiyinController.transform.position);
        case Tags.HaiyinJumpAttack2_Attack_Tag:
            return new AttackData(haiyinController.jumpAttack2Damage, haiyinController.jumpAttack2StunDuration, haiyinController.transform.position);
        case Tags.HaiyinCrouchAttacker1_Attack_Tag:
            return new AttackData(haiyinController.crouchAttack1Damage, haiyinController.crouchAttack1StunDuration, haiyinController.transform.position);
        case Tags.HaiyinCrouchAttacker2_Attack_Tag:
            return new AttackData(haiyinController.crouchAttack2Damage, haiyinController.crouchAttack2StunDuration, haiyinController.transform.position);
        case Tags.CeciliaPunch_Attack_Tag:
            return new AttackData(ceciliaController.punch1Damage, ceciliaController.punch1StunDuration, ceciliaController.transform.position);
        case Tags.CeciliaKick_Attack_Tag:
            return new AttackData(ceciliaController.kick1Damage, ceciliaController.kick1StunDuration, ceciliaController.transform.position);
        case Tags.CeciliaSpecial1_Attack_Tag:
            return new AttackData(ceciliaController.special1Damage, ceciliaController.special1StunDuration, ceciliaController.transform.position);
        case Tags.CeciliaJumpAttack1_Attack_Tag:
            return new AttackData(ceciliaController.jumpAttack1Damage, ceciliaController.jumpAttack1StunDuration, ceciliaController.transform.position);
        case Tags.CeciliaJumpAttack2_Attack_Tag:
            return new AttackData(ceciliaController.jumpAttack2Damage, ceciliaController.jumpAttack2StunDuration, ceciliaController.transform.position);
        case Tags.CeciliaCrouchAttacker1_Attack_Tag:
            return new AttackData(ceciliaController.crouchAttack1Damage, ceciliaController.crouchAttack1StunDuration, ceciliaController.transform.position);
        case Tags.CeciliaCrouchAttacker2_Attack_Tag:
            return new AttackData(ceciliaController.crouchAttack2Damage, ceciliaController.crouchAttack2StunDuration, ceciliaController.transform.position);
        default:
            return null;
    }
}

private class AttackData
{
    public float damage;
    public float stunDuration;
    public Vector3 position;

    public AttackData(float damage, float stunDuration, Vector3 position)
    {
        this.damage = damage;
        this.stunDuration = stunDuration;
        this.position = position;
    }
}


    private void ApplyStun(float duration)
{
    isStunned = true;
    isAttacking = true;
    stunTimer = duration;
    enemyAnim.Hurt(); // Trigger a stun animation if available
}

    // Activate and Deactivate attack points
    public void ActivatePunch1() => punch1AttackPoint.SetActive(true);
    public void ActivatePunch2() => punch2AttackPoint.SetActive(true);
    public void ActivateKick1() => kick1AttackPoint.SetActive(true);
    public void DeactivatePunch1() => punch1AttackPoint.SetActive(false);
    public void DeactivatePunch2() => punch2AttackPoint.SetActive(false);
    public void DeactivateKick1() => kick1AttackPoint.SetActive(false);
    public void DeactivateAllAttack() 
    {
        punch1AttackPoint.SetActive(false);
        punch2AttackPoint.SetActive(false);
        kick1AttackPoint.SetActive(false);
    }

    private void ApplyPushback(Vector3 attackerPosition, float attackForce)
{
    float pushDirection = transform.position.x > attackerPosition.x ? 1f : -1f;
    StartCoroutine(PushbackCoroutine(pushDirection, attackForce));
}



    private IEnumerator PushbackCoroutine(float direction, float force)
{
    float duration = 0.2f; // Duration of the pushback
    float timer = 0f;

    while (timer < duration)
    {
        transform.Translate(new Vector2(direction * force * Time.deltaTime, 0f));
        timer += Time.deltaTime;
        yield return null;
    }
}

public void ApplyForce(Vector3 force)
{
    Rigidbody2D rb = GetComponent<Rigidbody2D>(); // Assuming the enemy has a Rigidbody2D
    if (rb != null)
    {
        rb.AddForce(force, ForceMode2D.Impulse); // Apply the force to push the enemy back
    }
}
}
