using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class StageSelect : MonoBehaviour
{
    [SerializeField] private GameObject selector; // The selector UI object
    [SerializeField] private GameObject[] row1; // Row with 5 stage buttons
    [SerializeField] private Image stageImage; // UI element to display stage image
    [SerializeField] private TMP_Text stageNameText; // UI element to display stage name
    [SerializeField] private GameObject[] player1Prefabs; // Array of player1 prefabs (Control1, Control2)
    [SerializeField] private GameObject[] player2Prefabs; // Array of player2 prefabs (Control1, Control2)

    private const int cols = 5; // Number of columns (1 row, 5 stages)
    private Vector2 positionIndex; // Current position in the grid
    private GameObject currentSlot; // Currently selected slot
    private bool isMoving = false; // Prevent rapid movement

    private GameObject[,] grid = new GameObject[1, cols]; // 1x5 grid
    private int currentIndex;

    void Start()
    {
        // Initialize the grid
        AddRowToGrid(0, row1);

        // Start at the first stage
        positionIndex = new Vector2(0, 0);
        currentSlot = grid[0, 0];
        selector.transform.position = currentSlot.transform.position;

        // Update the stage info display for the first stage
        UpdateStageInfo(0);
    }

    private void AddRowToGrid(int rowIndex, GameObject[] row)
    {
        for (int i = 0; i < row.Length; i++)
        {
            grid[rowIndex, i] = row[i];
        }
    }

    void Update()
    {
        float xAxis = Input.GetAxisRaw("Horizontal");

        if (xAxis > 0)
        {
            MoveSelector("right");
        }
        else if (xAxis < 0)
        {
            MoveSelector("left");
        }

        if (Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.Mouse0))
        {
            SelectStage();
        }
    }

    private void MoveSelector(string direction)
    {
        if (!isMoving)
        {
            isMoving = true;

            if (direction == "right" && positionIndex.x < cols - 1)
            {
                positionIndex.x += 1;
            }
            else if (direction == "left" && positionIndex.x > 0)
            {
                positionIndex.x -= 1;
            }

            currentSlot = grid[0, (int)positionIndex.x];
            selector.transform.position = currentSlot.transform.position;

            UpdateStageInfo((int)positionIndex.x);

            Invoke(nameof(ResetMoving), 0.2f);
        }
    }

    private void UpdateStageInfo(int index)
    {
        LevelSelectItemScript levelScript = row1[index].GetComponent<LevelSelectItemScript>();
        if (levelScript != null)
        {
            stageImage.sprite = levelScript.StageImage; // Assign the stage image
            stageNameText.text = levelScript.StageName; // Assign the stage name
        }
    }

    private void SelectStage()
    {
        LevelSelectItemScript levelScript = currentSlot.GetComponent<LevelSelectItemScript>();
        currentIndex = (int)positionIndex.x;

        if (currentIndex == 2) // Random Button index
        {
            SelectRandomStage();
        }
        else if (levelScript != null)
        {
            SceneManager.LoadScene(levelScript.LevelID); // Load the stage's scene
            
        }
    }

    public void SelectRandomStage()
    {
        int randomIndex;

        // Ensure the random button is excluded
        do
        {
            randomIndex = Random.Range(0, row1.Length);
        } while (randomIndex == 2); // Exclude the random button itself

        // Select the random stage
        positionIndex = new Vector2(randomIndex, 0);
        currentSlot = row1[randomIndex];
        selector.transform.position = currentSlot.transform.position;

        UpdateStageInfo(randomIndex);

        // Automatically load the selected stage
        LevelSelectItemScript levelScript = currentSlot.GetComponent<LevelSelectItemScript>();
        if (levelScript != null && !string.IsNullOrEmpty(levelScript.LevelID))
        {
            SceneManager.LoadScene(levelScript.LevelID);
        }
        else
        {
            Debug.LogError("Invalid or missing LevelID for the random stage!");
        }
    }

    private void ResetMoving()
    {
        isMoving = false;
    }
}

