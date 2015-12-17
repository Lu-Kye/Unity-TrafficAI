using UnityEngine;
using System.Collections;

public class TrafficAIVoRole
{
	Sg_Traffic_Role sg_traffic_role;

	public TrafficAIVoRole(Sg_Traffic_Role sg_traffic_role)
	{
		this.sg_traffic_role = sg_traffic_role;
	}

	public int Id
	{
		get { return this.sg_traffic_role.id; }
	}

	public int Type
	{
		get { return this.sg_traffic_role.type; }
	}

	public string Icon3D
	{
		get { return "Prefabs/TrafficAI/" + this.sg_traffic_role.resource; }
	}

	public GameObject Icon3DPrefab
	{
		get { return ResourceManager.only.LoadAtPath<GameObject>(this.Icon3D); }
	}
}
