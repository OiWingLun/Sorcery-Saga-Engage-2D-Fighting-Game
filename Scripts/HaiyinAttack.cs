using System.Collections;
using UnityEngine;

public enum HaiyinComboState {
    None,
    Punch1,
    Punch2,
    Kick1
    
}

public class HaiyinAttack : MonoBehaviour
{
    private CharacterAnimation myAnim;

    private bool activateTimeToReset;
    private float defaultComboTimer = 0.5f;
    private float currentComboTimer;
    private ComboState currentComboState;
    private float punchCooldown = 0.1f; // Cooldown time between each punch
    private bool canPunch = true; 

    [SerializeField]
    private GameObject punch1AttackPoint;
    [SerializeField]
    private GameObject kick1AttackPoint;
    [SerializeField]
    private GameObject kick2AttackPoint;
    [SerializeField]
    private GameObject swaysword1AttackPoint;
    [SerializeField]
    private GameObject swaysword2AttackPoint;
    [SerializeField]
    private GameObject jumpAttack1AttackPoint;
    [SerializeField]
    private GameObject jumpAttack2AttackPoint;
    [SerializeField]
    private GameObject crouchAttack1AttackPoint;
    [SerializeField]
    private GameObject crouchAttack2AttackPoint;
    


    internal bool isAttacking;
    private HaiyinController haiyincontroller;
    private GameController gameController;

    private void Awake()
    {
        myAnim = GetComponent<CharacterAnimation>();
        haiyincontroller = GetComponent<HaiyinController>();
    }

    // Start is called before the first frame update
    void Start()
    {
        currentComboTimer = defaultComboTimer;
        currentComboState = ComboState.None;
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
    if (Input.GetKeyDown(KeyCode.J) && canPunch)
    {
        // Restrict punch combo if already at max or incompatible state
        if (currentComboState == ComboState.Punch2 || haiyincontroller.isJumping || haiyincontroller.isCrouching || haiyincontroller.isAttacking || haiyincontroller.isStunned)
        {
            return;
        }

        isAttacking = true; // Prevent other actions during attack
        currentComboState++; // Advance combo state
        activateTimeToReset = true;
        currentComboTimer = defaultComboTimer;

        // Trigger animations based on combo state
        if (currentComboState == ComboState.Punch1)
        {
            myAnim.Punch1();
        }
        else if (currentComboState == ComboState.Punch2)
        {
            myAnim.Punch1();
        }

        StartCoroutine(HandlePunchCooldown());
    }
}


    private IEnumerator HandlePunchCooldown()
{
    canPunch = false;               // Disable punching
    yield return new WaitForSeconds(punchCooldown); // Wait for cooldown

    canPunch = true;                // Re-enable punching

    // If no combo is queued, reset isAttacking
    if (currentComboState == ComboState.None)
    {
        isAttacking = false;
    }
}


    void ResetComboState() {
        if (activateTimeToReset) {
            currentComboTimer -= Time.deltaTime;

            if (currentComboTimer <= 0f) {
                currentComboState = ComboState.None;
                isAttacking = false;
                activateTimeToReset = false;
                currentComboTimer = defaultComboTimer;
            }
        }
    }

    public void ActivatePunch1() {
        punch1AttackPoint.SetActive(true);
    }

    public void ActivateKick1() {
        kick1AttackPoint.SetActive(true);
    }
    
    public void ActivateKick2() {
        kick2AttackPoint.SetActive(true);
    }

    public void ActivateSwaySword1() {
        swaysword1AttackPoint.SetActive(true);
    }

    public void ActivateSwaySword2() {
        swaysword2AttackPoint.SetActive(true);
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

    public void DeactivateKick1() {
        kick1AttackPoint.SetActive(false);
    }

    public void DeactivateKick2() {
        kick2AttackPoint.SetActive(false);
    }

    public void DeactivateSwaySword1() {
        swaysword1AttackPoint.SetActive(false);
    }

    public void DeactivateSwaySword2() {
        swaysword2AttackPoint.SetActive(false);
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
        kick1AttackPoint.SetActive(false);
        swaysword1AttackPoint.SetActive(false);
        swaysword2AttackPoint.SetActive(false);
        jumpAttack1AttackPoint.SetActive(false);
        jumpAttack2AttackPoint.SetActive(false);  
        crouchAttack1AttackPoint.SetActive(false);
        crouchAttack2AttackPoint.SetActive(false);
    }
}
