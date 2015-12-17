using UnityEngine;
using System.Collections;
using System.Collections.Generic;


public class TrafficAIEditNodeLink : MonoBehaviour 
{
	public TrafficAIEditNode Node
	{
		get { return this.GetComponent<TrafficAIEditNode>(); }
	}

//	public List<int> Nexts
//	{
//		get
//		{
//			return null;
//		}
//	}

	void Start() 
	{
		// Must be added to node
		if (this.Node == null)
			GameObject.DestroyImmediate(this);
	}

	#region for editor
	public void AddNext(int id)
	{
//		if (id % TrafficAIModel.IdBasic != 0)
//		{
//			Debug.LogError("TrafficAIEditNodeLink::AddNext node id:" + id + " is not valid");
//			return;
//		}

		var data = TrafficAIModel.Instance.GetNode(this.Node.Data.Type, id);
		if (data == null)
		{
			Debug.LogError("TrafficAIEditNodeLink::AddNext node not found id:" + id);
			return;
		}
		if (data.Edit == this.Node)
		{
			Debug.LogError("TrafficAIEditNodeLink::AddNext cant add current node");
			return;
		}

		this.Node.AddNext(data.Edit);
	}

	public void DeleteNext(int id)
	{
		var data = TrafficAIModel.Instance.GetNode(this.Node.Data.Type, id);
		if (data == null)
		{
			Debug.LogError("TrafficAIEditNodeLink::DeleteNext node not found id:" + id);
			return;
		}
		if (data.Edit == this.Node)
		{
			Debug.LogError("TrafficAIEditNodeLink::DeleteNext cant delete current node");
			return;
		}

		this.Node.DeleteNext(data.Edit);
	}
	#endregion
}
