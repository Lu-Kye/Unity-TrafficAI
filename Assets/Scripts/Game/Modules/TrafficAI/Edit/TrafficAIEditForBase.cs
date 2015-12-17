using UnityEngine;
using System.Collections;

public class TrafficAIEditForBase : TrafficAIEditBase 
{
	protected int type;

	#region prefab
	public GameObject Edge
	{
		get 
		{
			var parent = this.transform.parent;
			return UtilGameObject.FindInParent("Edge", parent.gameObject);
		}
	}
	
	public GameObject Node
	{
		get
		{
			var parent = this.transform.parent;
			return UtilGameObject.FindInParent("Node", parent.gameObject);
		}
	}
	#endregion


	public void TestPath(int start, int end)
	{
		TrafficAIModel.Instance.TestPath(this.type, start, end);
	}
	
	public virtual TrafficAIEditNode AddNode()
	{
		var prefab = this.Node;
		var go = UtilGameObject.CreateByGO(prefab, this.gameObject);
		
		var script = go.AddComponent<TrafficAIEditNode>();
		script.Data = TrafficAIModel.Instance.AddNode(this.type);
		go.AddComponent<TrafficAIEditNodeLink>();
		
		// Init
		var pos = go.transform.localPosition;
		pos.x = 0;
		pos.z = 0;
		go.transform.localPosition = pos;
		
		return script;
	}

	public TrafficAIEditNode AddNode(TrafficAIVoNode data)
	{
		var prefab = this.Node;
		var go = UtilGameObject.CreateByGO(prefab, this.gameObject);
		
		var script = go.AddComponent<TrafficAIEditNode>();
		go.AddComponent<TrafficAIEditNodeLink>();
		
		// Init
		script.Data = data;
		script.Data.Edit = script;
		script.ReInit();
		
		return script;
	}

	public virtual TrafficAIEditEdge AddEdge()
	{
		var prefab = this.Edge;
		var go = UtilGameObject.CreateByGO(prefab, this.gameObject);
		
		var script = go.AddComponent<TrafficAIEditEdge>();
		script.Data = TrafficAIModel.Instance.AddEdge(this.type);
		script.Size = 3;
		
		// Init
		var pos = go.transform.localPosition;
		pos.x = 0;
		pos.z = 0;
		go.transform.localPosition = pos;
		
		return script;
	}

	public TrafficAIEditEdge AddEdge(TrafficAIVoEdge data)
	{
		var prefab = this.Edge;
		var go = UtilGameObject.CreateByGO(prefab, this.gameObject);
		
		var script = go.AddComponent<TrafficAIEditEdge>();
		
		// Init
		script.Data = data;
		script.Data.Edit = script;
		script.ReInit();
		
		return script;
	}

	public void AddUneditEdge(TrafficAIVoEdge data)
	{
		var start = data.Start;
		var end = data.End;
		start.Edit.AddNext(end.Edit);
	}
}
