using System;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.AI;
using Random = UnityEngine.Random;

public class EnemySpawnManager : MonoBehaviour
{
    [SerializeField] private Transform house;
    [SerializeField] private List<Transform> spawnPositionsList;

    [SerializeField] private float spawnInterval = 5f;
    private Timer _timer;
    private bool _isHouseNotNull;
    private bool isGameStarted = false;

    private void OnEnable()
    {
        GameEvents.OnGameStart += GameEventsOnGameStart;
        GameEvents.OnGameEnd += GameEventsOnGameEnd;
    }
    
    private void Update()
    {
        if (isGameStarted && _timer.UpdateAndCheck(Time.deltaTime) && _timer.IsRunning())
        {
            if (EnemyPooler.Instance.IsLimitReached)
            {
                _timer.Stop();
            }
            else
            {
                SpawnEnemy();
            }
        }
    }

    private void OnDisable()
    {
        GameEvents.OnGameStart -= GameEventsOnGameStart;
        GameEvents.OnGameEnd -= GameEventsOnGameEnd;
    }
    
    private void GameEventsOnGameStart()
    {
        isGameStarted = true;
        _isHouseNotNull = house != null;
        _timer = new Timer(spawnInterval);
    }

    private void GameEventsOnGameEnd()
    {
        isGameStarted = false;
        _timer.Stop();
        EnemyPooler.Instance.ReturnAllObjects();
    }
    
    private void SpawnEnemy()
    {
        var position = spawnPositionsList[Random.Range(0, spawnPositionsList.Count - 1)].position;
        EnemyAI enemy = EnemyPooler.Instance.GetObject(position, quaternion.RotateY(180f));

        if (_isHouseNotNull && enemy.TryGetComponent(out NavMeshAgent meshAgent))
        {
            meshAgent.SetDestination(house.position);
        }
    }
}