using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class AI : MonoBehaviour {

	abstract public int[] getMove(int [,,,] board, int piece);

	public abstract string AI_Name{get; set;}
}
