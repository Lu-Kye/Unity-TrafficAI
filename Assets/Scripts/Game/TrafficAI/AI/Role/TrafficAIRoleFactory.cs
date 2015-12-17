using UnityEngine;

public static class TrafficAIRoleFactory
{
	// Create role
	public static TrafficAIRole Create(TrafficAIVoRoleSpawn data)
	{
		if (!data.Start.IsArrivable)
			return null;

		var prefab = data.Role.Icon3DPrefab;
		var parent = TrafficAIController.Instance.View.gameObject;
		var go = UtilGameObject.CreateByGO(prefab, parent);
		go.transform.position = data.Start.Pos;

		// script
		var script = go.AddComponent<TrafficAIRole>();
		script.Data = data.Role;
		script.StartNode = data.Start;
		script.EndNode = data.End;
		script.Tree = TrafficAIRoleTreeFactory.Create(script, data.Role);
		return script;
	}
}
