using UnityEngine;
using System.Collections;

public class TrafficAIPeopleTree : TrafficAIRoleTree 
{
	public TrafficAIPeopleTree(TrafficAIRole role, TrafficAIVoRole data)
		: base(role, data)
	{
	}

	protected override void Init()
	{
		// First add role tree
		base.Init();
	}
}
