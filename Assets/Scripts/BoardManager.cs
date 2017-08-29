using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BoardManager : MonoBehaviour {
	public int [,,,] board = new int [4,4,4,4];
	public Player [] players;
	public GameObject [] pieces;
	private float boardSize;
	public int turn;
	public int numOfPlayers = 2;
	public bool playing = true;
	public string [] playerNames;
	public Text superText;
	public GameObject glowEffect;
	public int winner = 0;

	public enum CONTROLLER {Human, AI};

	// Use this for initialization
	void Start () {
		
		//don't touch this
		boardSize = GetComponent<Renderer> ().bounds.size.x;
		//I said don't!


		if (players.Length != 0) {
			superText.text = players [0].playerName + " starts";
		}

		turn = 0;
	}
	// Update is called once per frame
	void Update () {
		
		if (playing && players [turn%numOfPlayers].controller == CONTROLLER.AI) {

			int[] pos = players [turn%numOfPlayers].ai.getMove (board, turn%numOfPlayers+1);

			place (pos [0], pos [1], pos [2], pos [3], turn%numOfPlayers);

			winner = checkWin (pos [0], pos [1], pos [2], pos [3]);
			if (winner != 0) {
				superText.text = players [turn%numOfPlayers].playerName + " wins!!!";
			} else {
				superText.text = players [++turn%numOfPlayers].playerName + "'s turn";
			}
		}

		if(Input.GetButtonDown("Mouse 1")){
			Vector3 mousePos = Vector3.zero;
			Ray mouseRay = Camera.main.ScreenPointToRay (Input.mousePosition);
			RaycastHit mouseHit;
			if (Physics.Raycast(mouseRay, out mouseHit)){
				mousePos = new Vector3 (mouseHit.point.x, mouseHit.point.y, 0);
			}
			if(mousePos.x - gameObject.transform.position.x < boardSize/2 && mousePos.x - gameObject.transform.position.x > -boardSize/2 &&
				mousePos.y - gameObject.transform.position.y < boardSize/2 && mousePos. y - gameObject.transform.position.y > -boardSize/2){
			
				int [] pos = worldToBoard (mousePos);

				//Debug.Log ("("+pos[0] + ", " + pos[1] + ", " + pos[2] + ", " + pos[3] + ")");

				if (playing && players [turn%numOfPlayers].controller == CONTROLLER.Human && board [pos [0], pos [1], pos [2], pos [3]] == 0) {
					place (pos [0], pos [1], pos [2], pos [3], turn%numOfPlayers);

					int w = checkWin (pos [0], pos [1], pos [2], pos [3]);
					if (w != 0) {
						superText.text = players [turn%numOfPlayers].playerName + " wins!!!";
					} else {
						superText.text = players [++turn%numOfPlayers].playerName + "'s turn";				
					}
				}
			}
		}

	}

	int checkWin(int x, int y, int z, int c){

		int w = board[x,y,z,c];

		int [] position = {x, y, z, c};

		if(w == 0) return 0;

		for (int i = 1; i < 16; i++){
			//There are 16 directions to check and I use the "Vector" to define each direction
			int [] vector = {getBit(i,0), getBit(i,1), getBit(i,2), getBit(i,3)};

			int l = 0;
			//These next two basically adjust the vector because some of them can be negative in certain patterns I forget exactly how.
			for (int j = 0; j < 4; j++){
				if(vector[j] == 0) continue;
				l = j;
				break;
			}

			bool goback = false;
			for (int j = l+1; j < 4; j++){
				if(vector[j] == 0) continue;
				if(position[l] + position[j] == 3){
					vector[j]*= -1;
				}
				else if(position[l] != position[j]){
					goback = true;
					break;
				}
			}
			if(goback)continue;

			goback = false;

			for(int j = 1; j < 4; j++){
				
				if( board[(4+x+j*vector[0])%4, (4+y+j*vector[1])%4, (4+z+j*vector[2])%4, (4+c+j*vector[3])%4] != w ){
					goback = true;
					break;
				}
			}
			if (goback) continue;

			for (int j = 0; j < 4; j++) {
				Instantiate (glowEffect, 

					gameObject.transform.position + new Vector3 (
						(boardSize * -15/32) + (boardSize / 16) * ((4+x+j*vector[0])%4) + boardSize/4 * ((4+z+j*vector[2])%4),
						(boardSize * 15/32) + (-boardSize / 16) * ((4+y+j*vector[1])%4) + (-boardSize/4 * ((4+c+j*vector[3])%4)),
						0),

					gameObject.transform.rotation, gameObject.transform);
				
			}
			playing = false;
			return w;

		}
		return 0;
	}

	private static int getBit(int number, int index){
		int n = 1;

		for (int i = 0; i < index; i++) n *= 2;

		return ((number/n)%2);
	}

	public void placeX(int x, int y, int z, int c){
		if (board[x, y, z, c] != 0) return;
		board [x, y, z, c] = 1;
		Instantiate (players[0].pieceObject, gameObject.transform.position + new Vector3 ((boardSize * -15/32) + (boardSize / 16) * x + boardSize/4 * z,
																		   (boardSize * 15/32) + (-boardSize / 16) * y + (-boardSize/4 * c), 0), 
					gameObject.transform.rotation, gameObject.transform);
		
	}

	public void placeO(int x, int y, int z, int c){
		if (board[x, y, z, c] != 0) return;
		board [x, y, z, c] = 2;
		Instantiate (players[1].pieceObject, gameObject.transform.position + new Vector3 ((boardSize * -15/32) + (boardSize / 16) * x + boardSize/4 * z,
																		   (boardSize * 15/32) + (-boardSize / 16) * y + (-boardSize/4 * c), 0), 
					gameObject.transform.rotation, gameObject.transform); 
	}

	public void place(int x, int y, int z, int c, int piece){
		if (board[x, y, z, c] != 0) return;
		board [x, y, z, c] = piece + 1;
		Instantiate (players[piece].pieceObject, gameObject.transform.position + new Vector3 ((boardSize * -15/32) + (boardSize / 16) * x + boardSize/4 * z,
			(boardSize * 15/32) + (-boardSize / 16) * y + (-boardSize/4 * c), 0), 
			gameObject.transform.rotation, gameObject.transform); 
	}



	public int[] worldToBoard(Vector3 pos){
		
		int[] coords = new int[4];
		float x = ((pos - gameObject.transform.position).x);
		x += boardSize / 2;
		int z = 0;
		for (float f = x; f >= boardSize / 4; f -= boardSize / 4, z++) { }
		coords [0] = ((x % (boardSize / 4)) < boardSize / 8) ? (x % (boardSize / 4) < boardSize / 16) ? 0 : 1 : (x % (boardSize / 4) < boardSize * 3 / 16) ? 2 : 3;
		coords [2] = z;

		float y = ((pos - gameObject.transform.position).y);
		y += boardSize / 2;
		int c = 3;
		for (float f = y; f >= boardSize / 4; f -= boardSize / 4, c--) { }
		coords [1] = ((y % (boardSize / 4)) < boardSize / 8) ? (y % (boardSize / 4) < boardSize / 16) ? 3 : 2 : (y % (boardSize / 4) < boardSize * 3 / 16) ? 1 : 0;
		coords [3] = c;

		return coords;
	}

	public void resetBoard(){
		foreach (Transform child in transform) {
			Destroy (child.gameObject);
		}
		board = new int[4, 4, 4, 4];
		turn = 0;
		playing = true;
		winner = 0;
	}


}
