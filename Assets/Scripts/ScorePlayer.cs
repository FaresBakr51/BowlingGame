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
	private int _StrikeInrow;
	void Start(){
		_currentframe = 0;
		if(!PhotonNetwork.OfflineMode || (PhotonNetwork.OfflineMode && PhotonNetwork.InRoom)){
				_playercontroll = GetComponent<PlayerController>();
			_playercontroll.UpdateSound(_playercontroll._FramesClips[0]);
	
		}else if(PhotonNetwork.OfflineMode && !PhotonNetwork.InRoom){

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
	
	  
		if(_scoreStrn.EndsWith("X ")){
			UpdateStrikeSound();
		}
		else if(_scoreStrn.EndsWith("/")){
					   	   	   
		   if((!PhotonNetwork.OfflineMode || (PhotonNetwork.OfflineMode && PhotonNetwork.InRoom ))){
			_playercontroll.UpdateSound(_playercontroll._gameClips[1]);
			StartCoroutine(WaitTxt(_playercontroll._spareTxt));
			
			}else if(PhotonNetwork.OfflineMode && !PhotonNetwork.InRoom){
				_offlinemodeControll.UpdateSound(_offlinemodeControll._gameClips[1]);
					StartCoroutine(WaitTxt(_offlinemodeControll._spareTxt));
				
			}
			_StrikeInrow = 0;
		}
		else if (_scoreStrn.EndsWith("-"))
        {
			if ((!PhotonNetwork.OfflineMode || (PhotonNetwork.OfflineMode && PhotonNetwork.InRoom)))
			{
				_playercontroll.UpdateSound(_playercontroll._gameClips[2]);
				StartCoroutine(WaitTxt(_playercontroll._gutterTxt));

			}
			else if (PhotonNetwork.OfflineMode && !PhotonNetwork.InRoom)
			{
				_offlinemodeControll.UpdateSound(_offlinemodeControll._gameClips[2]);
				StartCoroutine(WaitTxt(_offlinemodeControll._gutterTxt));

			}
			_StrikeInrow = 0;
        }
        else
        {
			_StrikeInrow = 0;
		}
		StartCoroutine(waitFrameSound());
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
		
		if (_scoreStrn.Length >= _counterStringFrames+2){

			_counterStringFrames = _scoreStrn.Length;
			_currentframe++;
			if(!PhotonNetwork.OfflineMode || (PhotonNetwork.OfflineMode && PhotonNetwork.InRoom)){
			_playercontroll.UpdateSound(_playercontroll._FramesClips[_currentframe]);
			}else if(PhotonNetwork.OfflineMode && !PhotonNetwork.InRoom){
				_offlinemodeControll.UpdateSound(_offlinemodeControll._FramesClips[_currentframe]);
			}
		
		}
	
	}
	private void UpdateStrikeSound()
    {
		_StrikeInrow ++;
		if ((!PhotonNetwork.OfflineMode || (PhotonNetwork.OfflineMode && PhotonNetwork.InRoom)))
		{
		
			StartCoroutine(WaitTxt(_playercontroll._strikeTxt));
		}
		else if (PhotonNetwork.OfflineMode && !PhotonNetwork.InRoom)
		{
			StartCoroutine(WaitTxt(_offlinemodeControll._strikeTxt));
		}
		if (_StrikeInrow == 2)
		{
			if ((!PhotonNetwork.OfflineMode || (PhotonNetwork.OfflineMode && PhotonNetwork.InRoom)))
			{
				_playercontroll.UpdateSound(_playercontroll._gameClips[3]);
			}
			else if (PhotonNetwork.OfflineMode && !PhotonNetwork.InRoom)
			{
				_offlinemodeControll.UpdateSound(_offlinemodeControll._gameClips[3]);
			}
		}
		else if (_StrikeInrow == 3)
		{
			if ((!PhotonNetwork.OfflineMode || (PhotonNetwork.OfflineMode && PhotonNetwork.InRoom)))
			{
				_playercontroll.UpdateSound(_playercontroll._gameClips[4]);
			}
			else if (PhotonNetwork.OfflineMode && !PhotonNetwork.InRoom)
			{
				_offlinemodeControll.UpdateSound(_offlinemodeControll._gameClips[4]);
			}
		}
		else if (_StrikeInrow == 4)
		{
			if ((!PhotonNetwork.OfflineMode || (PhotonNetwork.OfflineMode && PhotonNetwork.InRoom)))
			{
				_playercontroll.UpdateSound(_playercontroll._gameClips[5]);
			}
			else if (PhotonNetwork.OfflineMode && !PhotonNetwork.InRoom)
			{
				_offlinemodeControll.UpdateSound(_offlinemodeControll._gameClips[5]);
			}
		}
		else if (_StrikeInrow == 5)
		{

			if ((!PhotonNetwork.OfflineMode || (PhotonNetwork.OfflineMode && PhotonNetwork.InRoom)))
			{
				_playercontroll.UpdateSound(_playercontroll._gameClips[6]);
			}
			else if (PhotonNetwork.OfflineMode && !PhotonNetwork.InRoom)
			{
				_offlinemodeControll.UpdateSound(_offlinemodeControll._gameClips[6]);
			}
        }
        else
        {
			if ((!PhotonNetwork.OfflineMode || (PhotonNetwork.OfflineMode && PhotonNetwork.InRoom)))
			{
				_playercontroll.UpdateSound(_playercontroll._gameClips[0]);
			}
			else if (PhotonNetwork.OfflineMode && !PhotonNetwork.InRoom)
			{
				_offlinemodeControll.UpdateSound(_offlinemodeControll._gameClips[0]);
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
