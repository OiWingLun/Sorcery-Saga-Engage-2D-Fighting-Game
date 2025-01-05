using System.Collections;
using UnityEngine;


public class Player2Haiyin : MonoBehaviour
{
    private Rigidbody2D myRigidbody;
    private CharacterAnimation myAnim;
    private Health myHealth;

    [SerializeField]
    private string enemyName;

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
    private Player2HaiyinAttack playerAttack;

    private PlayerController playerController;
    private AierController aierController;
    private HaiyinController haiyinController;
    private CeciliaController ceciliaController;
    private bool isJumpInterrupted = false;
    

    [SerializeField]
    private BarStat healthBar;
    [SerializeField]
    private BarStat blockBar;
    public bool isPlayer1 = false;

    private void Awake()
    {
        playerAttack = GetComponent<Player2HaiyinAttack>();
        myRigidbody = GetComponent<Rigidbody2D>();
        myAnim = GetComponent<CharacterAnimation>();
        myHealth = GetComponent<Health>();

        healthBar.bar = GameObject.FindGameObjectWithTag(Tags.Right_Health_Bar).GetComponent<BarScript>();
        healthBar.Initialize();
        blockBar.bar = GameObject.FindGameObjectWithTag(Tags.Right_Block_Bar).GetComponent<BarScript>();
        blockBar.Initialize();
    }

    private void Start()
    {
        healthBar.MaxVal = myHealth.maxHealth;
        blockBar.MaxVal = myHealth.maxBlock;
        playerController = GameObject.FindGameObjectWithTag(Tags.Player_Tag).GetComponent<PlayerController>();
        aierController = GameObject.FindGameObjectWithTag(Tags.Player_Tag).GetComponent<AierController>();
        haiyinController = GameObject.FindGameObjectWithTag(Tags.Player_Tag).GetComponent<HaiyinController>();
        ceciliaController = GameObject.FindGameObjectWithTag(Tags.Player_Tag).GetComponent<CeciliaController>();
        facingRight = true;
        GameController.gameController.enemyName.text = enemyName;
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
    if (!Input.GetKey(KeyCode.Keypad8))
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

            if (Input.GetKey(KeyCode.Keypad8))
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
        GameController.gameController.enemyHealth = myHealth.health;
    }

    private void FixedUpdate()
{
    if (isAttacking && !jumpAttack1 && !jumpAttack2|| playerAttack.isAttacking || isBlock)
{
    myRigidbody.velocity = Vector2.zero;
    return;
}

    float horizontal = Input.GetAxis("Horizontal2P");

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
    float[] enemyXPositions = new float[4];
    int validEnemyCount = 0;

    // Check each enemy and add their X position if they are present
    if (aierController != null)
    {
        enemyXPositions[validEnemyCount++] = aierController.transform.position.x;
    }
    if (ceciliaController != null)
    {
        enemyXPositions[validEnemyCount++] = ceciliaController.transform.position.x;
    }
    if (haiyinController != null)
    {
        enemyXPositions[validEnemyCount++] = haiyinController.transform.position.x;
    }
    if (playerController != null) // Replace with the actual reference if needed
    {
        enemyXPositions[validEnemyCount++] = playerController.transform.position.x;
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
        GameController.gameController.isEnemyDead = isDie;
    }
    }

