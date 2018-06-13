using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;
using UnityEngine.UI; 
using CnControls;
using NetMQ;
using NetMQ.Sockets;
using System.Runtime.InteropServices;

public class MyGameManager : MonoBehaviour {
	/*
	[DllImport("__Internal")]
	private static extern void AddCurScore(int catScore,int dogScore);
	[DllImport("__Internal")]
	private static extern void AddCurScore2(int AvengersScore,int ThanosScore);
	[DllImport("__Internal")]
	private static extern void AddRecord(int win,int tie,int lose);
	*/


	public bool isEnd = false;
	public int PlayerScore = 0;
	public int EnemyScore = 0;
	public float catReward = 0.0f;
	public float dogReward = 0.0f;
    public float catReward_p = 0.0f;
    public float dogReward_p = 0.0f;
    public int coincnt;
	public int skullcnt;
	public float GAMETIME;
	bool isSendPost = false;
	public GameObject Wheel;
	public GameObject DogPlayer;
	public GameObject CatPlayer;
	string baseUrl = "";
	bool isDogGame;
	TextMesh playerScoretxt;
	TextMesh enemyScoretxt;
    TextMesh GameTime;
    float reNewt = 3000.0f;
    string currenturl = "";
	string gameType = "game";
	public float TIMESCALE = 10.0f;
    public Vector2 scoreAddVec_coin = new Vector2(0, 0);
    public Vector2 scoreAddVec_skul = new Vector2(0, 0);
    
    public int GetPlayerScore(){
		return PlayerScore;
	}
	public int GetEnemyScore(){
		return EnemyScore;
	}
	void TimeEndScoreCompare(){
		if (PlayerScore > EnemyScore) {
			if (isDogGame) {
				GameObject.Find ("Win").GetComponent<MeshRenderer> ().enabled = true;
			} else {
				GameObject.Find ("Lose").GetComponent<MeshRenderer> ().enabled = true;
			}
		}else if (PlayerScore < EnemyScore){
			if (isDogGame) {
				GameObject.Find ("Lose").GetComponent<MeshRenderer> ().enabled = true;
			} else {
				GameObject.Find ("Win").GetComponent<MeshRenderer> ().enabled = true;
			}
		} else {
			GameObject.Find ("Tie").GetComponent<MeshRenderer> ().enabled = true;
		}
			
	}

	void GameTimeElapse(){
		
		GameTime.text = string.Format("{0}",GAMETIME.ToString("0"));
		GAMETIME += Time.deltaTime;
		if (GAMETIME >= reNewt) {
			//TimeEndScoreCompare ();
			isEnd = true;
		}

	}
	public void ScoreCounter(bool isCoinTarget,bool isBorderRight){
		int playerScoreAdd = 0;
		int enemyScoreAdd = 0;
		if (isCoinTarget) {
			if (isBorderRight) {
				playerScoreAdd = 2;
                dogReward_p = 1.0f;
                catReward_p = -1.0f;
            } else {
				enemyScoreAdd = 2;
                dogReward_p = -1.0f;
                catReward_p = 1.0f;
            }
		} else {
			if (isBorderRight) {
				playerScoreAdd = -1;
				enemyScoreAdd = 1;
                dogReward_p = -1.0f;
                catReward_p = 1.0f;
            } else {
				playerScoreAdd = 1;
				enemyScoreAdd = -1;
                dogReward_p = 1.0f;
                catReward_p = -1.0f;
            }			
		}
		PlayerScore += playerScoreAdd;
		EnemyScore += enemyScoreAdd;
		playerScoretxt.text = string.Format ("{0}",PlayerScore);
		enemyScoretxt.text = string.Format ("{0}",EnemyScore);
		/*
		if (gameType == "InfinityWar"){
			WWWForm form = new WWWForm();
			if (isDogGame) {
				form.AddField ("AvengersScore", playerScoreAdd);
				form.AddField ("ThanosScore", enemyScoreAdd);
			} else {
				form.AddField ("AvengersScore", enemyScoreAdd);
				form.AddField ("ThanosScore", playerScoreAdd);
			}
			UnityWebRequest www = UnityWebRequest.Post (baseUrl + "scoreAdd_Avengers", form);
			www.SendWebRequest ();
			try{
				if(isDogGame){
					AddCurScore2(PlayerScore,EnemyScore);
				}else{
					AddCurScore2(EnemyScore,PlayerScore);
				}
			}catch{

			}
		}else{

			WWWForm form = new WWWForm();
			form.AddField ("DogScore",playerScoreAdd);
			form.AddField ("CatScore",enemyScoreAdd);
			UnityWebRequest www = UnityWebRequest.Post (baseUrl + "scoreAdd", form);
			www.SendWebRequest ();
			try{
				AddCurScore(EnemyScore,PlayerScore);
			}catch{
				
			}
			
		}
		*/
			

	}


