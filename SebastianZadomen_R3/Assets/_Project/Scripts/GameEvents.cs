using System;

public static class GameEvents 
{
    public static event Action<bool> OnPlayerIsDead;

    public static void PlayerIsDead(bool isDead)
    {
        OnPlayerIsDead?.Invoke(isDead);
    }




}
