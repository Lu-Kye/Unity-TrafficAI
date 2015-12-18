using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class TrafficAIRole : MonoBehaviour 
{
	public TrafficAIVoRole Data;

	// Behavior tree
	public TrafficAIRoleTree Tree;

	public TrafficAIVoNode StartNode;
	public TrafficAIVoNode EndNode;

	// Current node which role at
	int curIndex = 0;
	public TrafficAIVoNode CurNode
	{
		get 
		{
			if (this.curIndex >= this.movingPath.Count)
				return null;
			return this.movingPath[this.curIndex];
		}
	}

	// Next node which role moves to
	int nextIndex = 1;
	public TrafficAIVoNode NextNode
	{
		get 
		{
			if (this.nextIndex >= this.movingPath.Count)
				return null;
			return this.movingPath[this.nextIndex];
		}
	}

	void Start()
	{
		// Init first
		this.Init();

		// Run 
		this.Tree.Run();	
	}

	void Update()
	{
		this.Tree.Update();
	}

	void OnDestroy()
	{
		this.Close();	
	}

	void Init()
	{
		this.InitMove();
	}

	void Close()
	{
		this.CloseMove();
	}

	#region action move
	// Moving velocity
	public float Velocity = 5f;

	// Min distance of which when current position and next node distance smaller than it, 
	// then change to next node
	public float MinDistance = 0.1f;

	// Moving movingPath
	List<TrafficAIVoNode> movingPath = new List<TrafficAIVoNode>();

	public bool IsArrived
	{
		get { return this.CurNode == this.EndNode; }
	}

	void InitMove()
	{
		this.movingPath = TrafficAIModel.Instance.GetPath(this.StartNode.Type, this.StartNode.Id, this.EndNode.Id);
		this.CurNode.IsArrivable = false;
		this.Velocity = Random.Range(8f, 15f);
	}

	void CloseMove()
	{
		this.CurNode.IsArrivable = true;
	}

	protected virtual bool MoveEnable
	{
		get
		{
			return this.NextNode != null && this.NextNode.IsArrivable;
		}
	}

	protected virtual void Move(float delta)
	{
		var dir = (this.NextNode.Pos - this.CurNode.Pos).normalized;
		var pos = this.transform.position + dir * delta * this.Velocity;
		pos.y = 0.5f;

		// Set pos
		this.transform.position = pos;
	}

	public bool TryMove(float delta)
	{
		if (this.MoveEnable)
			this.Move(delta);
		return false;
	}

	protected virtual bool MoveToNextEnable
	{
		get
		{
			var disMax = Vector3.Distance(this.CurNode.Pos, this.NextNode.Pos);
			var disToPre = Vector3.Distance(this.transform.position, this.CurNode.Pos);
			if (disToPre >= disMax)
				return true;

			var disToNext = Vector3.Distance(this.transform.position, this.NextNode.Pos);
			if (disToNext > this.MinDistance)
				return false;
			
			return true;
		}
	}

	protected virtual void MoveToNext()
	{
		this.CurNode.IsArrivable = true;
		this.NextNode.IsArrivable = false;
		this.curIndex = this.nextIndex;
		this.nextIndex++;
	}

	public bool TryMoveToNext()
	{
		if (this.MoveToNextEnable)
			this.MoveToNext();
		return false;
	}
	#endregion

	#region action stop
	protected virtual bool StopEnable
	{
		get
		{
			return this.NextNode != null && !this.NextNode.IsArrivable;
		}
	}

	protected virtual void Stop()
	{
		// Nothing to do now
	}

	public bool TryStop()
	{
		if (this.StopEnable)
			this.Stop();
		return false;
	}
	#endregion

	#region action destroy
	protected virtual bool DestroyEnable
	{
		get
		{
			return this.NextNode == null || this.CurNode == this.EndNode;
		}
	}

	protected virtual void Destroy()
	{
		this.CurNode.IsArrivable = true;
		GameObject.DestroyImmediate(this.gameObject);
	}

	public bool TryDestroy()
	{
		if (this.DestroyEnable)
			this.Destroy();
		return false;
	}
	#endregion
}
