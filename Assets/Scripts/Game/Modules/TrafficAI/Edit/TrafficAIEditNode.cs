using UnityEngine;
using System.Collections;
using System.Collections.Generic;


public class TrafficAIEditNode : TrafficAIEditBase 
{
	TrafficAIVoNode data;
	public TrafficAIVoNode Data
	{
		get { return this.data; }
		set
		{
			this.data = value;
			this.data.Edit = this;
		}
	}

	TrafficAIEditEdge edge;
	public TrafficAIEditEdge Edge
	{
		get { return this.edge; }
		set
		{
			this.edge = value;
		}
	}

	public void ReInit()
	{
		this.transform.position = this.Data.Pos;
		this.RefreshAll();
	}

	#region gameobject
	public GameObject Point
	{
		get { return UtilGameObject.FindInParent("Point", this.gameObject); }
	}
	#endregion

	TrafficAIEditNode[] nextList = new TrafficAIEditNode[8];
	LineRenderer[] arrowList = new LineRenderer[8]; 

	public TrafficAIEditNode Next
	{
		get { return this.nextList[0]; }
		set 
		{ 
			this.nextList[0] = value; 
			this.RefreshNexts();
		}
	}

	public void AddNext(TrafficAIEditNode node)
	{
		if (node == null)
		{
			Debug.LogError("TrafficAIEditNode::AddNext error node is null");
			return;
		}

		for (int i = 1, max = this.nextList.Length; i < max; i++)
		{
			var next = this.nextList[i];
			if (next != null)
				continue;

			this.nextList[i] = node;
			break;
		}
		this.RefreshNexts();

		// Add edge
		TrafficAIModel.Instance.AddEdge(this.Data, node.Data);
	}

	public void DeleteNext(TrafficAIEditNode node)
	{
		for (int i = 0, max = this.nextList.Length; i < max; i++)
		{
			var next = this.nextList[i];
			if (next != node)
				continue;

			this.nextList[i] = null;

			// Delete edge
			TrafficAIModel.Instance.RemoveEdge(this.Data, node.Data);

			break;
		}
		this.RefreshNexts();
	}

	#region override
	protected override void OnDestroy()
	{
		// Delete edge
		for (int i = 0, max = this.nextList.Length; i < max; i++)
		{
			var next = this.nextList[i];
			if (next == null)
				continue;
			
			// Delete edge
			TrafficAIModel.Instance.RemoveEdge(this.Data, next.Data);
		}

		// Remove arrows
		for (int i = 0, max = this.arrowList.Length; i < max; i++)
		{
			var arrow = this.arrowList[i];
			if (arrow == null)
				continue;

			if (arrow.gameObject != null && !ReferenceEquals(arrow.gameObject, null))
				GameObject.Destroy(arrow.gameObject);
		}

		// Remove data
		if (this.Data != null)
			TrafficAIModel.Instance.RemoveNode(this.Data);

		// Delete edge
		if (this.Edge != null)
			GameObject.Destroy(this.Edge.gameObject);
	}

	protected override void RefreshPos()
	{
		var pos = this.transform.position;
		pos.y = this.initPos.y;
		this.transform.position = pos;
	}

	protected override void Step()
	{
		this.RefreshAll();
	}
	#endregion

	public void RefreshAll()
	{
		this.RefreshName();
		this.RefreshNexts();
	}

	public void RefreshName()
	{
		if (this.Data == null)
			return;
		this.name = "Node " + this.Data.Id.ToString();
	}

	public void RefreshNexts()
	{
		for (int i = 0, max = this.nextList.Length; i < max; i++)
		{
			var next = this.nextList[i];
			var arrow = this.arrowList[i];
			if (next == null)
			{
				if (arrow != null)
				{
					GameObject.DestroyImmediate(arrow);
					this.arrowList[i] = null;
				}
				continue;
			}

			if (arrow == null)
			{
				arrow = this.arrowList[i] = TrafficAIEdit.Instance.AddArrow();
				arrow.name = "Arrow(type:" + this.Data.Type + ")" + this.Data.Id + "-" + next.Data.Id;
			}

			// Arrow positions
			arrow.SetPosition(1, this.Point.transform.position);
			arrow.SetPosition(0, next.Point.transform.position);
		}
	}
}
