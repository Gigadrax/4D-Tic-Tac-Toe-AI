using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour {

	public GameObject pieceObject;
	public string playerName;
	public BoardManager.CONTROLLER controller;
	public AI ai;
	public Dropdown dropdown;
	public AI[] aiList;

	// Use this for initialization
	void Start () {
		List<string> options = new List<string> ();
		options.Add( "Human" ) ;

		for (int i = 0; i < aiList.Length; i++) {
			options.Add (aiList[i].AI_Name);
		}


		dropdown.AddOptions (options);
		}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void setAI(){
		if (dropdown.value == 0) {
			controller = BoardManager.CONTROLLER.Human;
		} else {
			ai = aiList [dropdown.value - 1];
			controller = BoardManager.CONTROLLER.AI;
		}
	}		
}
