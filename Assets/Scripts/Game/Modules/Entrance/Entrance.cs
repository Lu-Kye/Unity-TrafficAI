using UnityEngine;
using System.Collections;

public class Entrance : MonoBehaviour 
{
	void Awake() 
	{
		ConfigManager.Instance.Init();
	}

	public void ShowPath()
	{
		TrafficAIModel.Instance.InitEdit();

		var prefab = ResourceManager.Instance.Load<GameObject>(
			ResourceConfig.PREFAB_TRAFFICAIEDIT);
		if (UtilGameObject.Find(ResourceConfig.PREFAB_TRAFFICAIEDIT.GameObjectName, true) == null)
			UtilGameObject.CreateByGO(prefab);
	}
}
