using UnityEngine;
using System.Collections;


public class TrafficAIVoEdge 
{
	public TrafficAIEditEdge Edit;

	// Config
	Vo_Traffic_Edge vo_traffic_edge = new Vo_Traffic_Edge();
	
	public TrafficAIVoEdge()
	{
	}

	public TrafficAIVoEdge(Vo_Traffic_Edge vo_traffic_edge)
	{
		this.vo_traffic_edge = vo_traffic_edge;
	}

	public int Id
	{
		get { return this.vo_traffic_edge.id; }
		set 
		{
			if (!TrafficAIModel.Instance.IsEdit)
				Debug.LogError("TrafficAIVoEdge::Id cant set id in not edit mode");
			
			this.vo_traffic_edge.id = value; 
		}
	}

	public int Type
	{
		get { return this.vo_traffic_edge.type; }
		set
		{
			if (!TrafficAIModel.Instance.IsEdit)
				Debug.LogError("TrafficAIVoEdge::Type cant set type in not edit mode");
			
			this.vo_traffic_edge.type = value; 
		}
	}

	public int StartId
	{
		get { return this.vo_traffic_edge.start; }
		set 
		{
			if (!TrafficAIModel.Instance.IsEdit)
				Debug.LogError("TrafficAIVoEdge::StartId cant set start id in not edit mode");
			
			this.vo_traffic_edge.start = value; 
		}
	}

	TrafficAIVoNode start;
	public TrafficAIVoNode Start
	{
		get 
		{ 
			// Try get
			if (this.start == null)
				return this.start = TrafficAIModel.Instance.GetNode(this.Type, this.StartId);

			return this.start; 
		}
		set 
		{
			if (!TrafficAIModel.Instance.IsEdit)
				Debug.LogError("TrafficAIVoEdge::Start cant set start in not edit mode");
			
			this.start = value; 
			this.StartId = value.Id;
		}
	}

	public int EndId
	{
		get { return this.vo_traffic_edge.end; }
		set 
		{
			if (!TrafficAIModel.Instance.IsEdit)
				Debug.LogError("TrafficAIVoEdge::EndId cant set end id in not edit mode");
			
			this.vo_traffic_edge.end = value; 
		}
	}

	TrafficAIVoNode end;
	public TrafficAIVoNode End
	{
		get 
		{
			// Try get
			if (this.end == null)
				return this.end = TrafficAIModel.Instance.GetNode(this.Type, this.EndId);

			return this.end; 
		}
		set 
		{
			if (!TrafficAIModel.Instance.IsEdit)
				Debug.LogError("TrafficAIVoEdge::End cant set end in not edit mode");
			
			this.end = value; 
			this.EndId = value.Id;
		}
	}

	public float Dis
	{
		get 
		{ 
			float dis;
			if (!float.TryParse(this.vo_traffic_edge.dis, out dis))
				Debug.LogError("TrafficAIVoEdge::Dis parse error id: " + this.Id + " dis:" + dis);
			return dis;
		}
	}
	
	public bool IsEdit
	{
		get { return this.vo_traffic_edge.isedit == 1; }
	}

	public Vector3 Pos
	{
		get { return UtilString.ToVector(this.vo_traffic_edge.pos); }
	}

	public Quaternion Rotation
	{
		get 
		{
			var euler = UtilString.ToVector(this.vo_traffic_edge.rotation);
			var rotation = Quaternion.Euler(euler);
			return rotation;
		}
	}

	public float Distance
	{
		get 
		{
			return float.Parse(this.vo_traffic_edge.distance); 
		}
	}

	public int Size
	{
		get { return this.vo_traffic_edge.size; }
	}
	
	public object JsonObj
	{
		get
		{
			if (!TrafficAIModel.Instance.IsEdit)
				Debug.LogError("TrafficAIVoEdge::JsonObj cant get json object in not edit mode");
			
			// Update data
			this.vo_traffic_edge.id = TrafficAIModel.Instance.EdgeId(this.StartId, this.EndId);
			this.vo_traffic_edge.start = this.StartId;
			this.vo_traffic_edge.end = this.EndId;
			this.vo_traffic_edge.dis = (this.Start.Edit == null || this.End.Edit == null) ? "" + Digraph.Infinity :
				Vector3.Distance(this.Start.Edit.transform.position, this.End.Edit.transform.position).ToString();
			this.vo_traffic_edge.isedit = this.Edit != null ? 1 : 0;
			if (this.vo_traffic_edge.isedit == 1)
			{
				this.vo_traffic_edge.pos = UtilVector3.ToString(this.Edit.transform.position);
				this.vo_traffic_edge.rotation = UtilVector3.ToString(this.Edit.transform.rotation.eulerAngles);
				this.vo_traffic_edge.distance = this.Edit.Distance.ToString();
				this.vo_traffic_edge.size = this.Edit.Size;
			}
			
			return this.vo_traffic_edge;
		}
	}
	
	#region for exporter
	public string Json
	{
		get
		{
			if (!TrafficAIModel.Instance.IsEdit)
				Debug.LogError("TrafficAIVoEdge::Json cant get json in not edit mode");

			return JsonUtility.ToJson(this.JsonObj);
		}
	}
	#endregion
}
