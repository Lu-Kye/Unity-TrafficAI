using UnityEngine;
using System.Collections;
using System.Collections.Generic;


public class TrafficAIEditEdge : TrafficAIEditBase
{
	TrafficAIVoEdge data;
	public TrafficAIVoEdge Data
	{
		get { return this.data; }
		set
		{
			this.data = value;
			this.data.Edit = this;
			this.NodeStart.Data = this.data.Start;
			this.NodeStart.Edge = this;
			this.NodeEnd.Data = this.data.End;
			this.NodeEnd.Edge = this;
		}
	}

	public void ReInit()
	{
		this.transform.position = this.Data.Pos;
		this.transform.rotation = this.Data.Rotation;
		this.Distance = this.Data.Distance;
		this.Size = this.Data.Size;
		this.RefreshAll();
	}

	public GameObject Road
	{
		get { return UtilGameObject.FindInParent("Road", this.gameObject); }
	}

	public TrafficAIEditNode NodeStart
	{
		get;
		private set;
	}

	public TrafficAIEditNode NodeEnd
	{
		get;
		private set;
	}

	List<TrafficAIEditNode> nodeList = new List<TrafficAIEditNode>();

	// Distance
	public float Distance = 2.0f;

	// Size
	public int Size = 1;

	#region override
	protected override void Init()
	{
		this.NodeStart = UtilGameObject.FindInParent("NodeStart", this.gameObject)
			.AddComponent<TrafficAIEditNode>();
		this.NodeStart.gameObject.AddComponent<TrafficAIEditNodeLink>();

		this.NodeEnd = UtilGameObject.FindInParent("NodeEnd", this.gameObject)
			.AddComponent<TrafficAIEditNode>();
		this.NodeEnd.gameObject.AddComponent<TrafficAIEditNodeLink>();
	}

	protected override void Start()
	{
	}

	protected override void OnDestroy()
	{
		TrafficAIModel.Instance.RemoveEdge(this.Data);
	}

	protected override void RefreshPos()
	{
		var pos = this.transform.position;
		pos.y = this.initPos.y;
		this.transform.position = pos;
	}
	
	protected override void RefreshRotation()
	{
		var euler = this.transform.localEulerAngles;
		euler.y = 0;
		this.transform.Rotate(-euler);
	}

	protected override void Step()
	{
		this.RefreshAll();
	}
	#endregion

	public void RefreshAll()
	{
		this.RefreshName();
		this.RefreshRoad();
		this.RefreshNodes();
	}

	public void RefreshName()
	{
		if (this.NodeStart.Data == null || this.NodeEnd.Data == null)
			return;
		this.name = "Edge " + this.NodeStart.Data.Id.ToString() + "-" + this.NodeEnd.Data.Id.ToString();
	}

	public void RefreshRoad()
	{
		var scale = this.Road.transform.localScale;
		scale.x = this.Size * this.Distance;
		this.Road.transform.localScale = scale;
	}

	#region nodes
	void ClearNodes()
	{
		this.nodeList.ForEach((e) => { 
			e.Edge = null;
			GameObject.DestroyImmediate(e.gameObject);
		});
		this.nodeList.Clear();
	}

	void AddNodes()
	{
		var start = this.NodeStart.Data.Id;
		var type = this.NodeStart.Data.Type;
		for (int i = 1; i < this.Size; i++)
		{
			var prefab = this.NodeStart.gameObject;
			var parent = this.NodeStart.transform.parent.gameObject;
			var go = UtilGameObject.CreateByGO(prefab, parent);

			var script = go.GetComponent<TrafficAIEditNode>();
			script.Edge = this;
			script.Data = TrafficAIModel.Instance.AddNode(type, start + i);
			script.Data.Edit = script;
			script.Data.IsEdge = true;
			script.Point.transform.localScale = Vector3.one / 2;
			script.RefreshAll();

			this.nodeList.Add(script);
		}
	}

	int _size;
	public void RefreshNodes()
	{
		if (this.NodeStart == null || this.NodeEnd == null)
			return;

		if (this._size != this.Size)
		{
			this.ClearNodes();
			this.AddNodes();
		}
		this._size = this.Size;

		var pos1 = this.NodeStart.transform.localPosition;
		pos1.x = - this.Size * this.Distance / 2;
		this.NodeStart.transform.localPosition = pos1;

		var cur = this.NodeStart;
		for (int i = 0, max = this.nodeList.Count; i < max; i++)
		{
			var node = this.nodeList[i];
			var pos = pos1 + new Vector3((i + 1) * this.Distance, 0, 0);
			node.transform.localPosition = pos;

			cur.Next = node;
			cur = node;
		}
		cur.Next = this.NodeEnd;

		var pos2 = this.NodeEnd.transform.localPosition;
		pos2.x = this.Size * this.Distance / 2;
		this.NodeEnd.transform.localPosition = pos2;
	}
	#endregion
}
