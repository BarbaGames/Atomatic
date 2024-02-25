using System.Collections.Generic;
using System.Linq;

using Newtonsoft.Json;

using Progress;

using TMPro;

using UnityEngine;

public class LeaderboardHandler : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI text = null;

    private List<long> scores = null;
    private const string leaderBoardKey = "energy";
    
    public void Start()
    {
        text.text = "";
        scores = new List<long>();
        if (FileHandler.TryLoadFileRaw(leaderBoardKey, out string data))
        {
            scores = JsonConvert.DeserializeObject<List<long>>(data);
        }

        for (int i = 0; i < scores.Count; i++)
        {
            text.text += (i + 1) + "." + scores[i] + "\n";
        }
    }
    
    public void TryUpdateLeaderboard(long score)
    {
        scores.Add(score);
        scores.Sort((a, b) => b.CompareTo(a));
        if (scores.Count > 5)
        {
            scores.RemoveRange(5, scores.Count - 5);
        }
        FileHandler.SaveFile(leaderBoardKey, JsonConvert.SerializeObject(scores));

    }
}
