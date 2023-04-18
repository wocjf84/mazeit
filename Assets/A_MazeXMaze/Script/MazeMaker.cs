using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MazeMaker : MonoBehaviour {
	public int m_Now_X;
	public int m_Now_Y;
	public GameObject m_Loading;
	List<int> m_MoveVector = new List<int>();

	const int UP = 0;
	const int DOWN = 1;
	const int LEFT = 2;
	const int RIGHT = 3;
	List<int> m_CellStack = new List<int>();

	bool m_MakeStart = false;
	int m_Count = 0;
	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
		if (m_MakeStart) {
			if( !m_CellStack.Contains(m_Now_Y * GameManager.Instance.m_Max_X + m_Now_X) )
				m_CellStack.Add (m_Now_Y * GameManager.Instance.m_Max_X + m_Now_X);
			GameManager.Instance.getTile(m_Now_X, m_Now_Y).m_Check = true;
			int[] position = cellSearch();
			if( position[0] >= 0 )
			{
				MoveToMerge(position);
				m_Count++;
			}
			else
			{
				PreviousCell();
			}
		}
	}

	void test()
	{
		if( !m_CellStack.Contains(m_Now_Y * GameManager.Instance.m_Max_X + m_Now_X) )
			m_CellStack.Add (m_Now_Y * GameManager.Instance.m_Max_X + m_Now_X);
		GameManager.Instance.getTile(m_Now_X, m_Now_Y).m_Check = true;
		int[] position = cellSearch();
		if( position[0] >= 0 )
		{
			MoveToMerge(position);
			m_Count++;
		}
		else
		{
			PreviousCell();
		}
	}

	public void MakeMaze(int x, int y)
	{
		m_MoveVector.Add (0);
		m_MoveVector.Add (1);
		m_MoveVector.Add (2);
		m_MoveVector.Add (3);

		m_Now_X = x;
		m_Now_Y = y;

		test ();
		//m_MakeStart = true;
	}

	public void MoveToMerge(int[] position)
	{
		switch (position [2]) {
		case UP:
			transform.localEulerAngles = new Vector3(0, 0, 0);
			break;
		case DOWN:
			transform.localEulerAngles = new Vector3(0, 180, 0);
			break;
		case LEFT:
			transform.localEulerAngles = new Vector3(0, -90, 0);
			break;
		case RIGHT:
			transform.localEulerAngles = new Vector3(0, 90, 0);
			break;
		}

		RaycastHit hit;
		
		if (Physics.Raycast (transform.position, transform.forward, out hit, 0.5f)) {
			if( hit.collider.gameObject.tag == "Wall" )
			{
				Destroy(hit.collider.gameObject);
			}
		}

		m_Now_X = position [0];
		m_Now_Y = position [1];
		transform.localPosition = GameManager.Instance.getTile (m_Now_X, m_Now_Y).transform.localPosition;

		test ();
	}

	public void PreviousCell()
	{
		if (m_CellStack.Count >= GameManager.Instance.m_Max_X * GameManager.Instance.m_Max_Y) {
			m_MakeStart = false;
			m_Loading.SetActive(false);
			return;
		}
		m_Count--;
		if (m_Count < 0)
			m_Count = m_CellStack.Count - 1;
		int position = m_CellStack [m_Count];
		m_Now_X = position % GameManager.Instance.m_Max_X;
		m_Now_Y = position / GameManager.Instance.m_Max_X;

		transform.localPosition = GameManager.Instance.getTile (m_Now_X, m_Now_Y).transform.localPosition;
		test ();
	}

	public int[] cellSearch()
	{
		int[] iRet = new int[3];
		iRet [0] = -1;
		iRet [1] = -1;
		iRet [2] = -1;
		m_MoveVector = shuffle (m_MoveVector);
		for (int i = 0; i < m_MoveVector.Count; i++) {
			int x = m_Now_X;
			int y = m_Now_Y;
			switch( m_MoveVector[i] )
			{
			case UP:
				y--;
				break;
			case DOWN:
				y++;
				break;
			case LEFT:
				x--;
				break;
			case RIGHT:
				x++;
				break;
			}
			if( x < 0 || x > GameManager.Instance.m_Max_X - 1 || y < 0 || y > GameManager.Instance.m_Max_Y - 1 )
				continue;
			else if( GameManager.Instance.getTile(x, y).m_Check )
				continue;
			else
			{
				iRet[0] = x;
				iRet[1] = y;
				iRet[2] = m_MoveVector[i];
			}
		}
		return iRet;
	}

	public List<int> shuffle(List<int> target)
	{
		List<int> retList = new List<int> ();
		int count = target.Count;
		for (int i = 0; i < count; i++) {
			int rand = Random.Range(0, target.Count);
			retList.Add(target[rand]);
			target.RemoveAt(rand);
		}
		return retList;
	}
}
