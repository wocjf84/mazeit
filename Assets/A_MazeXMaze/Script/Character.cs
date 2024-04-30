using UnityEngine;
using System.Collections;

public class Character : MonoBehaviour {
	public int m_Now_X;
	public int m_Now_Y;

	const int RIGHT = 0;
	const int DOWN = 1;
	const int LEFT = 2;
	const int UP = 3;
	
	Vector3[] DIRECTION = {new Vector3 (1, 0, 0), new Vector3 (0, 0, -1), new Vector3 (-1, 0, 0), new Vector3 (0, 0, 1)};

	public int m_Direction = 0;

	public bool m_isMove = false;
	public int m_MoveCount = 0;
	public bool m_isBack = false;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void FixedUpdate () {
		if (!DataManager.Instance.m_isGameStart) {
			return;
		}
		if (m_isMove) {
			m_MoveCount++;
			Vector3 position = transform.localPosition;
			switch (m_Direction) {
			case UP:
				position.z += 0.04f;
				if( m_MoveCount > 4 ) {
					m_Now_Y--;
				}
				break;
			case DOWN:
				position.z -= 0.04f;
				if( m_MoveCount > 4 ) {
					m_Now_Y++;
				}
				break;
			case RIGHT:
				position.x += 0.04f;
				if( m_MoveCount > 4 ) {
					m_Now_X++;
				}
				break;
			case LEFT:
				position.x -= 0.04f;
				if( m_MoveCount > 4 ) {
					m_Now_X--;
				}
				break;
			}
			if( m_MoveCount == 3 ) {
				if( m_isBack ) {
					GameManager.Instance.removeLine();
				} else {
					GameManager.Instance.setLine(m_Now_X, m_Now_Y, m_Direction);
				}
			}
			transform.localPosition = position;

			if( m_MoveCount > 4 ) {
				m_MoveCount = 0;
				AudioManager.Instance.playOneShot ("move");
				if( checkWallCount() != 2 ) {
					m_isMove = false;
					m_isBack = false;
				} else {
					nextDirection();
				}
			}
		}
	}

	public int checkWallCount()
	{
		int wallCount = 0;

		RaycastHit hit;

		for (int i = 0; i < 4; i++) {
			if (Physics.Raycast (transform.position, DIRECTION[i], out hit, 0.2f)) {
				if (hit.collider.tag.Equals("Wall")) {
					wallCount++;
				}
			}
		}

		return wallCount;
	}

	public void nextDirection() {
		RaycastHit hit;
		
		for (int i = 0; i < 4; i++) {
			if( !m_isBack ) {
				if (!Physics.Raycast (transform.position, DIRECTION[i], out hit, 0.2f)) {
					m_Direction = i;
					break;
				}
			} else {
				if (Physics.Raycast (transform.position, DIRECTION[i], out hit, 0.2f)) {
					if( hit.collider.tag.Equals("Line") ) {
						m_Direction = i;
						break;
					}
				}
			}
		}
	}

	public string getDirectionObjectTag(int direction) {
		RaycastHit hit;
		string stRet = "";
		if (Physics.Raycast (transform.position, DIRECTION[direction], out hit, 0.2f)) {
			stRet = hit.collider.tag;
		}
		return stRet;
	}

	public void OnTriggerEnter(Collider other) {
		if (other.tag.Equals ("Tile") && GameManager.Instance.m_isLoading) {
			DataManager.Instance.onNext();
		}
	}
}
