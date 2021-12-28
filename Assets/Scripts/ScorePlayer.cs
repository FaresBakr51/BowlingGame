using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Photon.Pun;
using Photon.Realtime;
public class ScorePlayer : MonoBehaviourPunCallbacks {

	public List<Text> scores_text = new List<Text>();
	public List<Text> round_scores_text = new List<Text>();
	public  int totalscre = 0;
	private PlayerController _playercontroll;

	void Awake(){
		_playercontroll = GetComponent<PlayerController>();
	
	}
	public void FillRolls(List<int> rolls)
	{
		string scoresString = FormatRolls(rolls);
		for (int i = 0; i < scoresString.Length; i++)
		{
			scores_text[i].text = scoresString[i].ToString();
		}
	}


	public void FillFrames(List<int> frames)
	{
		for (int i = 0; i < frames.Count; i++)
		{
			round_scores_text[i].text = frames[i].ToString();
		
		   totalscre  = frames[i];
		
		  
		}
	
		   
			/* 	photonView.RPC("RpcChangeScore", RpcTarget.AllBuffered); */
			
	}
	/* [PunRPC]
    private void RpcChangeScore(){

		_playercontroll._mytotal.GetComponentInChildren<TextMeshProUGUI>().text = GetNickname.nickname +" :" + totalscre.ToString();
	}
 */
	public static string FormatRolls(List<int> rolls)
	{
		string output = "";

		for (int i = 0; i < rolls.Count; i++)
		{
			int box = output.Length + 1;                            // Score box 1 to 21 

			if (rolls[i] == 0)
			{                                   // Always enter 0 as -
				output += "-";
			}
			else if ((box % 2 == 0 || box == 21) && rolls[i - 1] + rolls[i] == 10)
			{   // SPARE
				output += "/";
			}
			else if (box >= 19 && rolls[i] == 10)
			{               // STRIKE in frame 10
				output += "X";
			}
			else if (rolls[i] == 10)
			{                           // STRIKE in frame 1-9
				output += "X ";
			}
			else
			{
				output += rolls[i].ToString();                      // Normal 1-9 bowl
			}
		}

		return output;
	}

   
}
