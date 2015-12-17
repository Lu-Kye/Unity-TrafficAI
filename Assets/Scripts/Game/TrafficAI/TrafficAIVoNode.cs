using UnityEngine;
using System.Collections;


public class TrafficAIVoNode : IDigraphData
{
	public TrafficAIEditNode Edit;

	// Config
	Sg_Traffic_Node sg_traffic_node = new Sg_Traffic_Node();

	public TrafficAIVoNode()
	{
	}

	public TrafficAIVoNode(Sg_Traffic_Node sg_traffic_node)
	{
		this.sg_traffic_node = sg_traffic_node;
	}

	public int Id
	{
		get { return this.sg_traffic_node.id; }
		set 
		{
			if (!TrafficAIModel.Instance.IsEdit)
				Debug.LogError("TrafficAIVoNode::Id cant set id in not edit mode");

			this.sg_traffic_node.id = value; 
		}
	}

	public int Type
	{
		get { return this.sg_traffic_node.type; }
		set
		{
			if (!TrafficAIModel.Instance.IsEdit)
				Debug.LogError("TrafficAIVoNode::Type cant set type in not edit mode");
			
			this.sg_traffic_node.type = value; 
		}
	}

	public Vector3 Pos
	{
		get { return UtilString.ToVector(this.sg_traffic_node.pos); }
	}

	public bool IsEdge
	{
		get { return this.sg_traffic_node.isedge == 1; }
		set
		{
			if (!TrafficAIModel.Instance.IsEdit)
				Debug.LogError("TrafficAIVoNode::IsEdge cant set isedge in not edit mode");

			this.sg_traffic_node.isedge = value ? 1 : 0;
		}
	}

	public object JsonObj
	{
		get
		{
			if (!TrafficAIModel.Instance.IsEdit)
				Debug.LogError("TrafficAIVoNode::JsonObj cant get json object in not edit mode");

			// Update data
			this.sg_traffic_node.pos = UtilVector3.ToString(this.Edit.transform.position);

			return this.sg_traffic_node;
		}
	}

	#region for exporter
	public string Json
	{
		get
		{
			if (!TrafficAIModel.Instance.IsEdit)
				Debug.LogError("TrafficAIVoNode::Json cant get json in not edit mode");

			return JsonUtility.ToJson(this.JsonObj);
		}
	}
	#endregion

	#region for role runtime actions
	bool isArrivable = true;
	public bool IsArrivable
	{
		get { return this.isArrivable; }
		set { this.isArrivable = value; }
	}
	#endregion
}
