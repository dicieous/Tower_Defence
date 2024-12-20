using System;
using UnityEngine;

public static class GameEvents
{
   public static event Action<int> OnEnemyKilled;
   public static event Action<int> OnHouseHit;

   public static event Action OnGameStart;
   public static event Action OnGameEnd;
   
   public static event Action<int> OnCoinsChanged;
   
   public static void CoinsChanged(int newAmount) => OnCoinsChanged?.Invoke(newAmount);
   
   public static void EnemyKilled(int rewardCoins) => OnEnemyKilled?.Invoke(rewardCoins);

   public static void HouseHit(int damage) => OnHouseHit?.Invoke(damage);

   public static void GameStarted() => OnGameStart?.Invoke();

   public static void GameEnded() => OnGameEnd?.Invoke();
}
