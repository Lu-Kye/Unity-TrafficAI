using UnityEngine;
using System.Collections;

public class TrafficAICarTree : TrafficAIRoleTree 
{
	public TrafficAICarTree(TrafficAIRole role, TrafficAIVoRole data)
		: base(role, data)
	{
	}

	protected override void Init()
	{
		// First add role tree
		base.Init();
	}
}
