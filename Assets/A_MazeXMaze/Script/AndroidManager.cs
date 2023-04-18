using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class AndroidManager : MonoBehaviour {
	private static AndroidManager _instance;
	
	public static AndroidManager Instance
	{
		get
		{
			if (_instance == null)
			{
				_instance = FindObjectOfType(typeof(AndroidManager)) as AndroidManager;
				if (_instance == null)
				{
					_instance = new GameObject("AndroidManager").AddComponent<AndroidManager>();
				}
			}
			return _instance;
		}
	}

	private const int PlusLogin = 3;
	private const string CALL_ANDROID = "CallAndroid";
	private const int INVISIBLE_AD = 4;
	private const int VISIBLE_AD = 5;
	private const int VISIBLE_BIG_AD = 6;
	private bool m_isAdSetup = false;
	public bool m_isAdVisible = false;
	public string AD_UNIT_ID;
	public int m_GameCount = 0;

	public bool m_isSignin;
#if UNITY_ANDROID
	private static AndroidJavaObject _plugins;

	public bool m_isGameStartReady = true;
	
	void Awake()
	{
		if( Application.platform == RuntimePlatform.Android )
		{
			AndroidJavaClass Ajc = new AndroidJavaClass("com.unity3d.player.UnityPlayer"); 
			_plugins = Ajc.GetStatic<AndroidJavaObject>("currentActivity");
			DontDestroyOnLoad(this);
		}
	}

	// Use this for initialization
	void Start () {
		Screen.sleepTimeout = SleepTimeout.NeverSleep;
	}
	
	// Update is called once per frame
	void Update () {
		if(Input.GetKeyDown(KeyCode.Escape))
		{
			_plugins.Call("QuitApplication");
		}
	}
	
	public void FinishApp()
	{
		System.Diagnostics.Process.GetCurrentProcess().Kill();
		Application.Quit ();
	}
	
	public void setupAd()
	{
		if (Application.platform == RuntimePlatform.Android) {
			_plugins.Call ("setupAd", AD_UNIT_ID);
		}
	}

	public void AdInVisible()
	{
		if (m_isAdSetup) {
			CallAndroid (INVISIBLE_AD);
			m_isAdVisible = false;
		}
	}
	
	public void AdVisible()
	{
		if (m_isAdSetup) {
			CallAndroid (VISIBLE_AD);
			m_isAdVisible = true;
		}
	}

	public void ShowBigAd()
	{
		if( Application.platform == RuntimePlatform.Android )
			CallAndroid (VISIBLE_BIG_AD);
	}

	public void RefleshAd()
	{
		_plugins.Call("RefleshAd");
	}
/**/
	public void ShowToast(string text)
	{
		if( Application.platform == RuntimePlatform.Android )
			_plugins.Call("ToastMessage", text);
		else
			Debug.Log(text);
	}

	public void CallAndroid(int func, params object[] values)
	{
		_plugins.Call("CallAndroid", func);
	}

	public void AdSetupComplete()
	{
		m_isAdSetup = true;
	}
#endif
}
