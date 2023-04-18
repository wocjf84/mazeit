using UnityEngine;
using System.Collections;

public class ClosePopup : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void onYes() {
		DataManager.Instance.m_isPopupOpen = false;
		System.Diagnostics.Process.GetCurrentProcess().Kill();
		Application.Quit ();
	}

	public void onNo() {
		DataManager.Instance.m_isPopupOpen = false;
		Destroy (gameObject);
	}
}
