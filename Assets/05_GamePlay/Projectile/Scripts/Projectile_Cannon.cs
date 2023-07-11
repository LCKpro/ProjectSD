using UnityEngine;

public class Projectile_Cannon : MonoBehaviour
{
    public string idName;
    public BoxCollider shotCollider;
    public GameObject shotEffect;

    private AI_Structure ai_Structure;

    public void ReadyAndShot(AI_Structure structure, Transform target)
    {
        ai_Structure = structure;

        shotCollider.enabled = true;

        transform.position = target.position;

        // �̵��� �Ŀ� ����Ʈ�� ������ ��
        shotEffect.SetActive(false);
        shotEffect.SetActive(true);

        Invoke("DisableCollider", 0.1f);
        Invoke("DisableShot", 1.0f);
    }

    public void DisableCollider()
    {
        shotCollider.enabled = false;
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
