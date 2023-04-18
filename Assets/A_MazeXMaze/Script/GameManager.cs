using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GameManager : MonoBehaviour {
	private static GameManager m_Instance;
	public static GameManager Instance
	{
		get
		{
			if( m_Instance == null )
			{
				m_Instance = FindObjectOfType(typeof(GameManager)) as GameManager;
				if( m_Instance == null )
				{
					m_Instance = new GameObject("GameManager").AddComponent<GameManager>();
				}
			}
			return m_Instance;
		}
	}
	public GameObject m_Tile;
	public GameObject m_Wall_Port;
	public GameObject m_Wall_Land;
	public GameObject m_MazeMaker;
	public Material m_StartTile;
	public Material m_EndTile;

	public UILabel m_TimeLabel;
	public UILabel m_ScoreLabel;

	public UILabel m_ResultScore;
	public UILabel m_ResultBest;
	public UISprite m_ResultMedal;
	public UIButton m_Restart;
	public UIButton m_Ranking;
	public GameObject m_ResultPopup;
	public GameObject m_Bottom;

	public UISprite m_Count;
	public GameObject m_Ready;

	public int m_Max_X;
	public int m_Max_Y;

	const int DEFINE_X = 12;

	const int RIGHT = 0;
	const int DOWN = 1;
	const int LEFT = 2;
	const int UP = 3;

	public bool m_isLoading = false;
	
	Vector3[] DIRECTION = {new Vector3 (0, 90, 0), new Vector3 (0, 180, 0), new Vector3 (0, 270, 0), new Vector3 (0, 0, 0)};

	private List<GameObject> m_TileList = new List<GameObject>();
	private List<GameObject> m_LineList = new List<GameObject>();
	// Use this for initialization
	void Start () {
		OPManager.Create<Line> ("Line", DataManager.Instance.m_Size * DataManager.Instance.m_Size);
		m_ScoreLabel.text = DataManager.Instance.m_Score.ToString ();
		m_Max_X = m_Max_Y = DataManager.Instance.m_Size;
		for( int i = 0; i < m_Max_X * m_Max_Y; i++ )
		{
			int x = i % m_Max_X;
			int y = i / m_Max_X;
			GameObject goTile = Instantiate(m_Tile) as GameObject;
			goTile.transform.parent = transform;
			goTile.transform.localPosition = new Vector3(x * 0.2f, 0, y * -0.2f);
			m_TileList.Add(goTile);
			if( x == 0 )
			{
				GameObject goWall_End = Instantiate(m_Wall_Port) as GameObject;
				goWall_End.transform.parent = transform;
				goWall_End.transform.localPosition = new Vector3((x * 0.2f) - 0.1f, 0.075f, (y * -0.2f));
			}
			if( y == 0 )
			{
				GameObject goWall_End = Instantiate(m_Wall_Land) as GameObject;
				goWall_End.transform.parent = transform;
				goWall_End.transform.localPosition = new Vector3((x * 0.2f), 0.075f, (y * -0.2f) + 0.1f);
			}
			GameObject goWall_Port = Instantiate(m_Wall_Port) as GameObject;
			goWall_Port.transform.parent = transform;
			goWall_Port.transform.localPosition = new Vector3((x * 0.2f) + 0.1f, 0.075f, y * -0.2f);
			GameObject goWall_Land = Instantiate(m_Wall_Land) as GameObject;
			goWall_Land.transform.parent = transform;
			goWall_Land.transform.localPosition = new Vector3((x * 0.2f), 0.075f, (y * -0.2f) - 0.1f);
		}
		if (m_Max_X - DEFINE_X > 0) {
//			float xz = Mathf.Abs(m_Max_X - DEFINE_X) * 0.08f;
//			transform.localScale = new Vector3(1 - xz, 1, 1 - xz);
		}
		int startPosition = 0;//Random.Range (0, m_Max_X * m_Max_Y);
		int ux = startPosition % m_Max_X;
		int uy = startPosition / m_Max_X;
		/*
		m_MazeMaker.transform.localPosition = m_TileList[startPosition].transform.localPosition;
		m_TileList [startPosition].GetComponent<Renderer>().material = m_StartTile;
		m_TileList [startPosition].GetComponent<Tile> ().m_Check = true;/**/
		m_MazeMaker.GetComponent<MazeMaker> ().MakeMaze (ux, uy);
		int endPosition = (m_Max_X * m_Max_Y) - 1;
		m_TileList [endPosition].GetComponent<Renderer>().material = m_EndTile;

		Vector3 position = transform.parent.localPosition;
		position.x -= DataManager.Instance.m_Size * 0.1f;
		position.z += DataManager.Instance.m_Size * 0.1f;
		transform.parent.localPosition = position;

		for( int i = 0; i < m_TileList.Count - 1; i++ ) {
			m_TileList[i].SetActive(false);
		}

		m_isLoading = true;
		if (!DataManager.Instance.m_isGameStart) {
			StartCoroutine (setReady ());
		} else {
			m_Ready.SetActive(false);
		}
	}

	public Tile getTile(int x, int y)
	{
		return m_TileList [y * m_Max_X + x].GetComponent<Tile>();
	}
	
	// Update is called once per frame
	void Update () {
		if (!DataManager.Instance.m_isGameStart) {
			return;
		}
		DataManager.Instance.m_Time -= Time.deltaTime;
		int second = (int)DataManager.Instance.m_Time;
		second = second < 0 ? 0 : second;
		int ms = (int)((DataManager.Instance.m_Time % 1) * 100);
		ms = ms < 0 ? 0 : ms;
		if (second <= 5) {
			if(ms < 2) {
				AudioManager.Instance.playOneShot ("timelimit");
			}
			m_TimeLabel.color = Color.red;
		}
		m_TimeLabel.text = string.Format ("{0}:{1:00}", second, ms);
		if (DataManager.Instance.m_Time < 0) {
			setResult();
			DataManager.Instance.m_isGameStart = false;
		}
	}

	public void setResult() {
		AudioManager.Instance.playOneShot ("gameover");
		m_Bottom.SetActive (false);
		m_ResultPopup.SetActive (true);
		m_Restart.isEnabled = false;
		m_Ranking.isEnabled = false;
		StartCoroutine (setResultScore(DataManager.Instance.m_Score));
	}

	public void setLine(int x, int y, int direction) {
		GameObject line = OPManager.getGameObject<Line> ();
		line.transform.parent = transform.parent;
		line.transform.localPosition = getTile (x, y).transform.localPosition;
		line.transform.localEulerAngles = DIRECTION [direction];
		m_LineList.Add (line);
	}

	public void removeLine() {
		if (m_LineList.Count > 0) {
			int last = m_LineList.Count - 1;
			OPManager.Release<Line> (m_LineList [last]);
			m_LineList.RemoveAt (last);
		}
	} 

	public IEnumerator setResultScore(int score) {
		m_ResultBest.text = DataManager.Instance.m_BestScore.ToString ();
		for( int i = 1; i <= 30; i++ ) {
			int nowScore = score * i / 30;
			m_ResultScore.text = nowScore.ToString();
			if( DataManager.Instance.m_BestScore < nowScore ) {
				m_ResultBest.text = nowScore.ToString();
			}
			yield return new WaitForSeconds(0.02f);
		}
		
		DataManager.Instance.setAchiievement(score);
		if( score >= 100 ) {
			m_ResultMedal.spriteName = string.Format("medal{0}", 0);
			m_ResultMedal.MakePixelPerfect ();
		}
		if( score >= 500 ) {
			m_ResultMedal.spriteName = string.Format("medal{0}", 1);
			m_ResultMedal.MakePixelPerfect ();
		}
		if( score >= 1000 ) {
			m_ResultMedal.spriteName = string.Format("medal{0}", 2);
			m_ResultMedal.MakePixelPerfect ();
		}
		if( score >= 10000 ) {
			m_ResultMedal.spriteName = string.Format("medal{0}", 3);
			m_ResultMedal.MakePixelPerfect ();
		}
		if( score >= 100000 ) {
			m_ResultMedal.spriteName = string.Format("medal{0}", 4);
			m_ResultMedal.MakePixelPerfect ();
		}
		if( score >= 1000000 ) {
			m_ResultMedal.spriteName = string.Format("medal{0}", 5);
			m_ResultMedal.MakePixelPerfect ();
		}
		if (DataManager.Instance.m_BestScore < score) {
			DataManager.Instance.m_BestScore = score;
			PlayerPrefs.SetInt(DataManager.BEST_SCORE, score);
			DataManager.Instance.setScore(score);
		}
		m_Restart.isEnabled = true;
		m_Ranking.isEnabled = true;
	}

	public IEnumerator setReady() {
		m_Count.spriteName = "count_3";
		m_Count.MakePixelPerfect ();
		AudioManager.Instance.playOneShot ("timelimit");
		yield return new WaitForSeconds(1.0f);
		m_Count.spriteName = "count_2";
		m_Count.MakePixelPerfect ();
		AudioManager.Instance.playOneShot ("timelimit");
		yield return new WaitForSeconds(1.0f);
		m_Count.spriteName = "count_1";
		m_Count.MakePixelPerfect ();
		AudioManager.Instance.playOneShot ("timelimit");
		yield return new WaitForSeconds(1.0f);
		m_Count.spriteName = "count_start";
		m_Count.MakePixelPerfect ();
		AudioManager.Instance.playOneShot ("clear");
		yield return new WaitForSeconds(1.0f);
		m_Ready.SetActive (false);
		DataManager.Instance.m_isGameStart = true;
	}

	public void onNewgame() {
		DataManager.Instance.onNewGame ();
	}

	public void onRanking() {
		DataManager.Instance.onRanking ();
	}
}
