using System.Collections;
using UnityEngine;

public enum Player2DarkComboState {
    None,
    Punch1,
    Punch2,
    Punch3,
    Punch4,
    Kick1,
    SwaySword1,
    SwaySword2
    
}

public class Player2DarkAttack : MonoBehaviour
{
    private CharacterAnimation myAnim;

    private bool activateTimeToReset;
    private float defaultComboTimer = 0.5f;
    private float currentComboTimer;
    private Player2DarkComboState currentComboState;
    private float punchCooldown = 0.1f; // Cooldown time between each punch
    private bool canPunch = true; 

    [SerializeField]
    private GameObject punch1AttackPoint;
    [SerializeField]
    private GameObject punch2AttackPoint;
    [SerializeField]
    private GameObject kick1AttackPoint;
    [SerializeField]
    private GameObject swaysword1AttackPoint;
    [SerializeField]
    private GameObject swaysword2AttackPoint;
    [SerializeField]
    private GameObject swaysword3AttackPoint;
    [SerializeField]
    private GameObject swaysword4AttackPoint;
    [SerializeField]
    private GameObject jumpAttack1AttackPoint;
    [SerializeField]
    private GameObject jumpAttack2AttackPoint;
    [SerializeField]
    private GameObject crouchAttack1AttackPoint;
    [SerializeField]
    private GameObject crouchAttack2AttackPoint;


    internal bool isAttacking;
    private Player2Dark player2Dark;
    private GameController gameController;

    private void Awake()
    {
        myAnim = GetComponent<CharacterAnimation>();
        player2Dark = GetComponent<Player2Dark>();
    }

    // Start is called before the first frame update
    void Start()
    {
        currentComboTimer = defaultComboTimer;
        currentComboState = Player2DarkComboState.None;
    }

    // Update is called once per frame
    void Update()
    {
        ComboAttack();
        ResetComboState();
    }

    void ComboAttack()
{
    // Handle Punch Combo (J Key)
    if (Input.GetKeyDown(KeyCode.Keypad4) && canPunch)
    {
        // Restrict punch combo if already at max or incompatible state
        if (currentComboState == Player2DarkComboState.Punch4 || player2Dark.isJumping || player2Dark.isCrouching || player2Dark.isAttacking || player2Dark.isStunned)
        {
            return;
        }

        isAttacking = true; // Prevent other actions during attack
        currentComboState++; // Advance combo state
        activateTimeToReset = true;
        currentComboTimer = defaultComboTimer;

        // Trigger animations based on combo state
        if (currentComboState == Player2DarkComboState.Punch1)
        {
            myAnim.Punch1();
        }
        else if (currentComboState == Player2DarkComboState.Punch2)
        {
            myAnim.Punch2();
        }

        StartCoroutine(HandlePunchCooldown());
    }

    // Handle Sword Combo (L Key)
    if (Input.GetKeyDown(KeyCode.Keypad6) && !isAttacking)
    {
        // Restrict sword combo if already at max or incompatible state
        if (currentComboState == Player2DarkComboState.SwaySword2 || player2Dark.isJumping || player2Dark.isCrouching || player2Dark.isAttacking)
        {
            return;
        }

        isAttacking = true; // Prevent other actions during attack
        currentComboState++; // Advance combo state
        activateTimeToReset = true;
        currentComboTimer = defaultComboTimer;

        // Trigger animations based on combo state
        if (currentComboState == Player2DarkComboState.SwaySword1)
        {
            myAnim.SwaySword3();
        }
        else if (currentComboState == Player2DarkComboState.SwaySword2)
        {
            myAnim.SwaySword4();
        }

        StartCoroutine(PerformSwaySwordCombo());
    }

    // Reset combo timer when active
    if (activateTimeToReset && currentComboTimer > 0)
    {
        currentComboTimer -= Time.deltaTime;
    }
    else if (activateTimeToReset && currentComboTimer <= 0)
    {
        ResetComboState();
    }
}

private IEnumerator PerformSwaySwordCombo()
{
    myAnim.SwaySword3();
    yield return new WaitForSeconds(0.5f); // Adjust delay for first attack

    myAnim.SwaySword4();
    yield return new WaitForSeconds(1f); // Adjust delay for second attack

    HandlePunchCooldown();
}

