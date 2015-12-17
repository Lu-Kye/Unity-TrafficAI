using UnityEngine;
using System.Collections;
using LKBehaviorTree;

public class TrafficAITree : BehaviorTree 
{
	protected override void Init()
	{
		// Spawn parallel
		var spawnParallel = this.PushNode(new CompositeParallel()) as CompositeParallel;

		// Add spawn task to spawn roles
		var spawns = TrafficAIModel.Instance.GetRoleSpawns();
		for (int i = 0, max = spawns.Count; i < max; i++)
		{
			spawnParallel.AddChild(new TrafficAITaskSpawn(spawns[i]));
		}

		// End
//		this.PushNode(new TaskLog("TrafficAITree End"));
	}
}
