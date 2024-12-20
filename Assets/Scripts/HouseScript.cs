using System;
using UnityEngine;

public class HouseScript : MonoBehaviour
{
    private int houseHealth;
    private void Start()
    {
        houseHealth = GameManager.Instance.HouseMaxHealth;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out EnemyAI enemyAI))
        {
            enemyAI.BlastOnContact();
            houseHealth -= enemyAI.giveDamageAmt;
            if (houseHealth <= 0)
            {
                GameManager.Instance.GameEnded();
            }
            GameEvents.HouseHit(10);
        }
    }
}
