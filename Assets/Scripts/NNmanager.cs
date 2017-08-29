using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NNmanager : MonoBehaviour {

	public BoardManager bm;
	public bool training = false;
	public Dropdown dropdown;
	public NNAI NNHost_1;
	public NNAI NNHost_2;

	private NeuralNet[] colony;
	private int[] fitness = new int[1000];
	private int iteration;

	private readonly int VS_SELF = 0;
	private readonly int VS_SOMMER = 1;
	private readonly int VS_RINGO = 2;
	private readonly int VS_HUMAN = 3;

	// Use this for initialization
	void Start () {

		colony = new NeuralNet[1000];
		for (int i = 0; i < colony.Length; i++){
			colony [i] = new NeuralNet ();
		}


	}
	
	// Update is called once per frame
	//Detect when there's a loop between two neural nets 
	//reset the board, award fitness and move onto the next neural net(s)
	//resort based on fitness scores, kill bottom half, randomly saving some from bottom half and killing some from the top half
	//breed the remaining 500 to fill the leftover spots and repeat
	void Update () {

		//Loop detection
		if (dropdown.value == VS_SELF && bm.turn > 512) {

		}

		if (bm.winner != 0) {
			//Fitness is assigned differently when it faces itself
			if (dropdown.value == VS_RINGO) {
				//If the NN won
				if (bm.players [bm.winner-1].ai.AI_Name != "Ringo") {
					fitness [iteration] += 256 + (256 - bm.turn);
				} else {
					int p = ((bm.players [0].ai.AI_Name == "Ringo") ? 2 : 1);

					for (int i = 0; i < 4; i++) {
						for (int j = 0; j < 4; j++) {
							for (int k = 0; k < 4; k++) {
								for (int l = 0; l < 4; l++) {
									if (bm.board [i, j, k, l] == p) {
										fitness [iteration] += 1;
									}
								}
							}
						}
					}
				}
				//If NN went first, do another round where it goes second else go to the next NN
				if (iteration >= 999) {
					NextGeneration ();
					NNHost_1.NN = colony [iteration];
					bm.players [0].ai = NNHost_1;
					bm.players [1].ai = bm.players [1].aiList [dropdown.value - 1];
					bm.resetBoard ();
				}else if (bm.players [0].ai.AI_Name == "Ringo") {
					NNHost_1.NN = colony [++iteration];
					bm.players [0].ai = NNHost_1;
					bm.players [1].ai = bm.players [1].aiList [dropdown.value - 1];
					bm.resetBoard ();
					Debug.Log ("NN " + iteration + " Playing: " + bm.players [0].ai.AI_Name);
				} else {
					bm.players [0].ai = bm.players[0].aiList [dropdown.value-1];
					bm.players [1].ai = NNHost_1;
					bm.resetBoard ();
				}
			}
		}
	}

	public void setTraining(bool value){
		training = value;
		if (value == false) {
			foreach (Player p in bm.players) {
				p.setAI ();
			}
			bm.resetBoard ();
			return;
		}
		bm.resetBoard ();
		iteration = 0;
		NNHost_1.NN = colony [iteration];
		bm.players [0].ai = NNHost_1;
		bm.players [0].controller = BoardManager.CONTROLLER.AI;

		if (dropdown.value == VS_SELF) {
			NNHost_2.NN = colony [++iteration];
			bm.players [1].ai = NNHost_2;
			bm.players [1].controller = BoardManager.CONTROLLER.AI;
		} else if (dropdown.value == VS_HUMAN) {
			bm.players [1].controller = BoardManager.CONTROLLER.Human;	
		} else {
			bm.players [1].ai = bm.players[1].aiList [dropdown.value-1];
			bm.players [1].controller = BoardManager.CONTROLLER.AI;
		}
	}
	//resort based on fitness scores, kill bottom half, randomly saving some from bottom half and killing some from the top half
	//breed the remaining 500 to fill the leftover spots and repeat
	private void NextGeneration(){
		//Just sort the top 500, the order of the rest doesn't matter.
		Debug.Log("Starting NextGeneration...");
		for (int i = 0; i < 500; i++) {
			int indexOfMax = i;
			for (int j = i; j < 1000; j++) {
				if (fitness [j] > fitness[indexOfMax]) {
					
					indexOfMax = j;
					//yield return null;
				}
			}
			int temp = fitness [i];
			fitness [i] = fitness [indexOfMax];
			fitness [indexOfMax] = temp;

			NeuralNet NNtemp = colony [i];
			colony [i] = colony [indexOfMax];
			colony [indexOfMax] = NNtemp;
		}
		//Fitness has served its purpose for now so we reset it.
		fitness = new int[1000];
		iteration = 0;
		//give 20 spots away randomly

		for (int i = 0; i < 20; i++) {
			//Debug.Log ("Picking the #" + (i + 1) + " lucky survivor");
			int r = 500 + Random.Range (i, 500);
			NeuralNet temp = colony [500 + i];
			colony [500 + i] = colony [r];
			colony [r] = temp;

			//Mostly random, just leaning towards the lower end
			r = Mathf.FloorToInt(Mathf.Sqrt (Random.Range (0, 499-i ^ 2)));
			//Saved NN is put in the back so as not to get chosen again while the one at the back takes the
			//spot of the one that dies
			colony [r] = colony [499 - i];
			colony [499 - i] = colony [500 + i];
		}

		for (int i = 0; i < 500; i++) {
			//Debug.Log ("Shuffling... " + i);
			int r = Random.Range (i, 500);

			NeuralNet temp = colony [i];
			colony [i] = colony [r];
			colony [r] = temp;
		}


		for (int i = 0; i < 500; i+=2){
			NeuralNet[] children = NeuralNet.breed (colony [i], colony [i + 1]);
			colony [500 + i] = children [0];
			colony [500 + i+1] = children [1];
		}


		Debug.Log ("Done Next Generation.");
	}
}
