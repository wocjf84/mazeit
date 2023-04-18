using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SocialPlatforms;
using UnityEngine.SceneManagement;

public class DataManager : MonoBehaviour {
	private static DataManager m_Instance;
	public static DataManager Instance
	{
		get
		{
			if( m_Instance == null )
			{
				m_Instance = FindObjectOfType(typeof(DataManager)) as DataManager;
				if( m_Instance == null )
				{
					m_Instance = new GameObject("DataManager").AddComponent<DataManager>();
				}
			}
			return m_Instance;
		}
	}
#if UNITY_ANDROID
	const string ACH_BRONZE_MEDAL = "CgkI1orI1-0KEAIQAA";
	const string ACH_SILVER_MEDAL = "CgkI1orI1-0KEAIQAg";
	const string ACH_GOLD_MEDAL = "CgkI1orI1-0KEAIQAw";
	const string ACH_BRONZE_TROPHY = "CgkI1orI1-0KEAIQBA";
	const string ACH_SILVER_TROPHY = "CgkI1orI1-0KEAIQBQ";
	const string ACH_GOLD_TROPHY = "CgkI1orI1-0KEAIQBg";
	const string LEAD_MAZE_EXPLORERS = "CgkI1orI1-0KEAIQAQ";
#elif UNITY_IPHONE
	const string ACH_BRONZE_MEDAL = "CgkI1tfOtsENEAIQAQ";
	const string ACH_SILVER_MEDAL = "CgkI1tfOtsENEAIQAw";
	const string ACH_GOLD_MEDAL = "CgkI1tfOtsENEAIQBA";
	const string ACH_BRONZE_TROPHY = "CgkI1tfOtsENEAIQBQ";
	const string ACH_SILVER_TROPHY = "CgkI1tfOtsENEAIQBg";
	const string ACH_GOLD_TROPHY = "CgkI1tfOtsENEAIQBw";
	const string LEAD_MAZE_EXPLORERS = "CgkI1tfOtsENEAIQAg";
#endif
	public const string BEST_SCORE = "BestScore";
	
	public string m_ADUnitId_Android;
	public string m_ADUnitId_IPhone;
	
	public string m_InterstitialId_Android;
	public string m_InterstitialId_IPhone;

	public GameObject m_ClosePopup;
	public Transform m_Root;

	public int m_Score;
	public int m_BestScore;
	public int m_Size = 10;
	public float m_Time = 30;
	public bool m_isSignin = false;
	public bool m_isGameStart = false;

	public bool m_isPopupOpen = false;

	void Awake() {
		DontDestroyOnLoad (this);
	}

	// Use this for initialization
	void Start () {
		m_BestScore = PlayerPrefs.GetInt (BEST_SCORE);
	}

	public void ShowBanner() {
#if UNITY_ANDROID
		AndroidManager.Instance.setupAd();
		AndroidManager.Instance.AdVisible();
#elif UNITY_IPHONE
#endif
	}
	
	// Update is called once per frame
	void Update () {
		if(Input.GetKeyDown(KeyCode.Escape))
		{
			if( !m_isPopupOpen ) {
				GameObject goClose = Instantiate(m_ClosePopup) as GameObject;
				goClose.transform.parent = m_Root;
				goClose.transform.localPosition = Vector3.zero;
				goClose.transform.localScale = Vector3.one;
				m_isPopupOpen = true;
			}
		}
	}

	public void onNext() {
		m_Score += (int)m_Time * (m_Size - 9);
		m_Time = 30;
		m_Size++;
		m_Size = m_Size > 29 ? 29 : m_Size;
		OPManager.setClear ();
		AudioManager.Instance.playOneShot ("clear");
		SceneManager.LoadScene("GameScene");
	}
	
	public void onNewGame() {
		m_Score = 0;
		m_Time = 30;
		m_Size = 10;
		OPManager.setClear ();
		SceneManager.LoadScene("GameScene");
	}
	
	public void onRanking() {
		if (m_isSignin) {
			Social.ShowLeaderboardUI ();
		} else {
			
		}
	}
	
	public void setAchiievement(int Score) {
		if( Score >= 100 ) {
			Social.ReportProgress(ACH_BRONZE_MEDAL, 100.0f, (bool success) => {
				
			});
		}
		if( Score >= 500 ) {
			Social.ReportProgress(ACH_SILVER_MEDAL, 100.0f, (bool success) => {
				
			});
		}
		if( Score >= 1000 ) {
			Social.ReportProgress(ACH_GOLD_MEDAL, 100.0f, (bool success) => {
				
			});
		}
		if( Score >= 10000 ) {
			Social.ReportProgress(ACH_BRONZE_TROPHY, 100.0f, (bool success) => {
				
			});
		}
		if( Score >= 100000 ) {
			Social.ReportProgress(ACH_SILVER_TROPHY, 100.0f, (bool success) => {
				
			});
		}
		if( Score >= 1000000 ) {
			Social.ReportProgress(ACH_GOLD_TROPHY, 100.0f, (bool success) => {
				
			});
		}
	}
	
	public void setScore(int score) {
		if (Social.localUser.authenticated) {
			Social.ReportScore(score, LEAD_MAZE_EXPLORERS, HandleScoreReported);
		}
	}
	
	private void HandleAuthenticated(bool success) {
		Debug.Log ("Authenticated is " + success);
		if (success) {
			Social.localUser.LoadFriends(HandleFriendsLoaded);
			m_isSignin = true;
		}
	}
	
	private void HandleFriendsLoaded(bool success) {
		foreach (IUserProfile friend in Social.localUser.friends) {
			Debug.Log("Friend = " + friend.ToString());
		}
	}
	
	private void HandleAchievementsLoaded(IAchievement[] achievements) {
		foreach (IAchievement achievement in achievements) {
			Debug.Log("achievement = " + achievement.ToString());
		}
	}
	
	private void HandleAchievementDescriptionsLoaded(IAchievementDescription[] achievementDescriptions) {
		foreach (IAchievementDescription achi in achievementDescriptions) {
			Debug.Log("achievementDescription = " + achi.ToString());
		}
	}
	
	private void HandleScoreReported(bool success) {
		Debug.Log ("HandleScoreReported: success = " + success);
	}
}
