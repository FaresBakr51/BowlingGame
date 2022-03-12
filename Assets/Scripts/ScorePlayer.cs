using System.Collections.Generic;
using UnityEngine.UI;
using Photon.Pun;
using UnityEngine;
using System.Collections;
public class ScorePlayer : MonoBehaviourPunCallbacks {

	
	public List<Text> scores_text = new List<Text>();
	public List<Text> round_scores_text = new List<Text>();
	public  int totalscre = 0;
	private PlayerController _playercontroll;
	private PlayerControllOFFlineMode _offlinemodeControll;
	private int _currentframe;
	private string _scoreStrn;
	private int _counterStringFrames;
	private bool _win;
	void Start(){
		_currentframe = 0;
		if(PhotonNetwork.OfflineMode == false || (PhotonNetwork.OfflineMode == true && PhotonNetwork.InRoom == true)){
				_playercontroll = GetComponent<PlayerController>();
			_playercontroll.UpdateSound(_playercontroll._FramesClips[0]);
	
		}else if(PhotonNetwork.OfflineMode == true && PhotonNetwork.InRoom == false){

			_offlinemodeControll = GetComponent<PlayerControllOFFlineMode>();
				_offlinemodeControll.UpdateSound(_offlinemodeControll._FramesClips[0]);
		}
	     
	
	}
	public void FillRolls(List<int> rolls)
	{
		
		 _scoreStrn = FormatRolls(rolls);
		for (int i = 0; i < _scoreStrn.Length; i++)
		{
			scores_text[i].text = _scoreStrn[i].ToString();

			
		   
		}
		Debug.Log(_scoreStrn.Length);
	   StartCoroutine(waitFrameSound());

		
		if(_scoreStrn.EndsWith("X ")){
					   	   	   
		   if((PhotonNetwork.OfflineMode == false || (PhotonNetwork.OfflineMode == true && PhotonNetwork.InRoom == true))){
			_playercontroll.UpdateSound(_playercontroll._gameClips[0]);
				StartCoroutine(WaitTxt(_playercontroll._strikeTxt));
			
			
			}else if(PhotonNetwork.OfflineMode == true && PhotonNetwork.InRoom == false){
				_offlinemodeControll.UpdateSound(_offlinemodeControll._gameClips[0]);
					StartCoroutine(WaitTxt(_offlinemodeControll._strikeTxt));
			}

		}else if(_scoreStrn.EndsWith("/")){
					   	   	   
		   if((PhotonNetwork.OfflineMode == false || (PhotonNetwork.OfflineMode == true && PhotonNetwork.InRoom == true))){
			_playercontroll.UpdateSound(_playercontroll._gameClips[1]);
			StartCoroutine(WaitTxt(_playercontroll._spareTxt));
			
			}else if(PhotonNetwork.OfflineMode == true && PhotonNetwork.InRoom == false){
				_offlinemodeControll.UpdateSound(_offlinemodeControll._gameClips[1]);
					StartCoroutine(WaitTxt(_offlinemodeControll._spareTxt));
				
			}

		}

		
		   
	}
	IEnumerator waitFrameSound(){

		yield return new WaitForSeconds(1.5f);
			UpdateFrameSound();
	}

	 IEnumerator WaitTxt(GameObject obj){

         obj.SetActive(true);

         yield return new WaitForSeconds(2.5f);

         obj.SetActive(false);
     }
	public void FillFrames(List<int> frames)
	{
		for (int i = 0; i < frames.Count; i++)
		{
			round_scores_text[i].text = frames[i].ToString();
		
		   totalscre  = frames[i];
		  
		  
		}
	
	
			
	}
	
	
	private void UpdateFrameSound(){

		if(_scoreStrn.Length >= _counterStringFrames+2){

			_counterStringFrames = _scoreStrn.Length;
			_currentframe++;
			if(PhotonNetwork.OfflineMode == false || (PhotonNetwork.OfflineMode == true && PhotonNetwork.InRoom == true)){
			_playercontroll.UpdateSound(_playercontroll._FramesClips[_currentframe]);
			}else if(PhotonNetwork.OfflineMode == true && PhotonNetwork.InRoom == false){
				_offlinemodeControll.UpdateSound(_offlinemodeControll._FramesClips[_currentframe]);
			}
		
		}
	
	}
	public  string FormatRolls(List<int> rolls)
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
				output += rolls[i].ToString();                // Normal 1-9 bowl
			}
		}

		

		return output;
	}

   
}
