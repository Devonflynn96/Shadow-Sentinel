using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundEmitter : MonoBehaviour
{
    [SerializeField] private float soundRange = 10f;
    [SerializeField] private LayerMask enemyLayer;
    public void EmitSound()
    {
        Collider[] enemies = Physics.OverlapSphere(transform.position, soundRange, enemyLayer);

        foreach (Collider enemy in enemies)
        {
            EnemyAI enemyAI = enemy.GetComponent<EnemyAI>();
            if (enemyAI != null)
            {
                enemyAI.OnHearSound(transform.position);
            }
        }
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, soundRange);
    }

}
