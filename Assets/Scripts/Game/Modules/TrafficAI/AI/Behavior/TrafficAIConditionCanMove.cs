using UnityEngine;
using System.Collections;
using LKBehaviorTree;

public class TrafficAIConditionCanMove : Condition 
{
	TrafficAIRole role;

	public TrafficAIConditionCanMove(TrafficAIRole role)
	{
		this.role = role;
	}

	protected override bool DoCondition()
	{
		return true;
	}
}
