using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class OPManager {
	public static Hashtable objectpools = new Hashtable();

	public static void Create<T>(string _resourcePath, int _size)
	{
		Object obj = Resources.Load (_resourcePath);
		Stack<GameObject> objects = new Stack<GameObject> (_size);
		for( int i = 0; i < _size; i++ )
		{
			GameObject gameObj = GameObject.Instantiate(obj, Vector3.zero, Quaternion.identity) as GameObject;
//			gameObj.transform.parent = GameManager.Instance.m_ObjectPool;
			gameObj.SetActive(false);

			objects.Push(gameObj);
		}
		objectpools.Add (typeof(T).ToString (), objects);
	}

	public static GameObject getGameObject<T>()
	{
		Stack<GameObject> objects = (Stack<GameObject>)objectpools [typeof(T).ToString ()];

		GameObject gameObj = objects.Pop ();
		gameObj.SetActive (true);

		return gameObj;
	}

	public static void Release<T>(GameObject _gameObj)
	{
		Stack<GameObject> objects = (Stack<GameObject>)objectpools [typeof(T).ToString ()];

//		_gameObj.transform.parent = GameManager.Instance.m_ObjectPool;
		objects.Push (_gameObj);
		_gameObj.SetActive (false);
	}

	public static GameObject getGameObject(string kind)
	{
		Stack<GameObject> objects = (Stack<GameObject>)objectpools [kind];
		
		GameObject gameObj = objects.Pop ();
		gameObj.SetActive (true);
		
		return gameObj;
	}

	public static void setClear()
	{
		objectpools.Clear ();
	}
}