    private void HandleUserInput()
{
    if (GameController.gameController.isGameFinished || isAttacking || playerAttack.isAttacking )
        return; // Prevent any input if the player is attacking or jumping

    if (Input.GetKeyDown(KeyCode.UpArrow) && isGrounded && !isBlock && !isAttacking)
    {
        Jump();
    }

    if (Input.GetKeyDown(KeyCode.DownArrow) && !isAttacking && !isJumping)
    {
        StartCrouch();
    }

    if (Input.GetKeyUp(KeyCode.DownArrow) && isCrouching && !isAttacking && !isJumping)
    {
        StopCrouch();
    }

    if (Input.GetKeyDown(KeyCode.Keypad8) && !isAttacking)
    {
        StartBlocking();
    }

    if (Input.GetKeyUp(KeyCode.Keypad8) && !isAttacking)
    {
        StopBlocking();
    }

    if (Input.GetKeyDown(KeyCode.Keypad5) && !Input.GetKey(KeyCode.D) && !isAttacking && !isCrouching)
    {
        Kick1();
    }

    if (Input.GetKeyDown(KeyCode.Keypad6) && !isCrouching)
{
    // Trigger special move when facing right and pressing D
    if (facingRight && Input.GetKey(KeyCode.RightArrow))
    {
        Kick2();
    }
    // Trigger special move when facing left and pressing A
    else if (!facingRight && Input.GetKey(KeyCode.LeftArrow))
    {
        Kick2();
    }
}

    if (Input.GetKeyDown(KeyCode.Keypad6) && !isCrouching && !isJumping)
    {
        Special1();
    }

    if (Input.GetKeyDown(KeyCode.Keypad5) && !isGrounded && !isAttacking)
    {
        JumpAttack1();
    }

    if (Input.GetKeyDown(KeyCode.Keypad6) && !isGrounded && !isAttacking)
    {
        JumpAttack2();
    }

    if (Input.GetKeyDown(KeyCode.Keypad4) && Input.GetKey(KeyCode.DownArrow) && !isAttacking && !crouchAttack1)
    {
        CrouchAttack1();  // Perform the crouch attack
    }

    if (Input.GetKeyDown(KeyCode.Keypad5) && Input.GetKey(KeyCode.DownArrow) && !isAttacking && !crouchAttack2)
    {
        CrouchAttack2();  // Perform the crouch attack
    }
}


    private void Jump()
{
    if (!isGrounded || isJumpInterrupted) return; // Don't allow jumping if not grounded or already interrupted

    // Apply vertical force for the jump
    myRigidbody.AddForce(new Vector2(0, forceJump), ForceMode2D.Impulse);

    // Set initial horizontal velocity
    if (Input.GetAxis("Horizontal2P") > 0) // Jump forward
    {
        myRigidbody.velocity = new Vector2(jumpHorizontalSpeed, myRigidbody.velocity.y);
    }
    else if (Input.GetAxis("Horizontal2P") < 0) // Jump backward
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
    StartCoroutine(EndAttackAfterDelay(0.7f));
}

    private void JumpAttack1()
{
    if (!isJumping || JumpAttacking) return; 

    JumpAttacking = true;
    myAnim.Jump(false); 
    myAnim.JumpAttack1(true);  

    StartCoroutine(EndAttackAfterDelay(0.5f)); 
}



private void JumpAttack2()
{
    if (!isJumping || JumpAttacking) return;  

    JumpAttacking = true;
    myAnim.Jump(false); 
    myAnim.JumpAttack2(true);  

    StartCoroutine(EndAttackAfterDelay(1f)); 
}

    private IEnumerator EndAttackAfterDelay(float delay)
{
    yield return new WaitForSeconds(delay);
    isAttacking = false;
    playerAttack.isAttacking = false;
    jumpAttack1 = false; 
    JumpAttacking = false;
    jumpAttack2 = false;
    jumpAttack3 = false;
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

    if (Input.GetKey(KeyCode.DownArrow))
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

    StartCoroutine(EndAttackAfterDelay(0.3f)); // Reset flags after attack delay
}

private void CrouchAttack2()
{
    if (!isCrouching || isAttacking || crouchAttack2) return; // Ensure the player is crouching, not already attacking, and not already in a crouch attack

    crouchAttack2 = true; // Set the crouch attack flag
    isAttacking = true; // Prevent other attacks
    myAnim.Crouch(false);
    myAnim.CrouchAttack2(true); // Trigger crouch kick animation

    StartCoroutine(EndAttackAfterDelay(0.1f)); // Reset flags after attack delay
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
        jumpAttack1 = false; // Reset jump attack flag when landing
        jumpAttack2 = false;
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