    private IEnumerator HandlePunchCooldown()
{
    canPunch = false;               // Disable punching
    yield return new WaitForSeconds(punchCooldown); // Wait for cooldown

    canPunch = true;                // Re-enable punching

    // If no combo is queued, reset isAttacking
    if (currentComboState == Player2DarkComboState.None)
    {
        isAttacking = false;
    }
}


    void ResetComboState() {
        if (activateTimeToReset) {
            currentComboTimer -= Time.deltaTime;

            if (currentComboTimer <= 0f) {
                currentComboState = Player2DarkComboState.None;
                isAttacking = false;
                activateTimeToReset = false;
                currentComboTimer = defaultComboTimer;
            }
        }
    }

    public void ActivatePunch1() {
        punch1AttackPoint.SetActive(true);
    }

    public void ActivatePunch2() {
        punch2AttackPoint.SetActive(true);
    }

    public void ActivateKick1() {
        kick1AttackPoint.SetActive(true);
    }

    public void ActivateSwaySword1() {
        swaysword1AttackPoint.SetActive(true);
    }

    public void ActivateSwaySword2() {
        swaysword2AttackPoint.SetActive(true);
    }

    public void ActivateSwaySword3() {
        swaysword3AttackPoint.SetActive(true);
    }

    public void ActivateSwaySword4() {
        swaysword4AttackPoint.SetActive(true);
    }

    public void ActivateJumpAttack1() {
        jumpAttack1AttackPoint.SetActive(true);
    }

    public void ActivateJumpAttack2() {
        jumpAttack2AttackPoint.SetActive(true);
    }

    public void ActivateCrouchAttack1() {
        crouchAttack1AttackPoint.SetActive(true);
    }

    public void ActivateCrouchAttack2() {
        crouchAttack2AttackPoint.SetActive(true);
    }

    public void DeactivatePunch1() {
        punch1AttackPoint.SetActive(false);
    }

    public void DeactivatePunch2() {
        punch2AttackPoint.SetActive(false);
    }

    public void DeactivateKick1() {
        kick1AttackPoint.SetActive(false);
    }

    public void DeactivateSwaySword1() {
        swaysword1AttackPoint.SetActive(false);
    }

    public void DeactivateSwaySword2() {
        swaysword2AttackPoint.SetActive(false);
    }

    public void DeactivateSwaySword3() {
        swaysword3AttackPoint.SetActive(false);
    }

    public void DeactivateSwaySword4() {
        swaysword4AttackPoint.SetActive(false);
    }

    public void DeactivateJumpAttack1() {
        jumpAttack1AttackPoint.SetActive(false);
    }

    public void DeactivateJumpAttack2() {
        jumpAttack2AttackPoint.SetActive(false);
    }

    public void DeactivateCrouchAttack1() {
        crouchAttack1AttackPoint.SetActive(false);
    }

    public void DeactivateCrouchAttack2() {
        crouchAttack2AttackPoint.SetActive(false);
    }


    

    public void DeactivateAllAttack() {
        punch1AttackPoint.SetActive(false);
        punch2AttackPoint.SetActive(false);
        kick1AttackPoint.SetActive(false);
        swaysword1AttackPoint.SetActive(false);
        swaysword2AttackPoint.SetActive(false);
        swaysword3AttackPoint.SetActive(false);
        jumpAttack1AttackPoint.SetActive(false);
        jumpAttack2AttackPoint.SetActive(false);
        swaysword4AttackPoint.SetActive(false);
        crouchAttack1AttackPoint.SetActive(false);  
        crouchAttack2AttackPoint.SetActive(false);
    }
}
