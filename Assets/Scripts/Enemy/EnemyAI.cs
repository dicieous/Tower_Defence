using System.Collections;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Serialization;

public class EnemyAI : MonoBehaviour
{
    [SerializeField] private float maxEnemyHealth = 10f;
    [SerializeField] private int killReward = 10;
    private float _currentEnemyHealth;

    [SerializeField] private Material hitMat;
    [SerializeField] private MeshRenderer meshRenderer;
    private Material _matInitial;
    private float _colorChangeDuration = 0.5f;
    private bool _canChangeColor = true;

    public int giveDamageAmt = 10;
    private void Start()
    {
        _currentEnemyHealth = maxEnemyHealth;
        _matInitial = meshRenderer.material;
    }

    public void TakeDamage(float damage)
    {
        _currentEnemyHealth -= damage;
        
        if (_canChangeColor)
        {
            StartCoroutine(ChangeColor());
        }

        if (_currentEnemyHealth <= 0)
        {
            RemoveEnemy();
            GameEvents.EnemyKilled(killReward);
            GameManager.Instance.IncreaseKillCount();
            GameManager.Instance.AddCoins(killReward);
        }
    }

    private IEnumerator ChangeColor()
    {
        meshRenderer.material = hitMat;
        
        _canChangeColor = false;

        yield return new WaitForSeconds(_colorChangeDuration);

        meshRenderer.material = _matInitial;
        _canChangeColor = true;
    }

    public void BlastOnContact()
    {
        //Blast on contact with House
        RemoveEnemy();
    }

    private void RemoveEnemy()
    {
        //implement objectPool
        meshRenderer.material = _matInitial;
        EnemyPooler.Instance.ReturnObject(this);
    }
}