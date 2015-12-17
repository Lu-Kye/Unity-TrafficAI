using UnityEngine;
using System.Collections;

public class TrafficAIVoRole
{
	Vo_Traffic_Role vo_traffic_role;

	public TrafficAIVoRole(Vo_Traffic_Role vo_traffic_role)
	{
		this.vo_traffic_role = vo_traffic_role;
	}

	public int Id
	{
		get { return this.vo_traffic_role.id; }
	}

	public int Type
	{
		get { return this.vo_traffic_role.type; }
	}

	public string Icon3D
	{
		get { return "Prefabs/TrafficAI/" + this.vo_traffic_role.resource; }
	}

	public GameObject Icon3DPrefab
	{
		get { return ResourceManager.Instance.LoadAtPath<GameObject>(this.Icon3D); }
	}
}
