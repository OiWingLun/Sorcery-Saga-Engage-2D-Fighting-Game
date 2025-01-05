using System.Collections;
using UnityEngine;


public class HaiyinController : MonoBehaviour
{
    private Rigidbody2D myRigidbody;
    private CharacterAnimation myAnim;
    private Health myHealth;

    [SerializeField]
    private string playerName;

    [SerializeField]
    private float movementSpeed;
    [SerializeField]
    private float forceJump;
    [SerializeField]
    private bool airControl;

    [SerializeField]
    private float jumpHorizontalSpeed;


  
    private bool facingRight;
    private bool isGrounded;
    public bool isJumping;
    public bool isCrouching;
    public bool isBlock;
    public bool isAttacking;
    public bool isDie;
    public bool jumpAttack1;
    public bool jumpAttack2;
    public bool jumpAttack3;
    public bool JumpAttacking;
    public bool crouchAttack1;
    public bool crouchAttack2;
    public bool isStunned = false;
    private float stunTimer = 0f;

    public float punch1Damage;
    public float kick1Damage;
    public float special1Damage;
    public float special2Damage;
    public float jumpAttack1Damage;
    public float jumpAttack2Damage;
    public float crouchAttack1Damage;
    public float crouchAttack2Damage;
    public float punch1StunDuration;
    public float kick1StunDuration;
    public float special1StunDuration;
    public float special2StunDuration;
    public float jumpAttack1StunDuration;
    public float jumpAttack2StunDuration;
    public float crouchAttack1StunDuration;
    public float crouchAttack2StunDuration;
    private EnemyController enemyController;
    private HaiyinAttack playerAttack;
    private Player2Aier player2Aier;
    private Player2Dark player2Dark;
    private Player2Cecilia player2Cecilia;
    private Player2Haiyin player2Haiyin;
    private bool isJumpInterrupted = false;
    

    [SerializeField]
    private BarStat healthBar;
    [SerializeField]
    private BarStat blockBar;
    public bool isPlayer1 = true;

    private void Awake()
    {
        playerAttack = GetComponent<HaiyinAttack>();
        myRigidbody = GetComponent<Rigidbody2D>();
        myAnim = GetComponent<CharacterAnimation>();
        myHealth = GetComponent<Health>();

        healthBar.bar = GameObject.FindGameObjectWithTag(Tags.Left_Health_Bar).GetComponent<BarScript>();
        healthBar.Initialize();
        blockBar.bar = GameObject.FindGameObjectWithTag(Tags.Left_Block_Bar).GetComponent<BarScript>();
        blockBar.Initialize();
    }

    private void Start()
    {
        healthBar.MaxVal = myHealth.maxHealth;
        blockBar.MaxVal = myHealth.maxBlock;
        enemyController = GameObject.FindGameObjectWithTag(Tags.Enemy_Tag).GetComponent<EnemyController>();
        player2Aier = GameObject.FindGameObjectWithTag(Tags.Enemy_Tag).GetComponent<Player2Aier>();
        player2Cecilia = GameObject.FindGameObjectWithTag(Tags.Enemy_Tag).GetComponent<Player2Cecilia>();
        player2Haiyin = GameObject.FindGameObjectWithTag(Tags.Enemy_Tag).GetComponent<Player2Haiyin>();
        player2Dark = GameObject.FindGameObjectWithTag(Tags.Enemy_Tag).GetComponent<Player2Dark>();
        facingRight = true;
        GameController.gameController.playerName.text = playerName;
        jumpAttack1 = false;
        jumpAttack2 = false;
        isDie = false;
    }

    private void Update()
    {
        if (isBlock && !isCrouching)
    {
        // Reduce block over time
        myHealth.ReduceBlockOverTime(5);

        // Update the block bar UI
        blockBar.CurrentVal = myHealth.block;

        // Stop blocking if the block value reaches 0
        if (myHealth.block <= 0)
        {
            StopBlocking();
        }
    }
    if (!Input.GetKey(KeyCode.I))
        {
            myHealth.RegenerateBlock(0.01f);
            blockBar.CurrentVal = myHealth.block;
        }
         if (isStunned)
    {
        // Decrement the stun timer
        stunTimer -= Time.deltaTime;

        // If stun duration is over, reset stun state
        if (stunTimer <= 0)
        {
            isStunned = false;
            isAttacking = false;

            if (Input.GetKey(KeyCode.I))
        {
            isBlock = true;
            StartBlocking();
        }
        }
    }
        healthBar.CurrentVal = myHealth.health;
        blockBar.CurrentVal = myHealth.block;
        HandleUserInput();
        CheckForDeath();
        FlipCharacter();
        GameController.gameController.playerHealth = myHealth.health;
    }

