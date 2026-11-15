using System;

public static class GameEvents 
{
    public static event Action<bool> OnPlayerIsDead;
    public static event Action<bool> OnPlayerWin;

    public static void PlayerIsDead(bool isDead)
    {
        OnPlayerIsDead?.Invoke(isDead);
    }

    public static void PlayerWin(bool flag)
    {
        OnPlayerWin?.Invoke(flag);
    }



}
