using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class LKModel<T>
	where T : LKModel<T>, new()
{
	static T _instance;
	public static T Instance 
	{
		get 
		{
			if (_instance == null)
			{
				_instance = new T();
				_instance.Init();
			}
			return _instance;
		}
	}

	protected virtual void Init()
	{

	}
}