using UnityEngine;

public class Projectile_Cannon : MonoBehaviour
{
    public string idName;
    public GameObject shotEffect;

    private AI_Structure ai_Structure;

    public void ReadyAndShot(AI_Structure structure, Transform target)
    {
        ai_Structure = structure;

        transform.position = target.position;

        // �̵��� �Ŀ� ����Ʈ�� ������ ��
        shotEffect.SetActive(false);
        shotEffect.SetActive(true);

        Invoke("DisableShot", 0.2f);
    }

    public void DisableShot()
    {
        GamePlay.Instance.spawnManager.ReturnProjectilePool(idName, this.transform);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Monster")
        {
            ai_Structure.DealDamage(other.gameObject);
        }
    }
}
