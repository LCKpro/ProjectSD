using UnityEngine;

public class Projectile : MonoBehaviour
{
    private AIPlayer player;
    public string idName;

    public void SetPlayer(AIPlayer aiPlayer)
    {
        player = aiPlayer;
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Breakable")
        {
            player.DealDamage(other.gameObject);
            GamePlay.Instance.spawnManager.ReturnProjectilePool(idName, this.transform);
        }
    }
}
