using System;

public static class GameEvents 
{
    public static event Action<bool> onPlayerIsDead;

    public static void PlayerIsDead(bool isDead)
    {
        onPlayerIsDead?.Invoke(isDead);
    }




}
