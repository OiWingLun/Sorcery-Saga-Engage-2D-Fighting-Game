using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class CharacterSelect : MonoBehaviour
{
    [SerializeField] private GameObject selector;
    [SerializeField] private GameObject blueSelector;
    [SerializeField] private GameObject[] row1;
    [SerializeField] private TMP_Text player1NameText;
    [SerializeField] private TMP_Text player2NameText;
    [SerializeField] private Image player1PixelArtImage;
    [SerializeField] private Image player2PixelArtImage;
    [SerializeField] private Image player1VerticalImage;
    [SerializeField] private Image player2VerticalImage;
    [SerializeField] private TMP_Text player1DescriptionText;
    [SerializeField] private TMP_Text player2DescriptionText;

    private const int cols = 5;
    private const int randomButtonIndex = 2;
    private Vector2 player1PositionIndex;
    private Vector2 player2PositionIndex;
    private GameObject player1CurrentSlot;
    private GameObject player2CurrentSlot;
    private bool isPlayer1Selecting = true;
    private bool isMoving = false;

    private GameObject[,] grid = new GameObject[1, cols];

    private int playerCode = -1; // Selected character code for Player 1
    private int enemyCode = -1; // Selected character code for Player 2

    void Start()
    {
        AddRowToGrid(0, row1);

        // Initialize selectors and positions
        player1PositionIndex = new Vector2(0, 0);
        player1CurrentSlot = grid[0, 0];
        selector.transform.position = player1CurrentSlot.transform.position;

        blueSelector.SetActive(false);

        UpdateCharacterInfo(player1PositionIndex, player1NameText, player1VerticalImage, player1PixelArtImage, player1DescriptionText);

        player2PositionIndex = new Vector2(0, 0);
        player2CurrentSlot = grid[0, 0];
        UpdateCharacterInfo(player2PositionIndex, player2NameText, player2VerticalImage, player2PixelArtImage, player2DescriptionText);
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
        if (isPlayer1Selecting)
        {
            HandlePlayerInput(ref player1PositionIndex, selector, player1NameText, player1VerticalImage, player1PixelArtImage, player1DescriptionText, true);
        }
        else
        {
            HandlePlayerInput(ref player2PositionIndex, blueSelector, player2NameText, player2VerticalImage, player2PixelArtImage, player2DescriptionText, false);
        }
    }

    private void HandlePlayerInput(ref Vector2 positionIndex, GameObject selectorObject, TMP_Text nameText, Image verticalImage, Image pixelArtImage, TMP_Text descriptionText, bool isPlayer1)
    {
        float xAxis = Input.GetAxisRaw("Horizontal");

        if (xAxis > 0) MoveSelector(ref positionIndex, "right", selectorObject);
        else if (xAxis < 0) MoveSelector(ref positionIndex, "left", selectorObject);

        if (Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.Mouse0))
        {
            HandleRandomSelection(ref positionIndex, nameText, verticalImage, pixelArtImage, descriptionText);

            if (isPlayer1)
            {
                playerCode = (int)positionIndex.x; // Assign Player 1's selection
                isPlayer1Selecting = false; // Switch to Player 2
                blueSelector.SetActive(true);
                blueSelector.transform.position = grid[0, (int)player2PositionIndex.x].transform.position;
            }
            else
            {
                enemyCode = (int)positionIndex.x; // Assign Player 2's selection

                // Proceed to the next scene after both have chosen
                Debug.Log("Player 1 Code: " + playerCode + ", Player 2 Code: " + enemyCode);
                CodeOfCharacter.codeOfCharacter.playerCode = playerCode;
                CodeOfCharacter.codeOfCharacter.enemyCode = enemyCode;
                SceneManager.LoadSceneAsync(4); // Replace 4 with your arena scene index or name
            }
        }
    }

    private void MoveSelector(ref Vector2 positionIndex, string direction, GameObject selectorObject)
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

        GameObject currentSlot = grid[0, (int)positionIndex.x];
        selectorObject.transform.position = currentSlot.transform.position;

        // Update the character info
        if (selectorObject == selector)
        {
            UpdateCharacterInfo(positionIndex, player1NameText, player1VerticalImage, player1PixelArtImage, player1DescriptionText);
        }
        else if (selectorObject == blueSelector)
        {
            UpdateCharacterInfo(positionIndex, player2NameText, player2VerticalImage, player2PixelArtImage, player2DescriptionText);
        }

        Invoke(nameof(ResetMoving), 0.2f);
    }
}


    private void HandleRandomSelection(ref Vector2 positionIndex, TMP_Text nameText, Image verticalImage, Image pixelArtImage, TMP_Text descriptionText)
    {
        if ((int)positionIndex.x == randomButtonIndex)
        {
            int randomIndex;
            do
            {
                randomIndex = Random.Range(0, row1.Length);
            } while (randomIndex == randomButtonIndex);

            positionIndex.x = randomIndex;
            UpdateCharacterInfo(positionIndex, nameText, verticalImage, pixelArtImage, descriptionText);
        }
    }

    private void UpdateCharacterInfo(Vector2 index, TMP_Text nameText, Image verticalImage, Image pixelArtImage, TMP_Text descriptionText)
    {
        CharacterSelectItemScript characterScript = row1[(int)index.x].GetComponent<CharacterSelectItemScript>();
        if (characterScript != null)
        {
            nameText.text = characterScript.CharacterName;
            verticalImage.sprite = characterScript.VerticalImage;
            pixelArtImage.sprite = characterScript.PixelArtImage;
            descriptionText.text = characterScript.Description;
        }
    }

    private void ResetMoving()
    {
        isMoving = false;
    }
}
