using UnityEngine;
using System.Collections;

public class LK3DView<T, TController> : MonoBehaviour 
	where T : LK3DView<T, TController>
	where TController : LK3DController<TController, T>, new()
{
	protected TController controller;

	void Start()
	{
		this.Init();
	}

	void OnDestroy()
	{
		if (this.controller != null)
			this.controller.Close();
		this.Close();
	}

	protected virtual void Init()
	{
		// Implemented by derived class
		// Should construct controller and init controller
	}

	protected virtual void Close()
	{
	}
}
