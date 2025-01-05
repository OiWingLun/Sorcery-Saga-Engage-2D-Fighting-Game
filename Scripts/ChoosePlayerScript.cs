
using UnityEngine;
using UnityEngine.SceneManagement;

public class ChoosePlayerScript : MonoBehaviour
{
    public int playerCode;
    public int enemyCode;

    private bool isPlayer1Choosing = true; 

    void Start()
    {
        playerCode = -1;
        enemyCode = -1;
    }

    public void ChooseCharacter(int code)
    {
        if (isPlayer1Choosing)
        {
            CodeOfCharacter.codeOfCharacter.playerCode = code;
            isPlayer1Choosing = false; 
        }
        else
        {
            CodeOfCharacter.codeOfCharacter.enemyCode = code;
            SceneManager.LoadSceneAsync(4); 
        }
    }
}