    private void FixedUpdate()
{
    if (isAttacking && !jumpAttack1 && !jumpAttack2|| playerAttack.isAttacking || isBlock)
{
    myRigidbody.velocity = Vector2.zero;
    return;
}

    float horizontal = Input.GetAxis("Horizontal");

    if (!isCrouching)
    {
        HandleMovement(horizontal);
        FlipCharacter();
    }
    else
    {
        myRigidbody.velocity = new Vector2(0, myRigidbody.velocity.y);
    }
}



    private void HandleMovement(float horizontal)
{
    if (GameController.gameController.isGameFinished)
        return;

    // Prevent movement while attacking or blocking
    if (isAttacking || playerAttack.isAttacking || isBlock)
    {
        myRigidbody.velocity = Vector2.zero;
        return;
    }

    // Allow movement only if grounded or air control is enabled
    if (isGrounded || airControl)
    {
        if (!isJumping)
        {
            myRigidbody.velocity = new Vector2(horizontal * movementSpeed, myRigidbody.velocity.y);
        }
    }

    // Update animations and facing direction
    if (!isJumping)
    {
        myAnim.Walk(Mathf.Abs(horizontal));
    }

    if (jumpAttack1 && !isGrounded)
    {
        myAnim.JumpAttack1(true);
    }
    if (!jumpAttack1)
    {
        myAnim.JumpAttack1(false);
    }
    if (jumpAttack2 && !isGrounded)
    {
        myAnim.JumpAttack2(true);
    }
    if (!jumpAttack2)
    {
        myAnim.JumpAttack2(false);
    }
}


    private void FlipCharacter()
{
    if (GameController.gameController.isGameFinished || isAttacking)
        return;

    // Get the player's position and the X positions of the enemies
    float playerX = transform.position.x;

    // Check if the enemies are present before trying to access their positions
    float[] enemyXPositions = new float[5];
    int validEnemyCount = 0;

    // Check each enemy and add their X position if they are present
    if (enemyController != null)
    {
        enemyXPositions[validEnemyCount++] = enemyController.transform.position.x;
    }
    if (player2Aier != null)
    {
        enemyXPositions[validEnemyCount++] = player2Aier.transform.position.x;
    }
    if (player2Cecilia != null)
    {
        enemyXPositions[validEnemyCount++] = player2Cecilia.transform.position.x;
    }
    if (player2Haiyin != null)
    {
        enemyXPositions[validEnemyCount++] = player2Haiyin.transform.position.x;
    }
    if (player2Dark != null) // Replace with the actual reference if needed
    {
        enemyXPositions[validEnemyCount++] = player2Dark.transform.position.x;
    }

    if (validEnemyCount == 0) return; // No enemies to compare, exit early

    // Find the closest enemy by comparing X positions
    float minDistance = Mathf.Infinity;
    float closestEnemyX = 0;

    for (int i = 0; i < validEnemyCount; i++)
    {
        float distance = Mathf.Abs(enemyXPositions[i] - playerX);
        if (distance < minDistance)
        {
            minDistance = distance;
            closestEnemyX = enemyXPositions[i];
        }
    }

    // Check if the player should be facing left or right based on the closest enemy's X position
    if (closestEnemyX > playerX && !facingRight)
    {
        facingRight = true;
        Vector3 theScale = transform.localScale;
        theScale.x = Mathf.Abs(theScale.x); // Flip the character to face right
        transform.localScale = theScale;
    }
    else if (closestEnemyX < playerX && facingRight)
    {
        facingRight = false;
        Vector3 theScale = transform.localScale;
        theScale.x = -Mathf.Abs(theScale.x); // Flip the character to face left
        transform.localScale = theScale;
    }
}

    private void CheckForDeath()
    {
        if (isDie) return; // Prevent redundant checks after death

    if (myHealth.health <= 0)
    {
        isAttacking = true;
        isDie = true;
        myAnim.Die(isDie);

        myRigidbody.velocity = Vector2.zero;
        myRigidbody.isKinematic = true;

        CapsuleCollider2D collider = GetComponent<CapsuleCollider2D>();
        if (collider != null)
        {
            collider.enabled = false;
        }

        // Notify the game controller
        GameController.gameController.isPlayerDead = isDie;
    }
    }

