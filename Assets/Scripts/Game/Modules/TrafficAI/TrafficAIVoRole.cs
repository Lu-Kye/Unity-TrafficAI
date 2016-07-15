using UnityEngine;
using System.Collections;
using System.Collections.Generic;

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

	public string ResourceId
	{
		get { return this.vo_traffic_role.resource; }
	}

	#region actions
	// Common actions
	public const string Run = "Run";

	public const string CarBroken = "Broken";
	public static List<string> CarActions 
	{
		get 
		{
			var actions = new List<string>();
			actions.Add(Run);
			return actions;
		}
	}

	public const string PeopleHI = "HI";
	public static List<string> PeopleActions
	{
		get
		{
			var actions = new List<string>();
			return actions;
		}
	}

	public List<string> Actions
	{
		get
		{
			switch (this.Type)
			{
			case TrafficAIModel.TypeCar:
				return CarActions;

			case TrafficAIModel.TypePeople:
				return PeopleActions;
			}
			return null;
		}
	}
	#endregion
}