	void Start () {
        /*
		int isWheelGame = Random.Range (0, 2);
		if (isWheelGame == 1) {
			Instantiate (Wheel, new Vector2(0.0f,-1.0f), Quaternion.identity);
		}
		*/
        Application.runInBackground = true;


        Time.timeScale = TIMESCALE;
		currenturl = Application.absoluteURL; //"https://infinite-reaches-12370.herokuapp.com/game/cat";//
		var array = currenturl.Split('/');
		try{
			gameType = array [3];
		}catch{
			gameType = "game";//"InfinityWar";
		}

		string baseU = "";
		for (int idx = 0 ; idx < Mathf.Min(3,array.Length); idx++ ) {
			baseU += array[idx] + "/";
		}
		baseUrl = baseU;

		playerScoretxt =  GameObject.Find ("PlayerScore").GetComponent<TextMesh> ();
		enemyScoretxt =  GameObject.Find ("EnemyScore").GetComponent<TextMesh> ();
        GameTime = GameObject.Find("GameTime").GetComponent<TextMesh>();

        isDogGame = currenturl.EndsWith ("dog");

		if (isDogGame) {
			DogPlayer.GetComponent<ComputerPlayer> ().enabled = false;
			DogPlayer.GetComponent<Player> ().enabled = true;	
			if (gameType == "InfinityWar") {
				Sprite thanosSprite = Resources.Load <Sprite> ("Sprites/thanos0");
				CatPlayer.GetComponent<Animator> ().enabled = false;
				CatPlayer.GetComponent<SpriteRenderer> ().sprite = thanosSprite;
			}

		} else {
			CatPlayer.GetComponent<ComputerPlayer> ().enabled = false;
			CatPlayer.GetComponent<Enemy> ().enabled = true;
			if (gameType == "InfinityWar") {
				Sprite thanosSprite = Resources.Load <Sprite> ("Sprites/thanos1");
				DogPlayer.GetComponent<Animator> ().enabled = false;
				DogPlayer.GetComponent<SpriteRenderer> ().sprite = thanosSprite;

			}
			/* below for mobile */

			/*
			Sprite newSprite =  Resources.Load <Sprite>("Sprites/Pullbtn_2");
			GameObject.Find ("ButtonPull").GetComponent<Image>().sprite = newSprite;

			*/
		}
		if (gameType == "InfinityWar") {
			Sprite stoneSprite = Resources.Load <Sprite> ("Sprites/gem0");
			Sprite bgSprite = Resources.Load <Sprite> ("Sprites/space");
			Sprite skull = Resources.Load <Sprite> ("Sprites/skull_w");
			var targetGen = GameObject.Find ("TargetGen").GetComponent<TargetGen> ();
			targetGen.coin.GetComponent<SpriteRenderer> ().sprite = stoneSprite;
			targetGen.skull.GetComponent<SpriteRenderer> ().sprite = skull;
			targetGen.isInfinityWar = true;
			GameObject.Find ("Background").GetComponent<SpriteRenderer> ().sprite = bgSprite;
			GameObject.Find ("UpBar").GetComponent<SpriteRenderer> ().color = new Color32 (182, 182, 182, 100);
			GameObject.Find ("Win").GetComponent<TextMesh> ().color = new Color32 (255, 255, 255, 255);
			GameObject.Find ("Lose").GetComponent<TextMesh> ().color = new Color32 (255, 255, 255, 255);
			GameObject.Find ("Tie").GetComponent<TextMesh> ().color = new Color32 (255, 255, 255, 255);

		} else {
			Sprite stoneSprite = Resources.Load <Sprite> ("Sprites/coin");
			Sprite bgSprite = Resources.Load <Sprite> ("Sprites/bgrimg"+Random.Range (0, 2).ToString());
			Sprite skull = Resources.Load <Sprite> ("Sprites/skull_w");
			var targetGen = GameObject.Find ("TargetGen").GetComponent<TargetGen> ();
			targetGen.coin.GetComponent<SpriteRenderer> ().sprite = stoneSprite;
			targetGen.coin.GetComponent<SpriteRenderer> ().color = new Color32 (255, 255, 255, 255);
			targetGen.skull.GetComponent<SpriteRenderer> ().sprite = skull;
			targetGen.isInfinityWar = false;
			//GameObject.Find ("Background").GetComponent<SpriteRenderer> ().sprite = bgSprite;
			//GameObject.Find ("UpBar").GetComponent<SpriteRenderer> ().color = new Color32 (0, 0, 0, 0);
			//GameObject.Find ("Win").GetComponent<TextMesh> ().color = new Color32 (0, 0, 0, 255);
			//GameObject.Find ("Lose").GetComponent<TextMesh> ().color = new Color32 (0, 0, 0, 255);
			//GameObject.Find ("Tie").GetComponent<TextMesh> ().color = new Color32 (0, 0, 0, 255);

		}

		PlayerScore = 0;
		EnemyScore = 0;
		coincnt = 0;
		skullcnt = 0;
		GAMETIME = 1.0f;
	}
	public void Action(int catAction,int dogAction){
		
		bool[] actions_dog = {false,false,false,false};
		bool[] actions_cat = {false,false,false,false};
		if (dogAction < 4)
			actions_dog [dogAction] = true;
		if (catAction < 4)
			actions_cat [catAction] = true;
		DogPlayer.GetComponent<Player_parent> ().Action (actions_dog);
		CatPlayer.GetComponent<Player_parent> ().Action (actions_cat);

		//print ("postition2 : "+dog_position.ToString() + " and "+cat_position.ToString());
	}
	// Update is called once per frame
	void Update () {
		
		if (isEnd) {
			/*
			if (!isSendPost) {
				int win = 0;int tie = 0; int lose = 0;
				if (PlayerScore == EnemyScore) {tie++;} 
				else if (PlayerScore > EnemyScore)
				{
					if (isDogGame) {win++;}
					else {lose++;}
				} else {
					if (isDogGame) {lose++;}
					else {win++;}
				}
				WWWForm form2 = new WWWForm();
				form2.AddField ("win",win);
				form2.AddField ("tie",tie);
				form2.AddField ("lose",lose);
				UnityWebRequest www = UnityWebRequest.Post (baseUrl + "gameresult", form2);
				www.SendWebRequest ();
				try{
					AddRecord (win, tie, lose);
				}catch{
				}
				isSendPost = true;
			}

			GameObject.Find ("TargetGen").GetComponent<TargetGen> ().endGame = true;
			GameObject.Find ("ContinueBtn").GetComponent<Image>().enabled = true;
			*/
			PlayerScore = 0;
			EnemyScore = 0;
			coincnt = 0;
			skullcnt = 0;
			catReward = 0;
			dogReward = 0;
            /*
			GameObject[] gObjs = GameObject.FindGameObjectsWithTag ("TargetCoin");
			for (int i = 0; i < gObjs.Length; i++) {
				Destroy (gObjs [i]);
			}
			GameObject[] skgObjs = GameObject.FindGameObjectsWithTag ("TargetSkull");
			for (int i = 0; i < skgObjs.Length; i++) {
				Destroy (skgObjs [i]);
			}
            */
		} else {
			GameTimeElapse ();

		}

		if (isEnd) {
			SceneManager.LoadScene( SceneManager.GetActiveScene().name );
			isEnd = false;
		}
		/*
		if (isEnd && CnInputManager.GetButtonDown ("Continue")) {
			SceneManager.LoadScene( SceneManager.GetActiveScene().name );
			isEnd = false;
		}
		*/

	}
}
