using System.Collections;
using UnityEngine;

public enum AierComboState {
    None,
    Punch1,
    Punch2,
    Punch3,
    Punch4,
    Kick1
   
    
}

public class AierAttack : MonoBehaviour
{
    private CharacterAnimation myAnim;

    private bool activateTimeToReset;
    private float defaultComboTimer = 0.5f;
    private float currentComboTimer;
    private AierComboState currentComboState;
    private float punchCooldown = 0.1f; // Cooldown time between each punch
    private bool canPunch = true; 

    [SerializeField]
    private GameObject punch1AttackPoint;
    [SerializeField]
    private GameObject punch2AttackPoint;
    [SerializeField]
    private GameObject kick1AttackPoint;
    [SerializeField]
    private GameObject special1AttackPoint;
    [SerializeField]
    private GameObject special2AttackPoint;
    [SerializeField]
    private GameObject jumpAttack1AttackPoint;
    [SerializeField]
    private GameObject jumpAttack2AttackPoint;
    [SerializeField]
    private GameObject crouchAttack1AttackPoint;
    [SerializeField]
    private GameObject crouchAttack2AttackPoint;

    

    internal bool isAttacking;
    private AierController AierController;
    private GameController gameController;

    private void Awake()
    {
        myAnim = GetComponent<CharacterAnimation>();
        AierController = GetComponent<AierController>();
    }

    // Start is called before the first frame update
    void Start()
    {
        currentComboTimer = defaultComboTimer;
        currentComboState = AierComboState.None;
    }

    // Update is called once per frame
    void Update()
    {
        ComboAttack();
        ResetComboState();
    }

    void ComboAttack()
{
    // Block if punching is on cooldown, jumping, crouching, or already in Punch4
    if (!canPunch || AierController.isJumping || AierController.isCrouching || currentComboState == AierComboState.Punch4 || AierController.isStunned)
        return;

    // Check for input to progress combo
    if (Input.GetKeyDown(KeyCode.J))
    {
        // Only allow new combos when not attacking
        if (currentComboState == AierComboState.None && isAttacking)
            return;

        // Begin the combo or progress to the next state
        isAttacking = true; 
        currentComboState++;
        activateTimeToReset = true;
        currentComboTimer = defaultComboTimer;

        // Play the appropriate animation based on combo state
        switch (currentComboState)
        {
            case AierComboState.Punch1:
                myAnim.Punch1();
                break;
            case AierComboState.Punch2:
                myAnim.Punch2();
                break;
            case AierComboState.Punch3:
                myAnim.Punch1();
                break;
            case AierComboState.Punch4:
                myAnim.Punch2();
                break;
        }

        // Start the punch cooldown
        StartCoroutine(HandlePunchCooldown());
    }

    // Manage combo timer for resetting
    if (activateTimeToReset && currentComboTimer > 0)
    {
        currentComboTimer -= Time.deltaTime;
    }
    else if (activateTimeToReset && currentComboTimer <= 0)
    {
        ResetComboState();
    }
}

// Coroutine to handle cooldown between punches
private IEnumerator HandlePunchCooldown()
{
    canPunch = false; // Disable punching
    yield return new WaitForSeconds(punchCooldown); // Wait for cooldown
    canPunch = true;  // Re-enable punching
}

    void ResetComboState() {
        if (activateTimeToReset) {
            currentComboTimer -= Time.deltaTime;

            if (currentComboTimer <= 0f) {
                currentComboState = AierComboState.None;
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

    public void ActivateSpecial1() {
        special1AttackPoint.SetActive(true);
    }

    public void ActivateSpecial2() {
        special2AttackPoint.SetActive(true);
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

    public void DeactivateSpecial1() {
        special1AttackPoint.SetActive(false);
    }

    public void DeactivateSpecial2() {
        special2AttackPoint.SetActive(false);
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
        special1AttackPoint.SetActive(false);
        special2AttackPoint.SetActive(false);
        jumpAttack1AttackPoint.SetActive(false);
        jumpAttack2AttackPoint.SetActive(false);

    }
}
