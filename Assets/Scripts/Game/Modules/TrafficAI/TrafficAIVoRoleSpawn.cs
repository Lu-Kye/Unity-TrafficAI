﻿using UnityEngine;
using System.Collections;

public class TrafficAIVoRoleSpawn
{
	Vo_Traffic_Role_Spawn vo_traffic_role_spawn;

	public TrafficAIVoRoleSpawn(Vo_Traffic_Role_Spawn vo_traffic_role_spawn)
	{
		this.vo_traffic_role_spawn = vo_traffic_role_spawn;
	}

	public int Id
	{
		get { return this.vo_traffic_role_spawn.id; }
	}

	public int Type
	{
		get { return this.vo_traffic_role_spawn.type; }
	}

	public TrafficAIVoRole Role
	{
		get { return TrafficAIModel.Instance.GetRole(this.vo_traffic_role_spawn.role_id); }
	}

	public TrafficAIVoNode Start
	{
		get { return TrafficAIModel.Instance.GetNode(this.Type, this.vo_traffic_role_spawn.start_node_id); }
	}

	public TrafficAIVoNode End
	{
		get { return TrafficAIModel.Instance.GetNode(this.Type, this.vo_traffic_role_spawn.end_node_id); }
	}

	public float Interval
	{
		get { return this.vo_traffic_role_spawn.interval / 1000f; }
	}
}
