using System;

using Progress;

using TMPro;

using UnityEngine;

public class LeaderboardHandler : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI text = null;

    private const string leaderBoardKey = "energy";

    
    public void Start()
    {
        if (FileHandler.TryLoadFileRaw(leaderBoardKey, out string data))
        {
            
        }
    }

    public void OnApplicationQuit()
    {
        
    }

    public void TryUpdateLeaderboard(double score)
    {
        
    }
}
