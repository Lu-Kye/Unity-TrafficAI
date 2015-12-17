using UnityEngine;
using System.Collections;
using LKBehaviorTree;

public class TrafficAITaskSpawn : TaskInterval 
{
	TrafficAIVoRoleSpawn data;

	public TrafficAITaskSpawn(TrafficAIVoRoleSpawn data)
	{
		this.data = data;
	}

	protected override void DoInit()
	{
		this.UpdateAtFirst = true;
		this.UpdateInterval = this.data.Interval;
	} 

	protected override void DoTask(float delta)
	{
		if (TrafficAIRoleFactory.Create(this.data) == null)
			Debug.Log("TrafficAITaskSpawn::DoTask error, start point is not arrivable id: " + this.data.Start.Id);
	}
}
