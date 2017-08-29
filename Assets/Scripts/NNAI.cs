using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NNAI : AI {

	public NeuralNet NN;

	private string NN_Name;
	// Use this for initialization
	void Start () {
		NN = new NeuralNet ();

		if (NN_Name == "") {
			NN_Name = generateName ();
		}

	}
		
	// Update is called once per frame
	void Update () {
		
	}

	public override string AI_Name {
		get {
			if (NN_Name == "") {
				NN_Name = generateName ();
			}
			return NN_Name;
		}
		set {
			NN_Name = value;
		}
	}

	public override int[] getMove (int[,,,] board, int piece)
	{
		return NN.getMove (board, piece);
	}

	public string generateName(){

		int size = Random.Range (0, 5);

		if(size != 0 && size != 4){
			size = 3;
		}
		else if (size == 0){
			size = 2;
		}

		string name = "";

		for(;size > 0; size--){
			switch(Random.Range(0, 101)){

			case 0: name += "a";
				break;

			case 1: name += "i";
				break;

			case 2: name += "u";
				break;

			case 3: name += "e";
				break;

			case 4: name += "o";
				break;

			case 5: name += "ka";
				break;

			case 6: name += "ki";
				break;

			case 7: name += "ku";
				break;

			case 8: name += "ke";
				break;

			case 9: name += "ko";
				break;

			case 10: name += "sa";
				break;

			case 11: name += "shi";
				break;

			case 12: name += "su";
				break;

			case 13: name += "se";
				break;

			case 14: name += "so";
				break;

			case 15: name += "ta";
				break;

			case 16: name += "chi";
				break;

			case 17: name += "tsu";
				break;

			case 18: name += "te";
				break;

			case 19: name += "to";
				break;

			case 20: name += "na";
				break;

			case 21: name += "ni";
				break;

			case 22: name += "nu";
				break;

			case 23: name += "ne";
				break;

			case 24: name += "no";
				break;

			case 25: name += "ha";
				break;

			case 26: name += "hi";
				break;

			case 27: name += "fu";
				break;

			case 28: name += "he";
				break;

			case 29: name += "ho";
				break;

			case 30: name += "ma";
				break;

			case 31: name += "mi";
				break;

			case 32: name += "mu";
				break;

			case 33: name += "me";
				break;

			case 34: name += "mo";
				break;

			case 35: name += "ya";
				break;

			case 36: name += "yu";
				break;

			case 37: name += "yo";
				break;

			case 38: name += "ra";
				break;

			case 39: name += "ri";
				break;

			case 40: name += "ru";
				break;

			case 41: name += "re";
				break;

			case 42: name += "ro";
				break;

			case 43: name += "wa";
				break;

			case 44: name += "n";
				break;

			case 45: name += "ga";
				break;

			case 46: name += "gi";
				break;

			case 47: name += "gu";
				break;

			case 48: name += "ge";
				break;

			case 49: name += "go";
				break;

			case 50: name += "za";
				break;

			case 51: name += "ji";
				break;

			case 52: name += "zu";
				break;

			case 53: name += "ze";
				break;

			case 54: name += "zo";
				break;

			case 55: name += "da";
				break;

			case 56: name += "de";
				break;

			case 57: name += "do";
				break;

			case 58: name += "ba";
				break;

			case 59: name += "bi";
				break;

			case 60: name += "bu";
				break;

			case 61: name += "be";
				break;

			case 62: name += "bo";
				break;

			case 63: name += "pa";
				break;

			case 64: name += "pi";
				break;

			case 65: name += "pu";
				break;

			case 66: name += "pe";
				break;

			case 67: name += "po";
				break;

			case 68: name += "kya";
				break;

			case 69: name += "kyu";
				break;

			case 70: name += "kyo";
				break;

			case 71: name += "sha";
				break;

			case 72: name += "shu";
				break;

			case 73: name += "sho";
				break;

			case 74: name += "cha";
				break;

			case 75: name += "chu";
				break;

			case 76: name += "cho";
				break;

			case 77: name += "nya";
				break;

			case 78: name += "nyu";
				break;

			case 79: name += "nyo";
				break;

			case 80: name += "hya";
				break;

			case 81: name += "hyu";
				break;

			case 82: name += "hyo";
				break;

			case 83: name += "mya";
				break;

			case 84: name += "myu";
				break;

			case 85: name += "myo";
				break;

			case 86: name += "rya";
				break;

			case 87: name += "ryu";
				break;

			case 88: name += "ryo";
				break;

			case 89: name += "gya";
				break;

			case 90: name += "gyu";
				break;

			case 91: name += "gyo";
				break;

			case 92: name += "ja";
				break;

			case 93: name += "ju";
				break;

			case 94: name += "jo";
				break;

			case 95: name += "bya";
				break;

			case 96: name += "byu";
				break;

			case 97: name += "byo";
				break;

			case 98: name += "pya";
				break;

			case 99: name += "pyu";
				break;

			case 100: name += "pyo";
				break;	
			}

			if(name == "n"){
				name = "";

				size++;
			}
		}

		name = name.Substring (0, 1).ToUpper () + name.Substring (1);

		return name;



	}
}
