using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class AudioManager : MonoBehaviour {
	private static AudioManager m_Instance;
	public static AudioManager Instance
	{
		get
		{
			if( m_Instance == null )
			{
				m_Instance = FindObjectOfType(typeof(AudioManager)) as AudioManager;
				if( m_Instance == null )
				{
					m_Instance = new GameObject("AudioManager").AddComponent<AudioManager>();
				}
			}
			return m_Instance;
		}
	}

	public AudioSource m_Audio;
	public List<AudioClip> m_AudioList;
	public Dictionary<string, AudioClip> m_AudioDictionart = new Dictionary<string, AudioClip>();

	void Awake() {
		DontDestroyOnLoad (this);
	}

	// Use this for initialization
	void Start () {
		for (int i = 0; i < m_AudioList.Count; i++) {
			m_AudioDictionart.Add (m_AudioList [i].name, m_AudioList [i]);
		}
	}
	
	// Update is called once per frame
	void Update () {

	}

	public void playOneShot(string name) {
		m_Audio.PlayOneShot (m_AudioDictionart[name]);
	}
}
