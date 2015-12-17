using UnityEngine;
using System.Collections;

public class LK3DController<T, TView>
	where T : LK3DController<T, TView>, new()
	where TView : LK3DView<TView, T>
{
	protected TView view;
	public GameObject View
	{
		get { return this.view.gameObject; }
	}

	static T _instance;
	public static T Instance 
	{
		get 
		{
			if (_instance == null)
			{
				_instance = new T();
			}
			return _instance;
		}
	}

	public void InitView(TView view)
	{
		this.view = view;
		this.Init();
	}

	protected void Init()
	{
		this.InitFirst();
		this.InitNet();
	}

	protected virtual void InitNet()
	{
	}

	protected virtual void InitFirst()
	{
	}

	public void Close()
	{
		this.CloseNet();
		this.CloseEnd();
		this.view = null;
	}

	protected virtual void CloseNet()
	{
	}

	protected virtual void CloseEnd()
	{
	}
}