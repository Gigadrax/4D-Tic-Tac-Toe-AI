using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NeuralNet{

	protected neuron[] HL1 = new neuron[32];

	protected neuron[] HL2 = new neuron[32];

	protected neuron[] output = new neuron[16];


	public NeuralNet(){
		HL1 = new neuron[32];

		HL2 = new neuron[32];

		output = new neuron[16];

		for (int i = 0; i < 32; i++){
			HL1 [i] = new neuron (256);
			HL2 [i] = new neuron (32);
		}

		for (int i = 0; i < output.Length; i++) {
			output [i] = new neuron (32);
		}
	}
		
	protected NeuralNet (neuron[] HL1, neuron[] HL2, neuron[] output){
		this.HL1 = HL1;
		this.HL2 = HL2;
		this.output = output;
	}


	//only works for 4x4x4x4 boards with 2 players
	public int[] getMove (int[,,,] board, int piece)
	{
		float[] input = new float[256];

		for (int i = 0; i < 4; i++) {
			for (int j = 0; j < 4; j++) {
				for (int k = 0; k < 4; k++) {
					for (int l = 0; l < 4; l++) {
						input [l + 4 * k + 16 * j + 64 * i] = (board [i, j, k, l] == 0 ? 0f : board [i, j, k, l] == piece ? 1f : -1);  
					}
				}
			}
		}

		float[] result = fireAll (output, fireAll (HL2, fireAll (HL1, input)));

		int[] moveCoords = new int[] { 0, 0, 0, 0 };

		for (int i = 0; i < 4; i++){
			for (int j = 1; j < 4; j++) {
				if (result [moveCoords [i]] < result [j + (4 * i)]) {
					moveCoords [i] = j;
				}
			}
		}

		//Debug.Log ("[" + moveCoords [0] + ", " + moveCoords [1] + ", " + moveCoords [2] + ", " + moveCoords [3] + "]");

		return moveCoords;

	}



	private float[] fireAll(neuron[] ns, float[] inputLayer){
		float[] output = new float[ns.Length];

		for (int i = 0; i < ns.Length; i++) {
			output [i] = ns [i].fire (inputLayer);
		}

		return output;
	}

	public static NeuralNet[] breed(NeuralNet mother, NeuralNet father){
		NeuralNet[] children = new NeuralNet[2];

		neuron[] c1_HL1 = new neuron[32];
		neuron[] c1_HL2 = new neuron[32];
		neuron[] c1_output = new neuron[16];

		neuron[] c2_HL1 = new neuron[32];
		neuron[] c2_HL2 = new neuron[32];
		neuron[] c2_output = new neuron[16];

		for (int i = 0; i < mother.HL1.Length; i++) {
			float c1_bias, c2_bias;

			float[] c1_weights = new float[256];
			float[] c2_weights = new float[256];

			if (Random.Range (0, 2) == 0) {
				c1_bias = mother.HL1 [i].bias;
				c2_bias = father.HL1 [i].bias;
			} else {
				c1_bias = father.HL1 [i].bias;
				c2_bias = mother.HL1 [i].bias;
			}

			for (int j = 0; j < mother.HL1 [i].weights.Length; j++) {
				if (Random.Range (0, 2) == 0) {
					c1_weights [j] = mother.HL1 [i].weights [j];
					c2_weights [j] = father.HL1 [i].weights [j];
				} else {
					c1_weights [j] = father.HL1 [i].weights [j];
					c2_weights [j] = mother.HL1 [i].weights [j];
				}
			}

			c1_HL1 [i] = new neuron (c1_weights, c1_bias);
			c1_HL1 [i].mutate();
			c2_HL1 [i] = new neuron (c2_weights, c2_bias);
			c2_HL1 [i].mutate();
		}

		for (int i = 0; i < mother.HL2.Length; i++) {
			float c1_bias, c2_bias;

			float[] c1_weights = new float[32];
			float[] c2_weights = new float[32];

			if (Random.Range (0, 2) == 0) {
				c1_bias = mother.HL2 [i].bias;
				c2_bias = father.HL2 [i].bias;
			} else {
				c1_bias = father.HL2 [i].bias;
				c2_bias = mother.HL2 [i].bias;
			}

			for (int j = 0; j < mother.HL2 [i].weights.Length; j++) {
				if (Random.Range (0, 2) == 0) {
					c1_weights [j] = mother.HL2 [i].weights [j];
					c2_weights [j] = father.HL2 [i].weights [j];
				} else {
					c1_weights [j] = father.HL2 [i].weights [j];
					c2_weights [j] = mother.HL2 [i].weights [j];
				}
			}

			c1_HL2 [i] = new neuron (c1_weights, c1_bias);
			c1_HL2 [i].mutate();
			c2_HL2 [i] = new neuron (c2_weights, c2_bias);
			c2_HL2 [i].mutate();
		}

		for (int i = 0; i < mother.output.Length; i++) {
			float c1_bias, c2_bias;

			float[] c1_weights = new float[32];
			float[] c2_weights = new float[32];

			if (Random.Range (0, 2) == 0) {
				c1_bias = mother.output [i].bias;
				c2_bias = father.output [i].bias;
			} else {
				c1_bias = father.output [i].bias;
				c2_bias = mother.output [i].bias;
			}

			for (int j = 0; j < mother.output [i].weights.Length; j++) {
				if (Random.Range (0, 2) == 0) {
					c1_weights [j] = mother.output [i].weights [j];
					c2_weights [j] = father.output [i].weights [j];
				} else {
					c1_weights [j] = father.output [i].weights [j];
					c2_weights [j] = mother.output [i].weights [j];
				}
			}

			c1_output [i] = new neuron (c1_weights, c1_bias);
			c1_output [i].mutate();
			c2_output [i] = new neuron (c2_weights, c2_bias);
			c2_output [i].mutate();
		}

		children [0] = new NeuralNet (c1_HL1, c1_HL2, c1_output);
		children [1] = new NeuralNet (c2_HL1, c2_HL2, c2_output);

		return children;
	}


	//************************************************************
	//Neuron class
	//************************************************************

	protected class neuron {
		public float [] weights;
		public float bias;

		public neuron(int numOfWeights, float bias){
			weights = new float[numOfWeights];
			this.bias = bias;

			for (int i = 0; i < weights.Length; i++){
				//assigning random weights that use a vague rule of thumb to stay at around the same scale
				//bias is squared to keep positive
				weights[i] = Random.Range(-(bias * bias / numOfWeights), (bias * bias / numOfWeights));
			}
		}

		public neuron(int numOfWeights){
			bias = Random.Range(-10000f, 10000f);
			weights = new float[numOfWeights];

			for (int i = 0; i < weights.Length; i++){
				//assigning random weights that use a vague rule of thumb to stay at around the same scale
				//bias is squared to keep positive
				weights[i] = Random.Range(-(bias * bias / numOfWeights), (bias * bias / numOfWeights));
			}
		}

		public neuron(float [] weights, float bias){
			this.weights = new float[weights.Length];
			for (int i = 0; i < weights.Length; i++){
				this.weights[i] = weights[i];
			}
			this.bias = bias;
		}

		public void mutate(){

			for (int i = 0; i < weights.Length; i++) {
				weights[i] *= Random.Range (-0.01f, 0.01f);
			}

			bias *= Random.Range (-0.01f, 0.01f);
		}

		//fires this neuron based on the previous layers results in x
		public float fire(float [] x){
			return sigmoid (weightsDot (x), false);
		}

		//performs a dot product with the weights and given vector of the same size
		private float weightsDot(float [] v){
			if (v.Length != weights.Length) {
				throw new System.ArgumentException ();
			}
			float sum = 0;
			for (int i = 0; i < weights.Length; i++) {
				sum += weights [i] * v [i];
			}
			return sum;
		}
	
		private float sigmoid (float x, bool deriv)
		{
			if (deriv == true) {
				return x * (1 - x);
			}
			return 1 / (1 + Mathf.Exp (-x));
		}
	}
		
}
