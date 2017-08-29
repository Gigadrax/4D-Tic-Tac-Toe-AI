using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sommer : AI {

	private string Sommer_Name = "Sommer";
	// Use this for initialization
	void Start () {

	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public override string AI_Name {
		get {
			return Sommer_Name;
		}
		set {
			Sommer_Name = value;
		}
	}

	//Goes through each position and sums up points 
	public override int[] getMove(int [,,,] board, int piece){

		int[] bestMove = new int[1];

		int bestPoints = 0;

		for (int i = 0; i < 4; i++) {
			for (int j = 0; j < 4; j++) {
				for (int k = 0; k < 4; k++) {
					for (int l = 0; l < 4; l++) {

						Debug.Log ("Checking" + "[" + i + ", " + j + ", " + k + ", " + l + "]");

						if (board [i, j, k, l] != 0) {
							continue;
						}

						int[] position = new int[] { i, j, k, l };

						int sum = 1;

						//Summing stuff

						//it basically runs the win detect algorithm but gives it a value instead of a win/not win and picks the one with the largest value
						for (int f = 1; f < 16; f++){
							//There are 16 directions to check and I use the "Vector" to define each direction
							int [] vector = {getBit(f,0), getBit(f,1), getBit(f,2), getBit(f,3)};

							int u = 0;
							//These next two basically adjust the vector because some of them can be negative in certain patterns I forget exactly how.
							for (int g = 0; g < 4; g++){
								if(vector[g] == 0) continue;
								u = g;
								break;
							}

							bool goback = false;
							for (int g = u+1; g < 4; g++){
								if(vector[g] == 0) continue;
								if(position[u] + position[g] == 3){
									vector[g]*= -1;
								}
								else if(position[u] != position[g]){
									goback = true;
									break;
								}
							}
							if(goback)continue;

							goback = false;

							//The sum of points for this particular line
							int lineSum = 1;
							int lastPiece = -1;
							for(int g = 0; g < 4; g++){

								if( board[(4+i+g*vector[0])%4, (4+j+g*vector[1])%4, (4+k+g*vector[2])%4, (4+l+g*vector[3])%4] != 0){
									if (lastPiece == -1) {
										lastPiece = board [(4 + i + g * vector [0]) % 4, (4 + j + g * vector [1]) % 4, (4 + k + g * vector [2]) % 4, (4 + l + g * vector [3]) % 4];
									}
									if (lastPiece != board[(4+i+g*vector[0])%4, (4+j+g*vector[1])%4, (4+k+g*vector[2])%4, (4+l+g*vector[3])%4]){
										lineSum = 0;
										break;
									}

									if (lastPiece == piece) {
										lineSum *= 2;
									}

									lineSum *= 16;

								}
							}
							sum += lineSum;
						}
						//End of summing stuff

						if (sum > bestPoints){
							bestPoints = sum;
							bestMove = new int[] { i, j, k, l };
						}
						if (sum == bestPoints && Random.Range(0,2) == 0) {
							bestMove = new int[] { i, j, k, l };
						}
					}
				}
			}
		}

		return bestMove;
	}

	private static int getBit(int number, int index){
		int n = 1;

		for (int i = 0; i < index; i++) n *= 2;

		return ((number/n)%2);
	}
}
