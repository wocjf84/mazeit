using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CameraHandle : MonoBehaviour {
	bool m_isTouch = false;
	Vector3 m_MousePosition;
	public const string TILE_TAG = "Tile";
	public Character m_Character;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		if (!DataManager.Instance.m_isGameStart || m_Character.m_isMove) {
			return;
		}
		if( Input.GetMouseButtonDown(0) )
		{
			m_isTouch = true;
			m_MousePosition = Input.mousePosition;
		}
		if( Input.GetMouseButton(0) )
		{
			if( m_MousePosition.x + 20 < Input.mousePosition.x ) {
				string tag = m_Character.getDirectionObjectTag(0);
				if( !tag.Equals("Wall") ) {
					if( tag.Equals("Line") ) {
						m_Character.m_isBack = true;
					}
					m_Character.m_Direction = 0;
					m_Character.m_isMove = true;
				}
				m_isTouch = false;
			}
			else if( m_MousePosition.x - 20 > Input.mousePosition.x ) {
				string tag = m_Character.getDirectionObjectTag(2);
				if( !tag.Equals("Wall") ) {
					if( tag.Equals("Line") ) {
						m_Character.m_isBack = true;
					}
					m_Character.m_Direction = 2;
					m_Character.m_isMove = true;
				}
				m_isTouch = false;
			}
			else if( m_MousePosition.y + 20 < Input.mousePosition.y ) {
				string tag = m_Character.getDirectionObjectTag(3);
				if( !tag.Equals("Wall") ) {
					if( tag.Equals("Line") ) {
						m_Character.m_isBack = true;
					}
					m_Character.m_Direction = 3;
					m_Character.m_isMove = true;
				}
				m_isTouch = false;
			}
			else if( m_MousePosition.y - 20 > Input.mousePosition.y ) {
				string tag = m_Character.getDirectionObjectTag(1);
				if( !tag.Equals("Wall") ) {
					if( tag.Equals("Line") ) {
						m_Character.m_isBack = true;
					}
					m_Character.m_Direction = 1;
					m_Character.m_isMove = true;
				}
				m_isTouch = false;
			}
		}
		if (Input.GetMouseButtonUp (0)) {
			m_isTouch = false;
		}
	}
	
	public void Notify (GameObject go, string funcName, object obj)
	{
		if (go != null)
		{
			go.SendMessage(funcName, obj, SendMessageOptions.DontRequireReceiver);
		}
	}
	
}