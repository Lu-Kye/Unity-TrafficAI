using UnityEngine;
using System.Collections;


[ExecuteInEditMode]
public class TrafficAIEditBase : MonoBehaviour 
{
	protected Vector3 initPos;
	protected Quaternion initRotation;
	protected Vector3 initScale;

	void Awake() 
	{
		this.initPos = this.transform.position;
		this.initRotation = this.transform.rotation;
		this.initScale = this.transform.localScale;

		// Init
		this.Init();
	}

	protected virtual void Init()
	{
		// Implemented by derive
	}

	protected virtual void Start()
	{
		// Implemented by derive
	}

	protected virtual void OnDestroy()
	{
		// Implemented by derive
	}

	void Update() 
	{
		this.RefreshPos();
		this.RefreshRotation();
		this.RefreshScale();

		// Step
		this.Step();
	}

	protected virtual void Step()
	{
		// Implemented by derive
	}

	protected virtual void RefreshPos()
	{
		this.transform.position = this.initPos;
	}

	protected virtual void RefreshRotation()
	{
		this.transform.rotation = this.initRotation;
	}

	protected virtual void RefreshScale()
	{
		this.transform.localScale = this.initScale;
	}	
}
