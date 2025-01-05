using UnityEngine;

public class NewBehaviourScript : MonoBehaviour
{
    [SerializeField]
    private GameObject punchSlasher;
    private PlayerController GetPlayer;
    private AierController GetPlayer1;
    private CeciliaController GetPlayer2;
    private HaiyinController GetPlayer3;
    private Player2Aier GetPlayer4;
    private Player2Dark GetPlayer5;
    private Player2Haiyin GetPlayer6;
    private Player2Cecilia GetPlayer7;
    private EnemyController GetEnemy;

    private void Awake()
{
    var player = GameObject.FindGameObjectWithTag(Tags.Player_Tag);
    if (player == null)
    {
        GetPlayer = player.GetComponent<PlayerController>();
    }
}


    private void OnTriggerEnter2D(Collider2D collision)
{
    if (collision.tag == Tags.Player_Tag)
    {
        if (GetPlayer != null && !GetPlayer.isBlock)
        {
            Instantiate(punchSlasher, new Vector3(transform.position.x, transform.position.y, -4.0f), Quaternion.identity);
        }
    }

    if (collision.tag == Tags.Enemy_Tag)
    {
        if (punchSlasher != null)
        {
            Instantiate(punchSlasher, new Vector3(transform.position.x, transform.position.y, -4.0f), Quaternion.identity);
        }
    }
}

}
