using UnityEngine;
using TMPro;
public class playerLeaderboardData : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI userName;
    [SerializeField] protected TextMeshProUGUI userrank;
    [SerializeField] protected TextMeshProUGUI userscore;
   public void SetData(string name,int score,int rank)
    {
        userName.text = name;
        userscore.text = score.ToString();
        userrank.text = (rank +1).ToString();
    }
}