    private void HandleUserInput()
{
    if (GameController.gameController.isGameFinished || isAttacking || playerAttack.isAttacking )
        return; // Prevent any input if the player is attacking or jumping

    if (Input.GetKeyDown(KeyCode.W) && isGrounded && !isBlock && !isAttacking)
    {
        Jump();
    }

    if (Input.GetKeyDown(KeyCode.S) && !isAttacking && !isJumping)
    {
        StartCrouch();
    }

    if (Input.GetKeyUp(KeyCode.S) && isCrouching && !isAttacking && !isJumping)
    {
        StopCrouch();
    }

    if (Input.GetKeyDown(KeyCode.I) && !isAttacking)
    {
        StartBlocking();
    }

    if (Input.GetKeyUp(KeyCode.I) && !isAttacking)
    {
        StopBlocking();
    }

    if (Input.GetKeyDown(KeyCode.K) && !Input.GetKey(KeyCode.D) && !isAttacking && !isCrouching)
    {
        Kick1();
    }

    if (Input.GetKeyDown(KeyCode.K) && !isCrouching)
{
    // Trigger special move when facing right and pressing D
    if (facingRight && Input.GetKey(KeyCode.D))
    {
        Kick2();
    }
    // Trigger special move when facing left and pressing A
    else if (!facingRight && Input.GetKey(KeyCode.A))
    {
        Kick2();
    }
}

    if (Input.GetKeyDown(KeyCode.L) && !isCrouching && !isJumping && !Input.GetKey(KeyCode.D) && !Input.GetKey(KeyCode.A))
    {
        Special1();
    }

    if (Input.GetKeyDown(KeyCode.L) && !isCrouching)
{
    // Trigger special move when facing right and pressing D
    if (facingRight && Input.GetKey(KeyCode.D))
    {
        Special2();
    }
    // Trigger special move when facing left and pressing A
    else if (!facingRight && Input.GetKey(KeyCode.A))
    {
        Special2();
    }
}

    if (Input.GetKeyDown(KeyCode.K) && !isGrounded && !isAttacking)
    {
        JumpAttack1();
    }

    if (Input.GetKeyDown(KeyCode.L) && !isGrounded && !isAttacking)
    {
        JumpAttack2();
    }

    if (Input.GetKeyDown(KeyCode.J) && Input.GetKey(KeyCode.S) && !isAttacking && !crouchAttack1)
    {
        CrouchAttack1();  // Perform the crouch attack
    }

    if (Input.GetKeyDown(KeyCode.K) && Input.GetKey(KeyCode.S) && !isAttacking && !crouchAttack2)
    {
        CrouchAttack2();  // Perform the crouch attack
    }
}


    private void Jump()
{
    if (!isGrounded || isJumpInterrupted) return; 

    // Apply vertical force for the jump
    myRigidbody.AddForce(new Vector2(0, forceJump), ForceMode2D.Impulse);

    // Set initial horizontal velocity
    if (Input.GetAxis("Horizontal") > 0) // Jump forward
    {
        myRigidbody.velocity = new Vector2(jumpHorizontalSpeed, myRigidbody.velocity.y);
    }
    else if (Input.GetAxis("Horizontal") < 0) // Jump backward
    {
        myRigidbody.velocity = new Vector2(-jumpHorizontalSpeed, myRigidbody.velocity.y);
    }
    else // Jump straight up
    {
        myRigidbody.velocity = new Vector2(0, myRigidbody.velocity.y);
    }

    // Trigger jump animation
    myAnim.Jump(true);
    isJumping = true;  // Mark the player as jumping
    isGrounded = false;  // Player is no longer grounded

    // Start the reverse U jump coroutine
    StartCoroutine(ReverseUJump());
}

// Coroutine to simulate reverse U shape jump with faster speed
private IEnumerator ReverseUJump()
{
    // Store the initial horizontal velocity
    float initialHorizontalSpeed = myRigidbody.velocity.x;

    // Wait until the player has reached the peak of the jump (vertical velocity is near 0)
    while (myRigidbody.velocity.y > 0)
    {
        if (isJumpInterrupted)
        {
            yield break; // Exit the coroutine if the jump is interrupted
        }
        yield return null;
    }

    // At the peak, start curving backward more quickly
    float peakHeight = myRigidbody.velocity.y;
    while (myRigidbody.velocity.y < 0) // As the player starts descending
    {
        if (isJumpInterrupted)
        {
            yield break; // Exit the coroutine if the jump is interrupted
        }

        // Gradually reverse the horizontal direction to simulate the reverse U turn faster
        float curveFactor = Mathf.InverseLerp(peakHeight, 0, myRigidbody.velocity.y); // Smoothly interpolate
        float newHorizontalSpeed = Mathf.Lerp(initialHorizontalSpeed, -jumpHorizontalSpeed, curveFactor * 2);

        // Apply the horizontal speed with collision handling
        myRigidbody.velocity = new Vector2(newHorizontalSpeed, myRigidbody.velocity.y);

        yield return null;  // Wait for the next frame
    }
}

