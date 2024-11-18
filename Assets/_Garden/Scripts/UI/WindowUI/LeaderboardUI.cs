using UnityEngine;
using YG;

public class LeaderboardUI : WindowUI
{
    [Header("Leaderboard UI")]
    [SerializeField] private LeaderboardYG leaderboardYG;

    public void OnEnable()
    {
        leaderboardYG.UpdateLB();
    }
}
