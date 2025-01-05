
using UnityEngine;

public class CodeOfCharacter : MonoBehaviour
{
    public static CodeOfCharacter codeOfCharacter;

    public int playerCode;
    public int enemyCode;

    private void Awake()
    {
        if(codeOfCharacter == null) {
            DontDestroyOnLoad(gameObject);
            codeOfCharacter = this;
        }
        else if (codeOfCharacter != this) {
            Destroy(gameObject);
        }
    }
}
