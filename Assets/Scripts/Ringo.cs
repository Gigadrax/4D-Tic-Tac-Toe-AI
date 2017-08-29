using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ringo : AI {

	private string Ringo_Name = "Ringo";

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public override string AI_Name {
		get {
			return Ringo_Name;
		}
		set {
			Ringo_Name = value;
		}
	}

	//Get next move the AI will do on the 4x4x4x4 board. Unlikely it will work for any other shapes of boards
	//returns 4 ints for the coordinates of the move it chooses
	public override int[] getMove (int [,,,] board, int piece){
		int x = Random.Range(0, 4);
		int y = Random.Range(0, 4);
		int z = Random.Range(0, 4);
		int c = Random.Range(0, 4);

		while(board[x,y,z,c] != 0){		
			x = Random.Range(0, 4);
			y = Random.Range(0, 4);
			z = Random.Range(0, 4);
			c = Random.Range(0, 4);
		}

		return new int[] {x,y,z,c};
	}

}