    private void Kick1()
{
    if (playerAttack.isAttacking || isJumping) 
        return;

    isAttacking = true;
    playerAttack.isAttacking = true; 
    myRigidbody.velocity = Vector2.zero;
    myAnim.Kick1();
    StartCoroutine(EndAttackAfterDelay(0.8f));
}

    private void Kick2()
{
    if (playerAttack.isAttacking || isJumping) 
        return;

    isAttacking = true;
    playerAttack.isAttacking = true; 
    myRigidbody.velocity = Vector2.zero;
    myAnim.Kick2();
    StartCoroutine(EndAttackAfterDelay(0.7f));
}
    private void Special1()
{
    if (playerAttack.isAttacking || isJumping) 
        return;

    isAttacking = true;
    playerAttack.isAttacking = true; 
    myRigidbody.velocity = Vector2.zero;
    myAnim.Special1();
    StartCoroutine(EndAttackAfterDelay(0.6f));
}

    private void Special2()
{
    if (playerAttack.isAttacking || isJumping) 
        return;

    isAttacking = true;
    playerAttack.isAttacking = true; 
    myRigidbody.velocity = Vector2.zero;
    myAnim.Special2();
    StartCoroutine(EndAttackAfterDelay(0.5f));
}

    private void JumpAttack1()
{
    if (!isJumping || JumpAttacking) return; 

    JumpAttacking = true;
    myAnim.Jump(false); 
    myAnim.JumpAttack1(true);  

    StartCoroutine(EndAttackAfterDelay(0.3f)); 
}



private void JumpAttack2()
{
    if (!isJumping || JumpAttacking) return;  

    JumpAttacking = true;
    myAnim.Jump(false); 
    myAnim.JumpAttack2(true);  

    StartCoroutine(EndAttackAfterDelay(0.4f)); 
}

    private IEnumerator EndAttackAfterDelay(float delay)
{
    yield return new WaitForSeconds(delay);
    isAttacking = false;
    playerAttack.isAttacking = false;
    crouchAttack1 = false; // Reset crouch attack flag
    isCrouching = false;
    crouchAttack2 = false;
    myAnim.CrouchAttack1(false);
    myAnim.CrouchAttack2(false);
    myAnim.JumpAttack1(false);
    myAnim.JumpAttack2(false);
    myAnim.JumpAttack3(false);

    if (isJumping)
    {
        myAnim.Jump(true); 
    }

    if (Input.GetKey(KeyCode.S))
    {
        isCrouching = true;
        myAnim.Crouch(true);
    }
    
}



private void CrouchAttack1()
{
    if (!isCrouching || isAttacking || crouchAttack1) return; // Ensure the player is crouching, not already attacking, and not already in a crouch attack

    crouchAttack1 = true; // Set the crouch attack flag
    isAttacking = true; // Prevent other attacks
    myAnim.Crouch(false);
    myAnim.CrouchAttack1(true); // Trigger crouch kick animation

    StartCoroutine(EndAttackAfterDelay(0.4f)); // Reset flags after attack delay
}

private void CrouchAttack2()
{
    if (!isCrouching || isAttacking || crouchAttack2) return; // Ensure the player is crouching, not already attacking, and not already in a crouch attack

    crouchAttack2 = true; // Set the crouch attack flag
    isAttacking = true; // Prevent other attacks
    myAnim.Crouch(false);
    myAnim.CrouchAttack2(true); // Trigger crouch kick animation

    StartCoroutine(EndAttackAfterDelay(0.5f)); // Reset flags after attack delay
}



    private void StartCrouch()
{
    if (GameController.gameController.isGameFinished)
        return;

    isCrouching = true;

    myAnim.Crouch(true); // Trigger crouch animation
}

   private void StopCrouch()
{
    if (GameController.gameController.isGameFinished)
        return;

    isCrouching = false;
    myAnim.Crouch(false); // Stop crouch animation
}


