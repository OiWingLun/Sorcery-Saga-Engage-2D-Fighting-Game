using UnityEngine;
using TMPro;
using System.Collections.Generic;
using System.Collections;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour
{
    public static GameController gameController;

    [SerializeField]
    private TMP_Text timerText;
    [SerializeField]
    private float defaultGameTimer;
    [SerializeField]
    private float currentGameTimer;
    public TMP_Text playerName;
    public TMP_Text enemyName;
    public GameObject winPanel;
    public GameObject losePanel;
    public GameObject drawPanel;
    public int playerCode;
    public int enemyCode;
    public bool isGameFinished;
    public bool isEnemyDead;
    public bool isPlayerDead;
    public bool isEnemyWin;
    public bool isPlayerWin;
    public bool isDraw;
    public float playerHealth;
    public float enemyHealth;

    [SerializeField]
    private List<GameObject> playerCharacter;
    [SerializeField]
    private List<GameObject> enemyCharacter;

    public Vector3 playerSpawnPosition = new Vector3(-28f, -7f, 0f);
    public Vector3 enemySpawnPosition = new Vector3(28f, -7f, 0f);

    private void Awake()
    {
        if (gameController == null)
    {
        gameController = this;
    }

        playerCode = CodeOfCharacter.codeOfCharacter.playerCode;
        enemyCode = CodeOfCharacter.codeOfCharacter.enemyCode;
        SpawnCharacter(playerCode, enemyCode);
    }

    private void Start()
    {
        AudioController.audioController.StopAllSound();
        AudioController.audioController.PlayBGMSound("Stage1");
        isGameFinished = false;
        currentGameTimer = defaultGameTimer;

        playerHealth = 100f; 
        enemyHealth = 100f;  
    }

    private void Update()
    {  
        UpdateUI();
        CheckIsGameFinished();
    }
    void CheckIsGameFinished() {
        if (isGameFinished)
            return;

        currentGameTimer -= Time.deltaTime;
        if (currentGameTimer <= 0)
        {
            isGameFinished = true;
            if (playerHealth > enemyHealth)
            {
                isPlayerWin = true;
                StartCoroutine(GameEnded());
            }
            else if (playerHealth < enemyHealth)
            {
                isEnemyWin = true;
                StartCoroutine(GameEnded());
            }
            else if (playerHealth == enemyHealth)
            {
                isDraw = true;
                StartCoroutine(GameEnded());
            }
        }    
        if (isEnemyDead)
        {
            isGameFinished = true;
            isPlayerWin = true;
            StartCoroutine(GameEnded());
        }
        else if (isPlayerDead) {
            isGameFinished = true;
            isEnemyWin = true;
            StartCoroutine(GameEnded());
        }
    }    
    void UpdateUI()
    {
        timerText.text = currentGameTimer.ToString("0");
    }

    void SpawnCharacter(int code1, int code2)
    {
        Destroy(GameObject.FindWithTag("Player"));  
        Destroy(GameObject.FindWithTag("Enemy")); 
        Instantiate(playerCharacter[code1], playerSpawnPosition, Quaternion.identity);
        Instantiate(enemyCharacter[code2], enemySpawnPosition, Quaternion.identity);
    }
    public void GoToMenu() 
{
    foreach (GameObject obj in FindObjectsOfType<GameObject>())
    {
        Destroy(obj);
    }
    Time.timeScale = 1;  
    gameController = null; 
    SceneManager.LoadSceneAsync(1);
}

    public void Rematch()
{
    isGameFinished = false;
    currentGameTimer = defaultGameTimer;

    isPlayerDead = false;
    isPlayerWin = false;
    isEnemyDead = false;
    isEnemyWin = false;
    isDraw = false;
    PauseMenu.GameIsPaused = false;

    playerHealth = 100f;
    enemyHealth = 100f;

    winPanel.SetActive(false);
    losePanel.SetActive(false);
    drawPanel.SetActive(false);

    Time.timeScale = 1;

    SpawnCharacter(playerCode, enemyCode);
    UpdateUI();
}
    IEnumerator GameEnded()
    {
        yield return new WaitForSeconds(1.5f);

        Time.timeScale = 0;
        if (isPlayerWin) 
            winPanel.SetActive(true);
        else if (isEnemyWin) 
            losePanel.SetActive(true);   
        else if (isDraw) 
            drawPanel.SetActive(true);
    }
}