    private void StartBlocking()
    {
        if (isAttacking || isJumping || isCrouching)
        {
            return;
        }
        isBlock = true;
        myAnim.Block(isBlock);
    }

    private void StopBlocking()
    {
        if (isAttacking || isJumping || isCrouching)
        {
            return;
        }
        isBlock = false;
        myAnim.Block(isBlock);
    }

    private void OnTriggerEnter2D(Collider2D collision)
{
    // List of attack tags and their corresponding damage, stun, and pushback values
    AttackData attackData = GetAttackData(collision.tag);

    // If attack data is found and the player is not blocking
    if (attackData != null)
    {
        if (isBlock)
        {
            // Reduce block bar by 2% of its max value
            float blockReduction = myHealth.maxBlock * 0.04f;
            myHealth.block -= blockReduction;
            blockBar.CurrentVal = myHealth.block;

            // Play block sound
            AudioController.audioController.PlaySound("BLOCK");
        }
        else
        {
            // If not blocking, apply damage and other effects
            AudioController.audioController.PlaySound("HURT");
            myAnim.Hurt();
            myHealth.health -= attackData.damage;
            blockBar.CurrentVal = myHealth.health; // Update health bar if needed

            ApplyStun(attackData.stunDuration);
            ApplyPushback(attackData.position, attackData.damage);
        }
    }
}

private AttackData GetAttackData(string attackTag)
{
    // Match tag with the correct controller
    switch (attackTag)
    {
        case Tags.Punch_Attack_Tag:
            return new AttackData(enemyController.punch1Damage, enemyController.punch1StunDuration, enemyController.transform.position);
        case Tags.Kick_Attack_Tag:
            return new AttackData(enemyController.kick1Damage, enemyController.kick1StunDuration, enemyController.transform.position);
        case Tags.P2Punch_Attack_Tag:
            return new AttackData(player2Dark.punch1Damage, player2Dark.punch1StunDuration, player2Dark.transform.position);
        case Tags.P2Kick_Attack_Tag:
            return new AttackData(player2Dark.kick1Damage, player2Dark.kick1StunDuration, player2Dark.transform.position);
        case Tags.Special1_Attack_Tag:
            return new AttackData(player2Dark.special1Damage, player2Dark.special1StunDuration, player2Dark.transform.position);
        case Tags.Special2_Attack_Tag:
            return new AttackData(player2Dark.special2Damage, player2Dark.special2StunDuration, player2Dark.transform.position);
        case Tags.JumpAttack1_Attack_Tag:
            return new AttackData(player2Dark.jumpAttack1Damage, player2Dark.jumpAttack1StunDuration, player2Dark.transform.position);
        case Tags.JumpAttack2_Attack_Tag:
            return new AttackData(player2Dark.jumpAttack2Damage, player2Dark.jumpAttack2StunDuration, player2Dark.transform.position);
        case Tags.CrouchAttack1_Attack_Tag:
            return new AttackData(player2Dark.crouchAttack1Damage, player2Dark.crouchAttack1StunDuration, player2Dark.transform.position);
        case Tags.CrouchAttack2_Attack_Tag:
            return new AttackData(player2Dark.crouchAttack2Damage, player2Dark.crouchAttack2StunDuration, player2Dark.transform.position);
        case Tags.SwaySword1_Attack_Tag:
            return new AttackData(player2Dark.swaysword1Damage, player2Dark.swaysword1StunDuration, player2Dark.transform.position);
        case Tags.SwaySword2_Attack_Tag:
            return new AttackData(player2Dark.swaysword2Damage, player2Dark.swaysword2StunDuration, player2Dark.transform.position);
        case Tags.AierPunch_Attack_Tag:
            return new AttackData(player2Aier.punch1Damage, player2Aier.punch1StunDuration, player2Aier.transform.position);
        case Tags.AierKick_Attack_Tag:
            return new AttackData(player2Aier.kick1Damage, player2Aier.kick1StunDuration, player2Aier.transform.position);
        case Tags.AierSpecial1_Attack_Tag:
            return new AttackData(player2Aier.special1Damage, player2Aier.special1StunDuration, player2Aier.transform.position);
        case Tags.AierSpecial2_Attack_Tag:
            return new AttackData(player2Aier.special2Damage, player2Aier.special2StunDuration, player2Aier.transform.position);
        case Tags.AierJumpAttack1_Attack_Tag:
            return new AttackData(player2Aier.jumpAttack1Damage, player2Aier.jumpAttack1StunDuration, player2Aier.transform.position);
        case Tags.AierJumpAttack2_Attack_Tag:
            return new AttackData(player2Aier.jumpAttack2Damage, player2Aier.jumpAttack2StunDuration, player2Aier.transform.position);
        case Tags.AierCrouchAttacker1_Attack_Tag:
            return new AttackData(player2Aier.crouchAttack1Damage, player2Aier.crouchAttack1StunDuration, player2Aier.transform.position);
        case Tags.AierCrouchAttacker2_Attack_Tag:
            return new AttackData(player2Aier.crouchAttack2Damage, player2Aier.crouchAttack2StunDuration, player2Aier.transform.position);
        case Tags.HaiyinPunch_Attack_Tag:
            return new AttackData(player2Haiyin.punch1Damage, player2Haiyin.punch1StunDuration, player2Haiyin.transform.position);
        case Tags.HaiyinKick_Attack_Tag:
            return new AttackData(player2Haiyin.kick1Damage, player2Haiyin.kick1StunDuration, player2Haiyin.transform.position);
        case Tags.HaiyinSpecial1_Attack_Tag:
            return new AttackData(player2Haiyin.special1Damage, player2Haiyin.special1StunDuration, player2Haiyin.transform.position);
        case Tags.HaiyinSpecial2_Attack_Tag:
            return new AttackData(player2Haiyin.special2Damage, player2Haiyin.special2StunDuration, player2Haiyin.transform.position);
        case Tags.HaiyinJumpAttack1_Attack_Tag:
            return new AttackData(player2Haiyin.jumpAttack1Damage, player2Haiyin.jumpAttack1StunDuration, player2Haiyin.transform.position);
        case Tags.HaiyinJumpAttack2_Attack_Tag:
            return new AttackData(player2Haiyin.jumpAttack2Damage, player2Haiyin.jumpAttack2StunDuration, player2Haiyin.transform.position);
        case Tags.HaiyinCrouchAttacker1_Attack_Tag:
            return new AttackData(player2Haiyin.crouchAttack1Damage, player2Haiyin.crouchAttack1StunDuration, player2Haiyin.transform.position);
        case Tags.HaiyinCrouchAttacker2_Attack_Tag:
            return new AttackData(player2Haiyin.crouchAttack2Damage, player2Haiyin.crouchAttack2StunDuration, player2Haiyin.transform.position);
        case Tags.CeciliaPunch_Attack_Tag:
            return new AttackData(player2Cecilia.punch1Damage, player2Cecilia.punch1StunDuration, player2Cecilia.transform.position);
        case Tags.CeciliaKick_Attack_Tag:
            return new AttackData(player2Cecilia.kick1Damage, player2Cecilia.kick1StunDuration, player2Cecilia.transform.position);
        case Tags.CeciliaSpecial1_Attack_Tag:
            return new AttackData(player2Cecilia.special1Damage, player2Cecilia.special1StunDuration, player2Cecilia.transform.position);
        case Tags.CeciliaJumpAttack1_Attack_Tag:
            return new AttackData(player2Cecilia.jumpAttack1Damage, player2Cecilia.jumpAttack1StunDuration, player2Cecilia.transform.position);
        case Tags.CeciliaJumpAttack2_Attack_Tag:
            return new AttackData(player2Cecilia.jumpAttack2Damage, player2Cecilia.jumpAttack2StunDuration, player2Cecilia.transform.position);
        case Tags.CeciliaCrouchAttacker1_Attack_Tag:
            return new AttackData(player2Cecilia.crouchAttack1Damage, player2Cecilia.crouchAttack1StunDuration, player2Cecilia.transform.position);
        case Tags.CeciliaCrouchAttacker2_Attack_Tag:
            return new AttackData(player2Cecilia.crouchAttack2Damage, player2Cecilia.crouchAttack2StunDuration, player2Cecilia.transform.position);
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
    myAnim.Crouch(false);
    myAnim.Jump(false);
    myAnim.Hurt(); // Trigger a stun animation if available
}


    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Ground"))
        {
            isGrounded = false;
        }
    }

   private void OnCollisionEnter2D(Collision2D collision)
{
    if (collision.gameObject.CompareTag("Ground"))
    {
        isGrounded = true; // Player is now grounded
        isJumping = false; // Reset jump state
        jumpAttack1 = false; 
        JumpAttacking = false;
        jumpAttack2 = false;
        jumpAttack3 = false;
        isJumpInterrupted = false;

        myAnim.Jump(false); // End jump animation
    }
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

}


