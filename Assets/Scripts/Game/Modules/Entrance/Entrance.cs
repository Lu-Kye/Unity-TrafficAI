using UnityEngine;
using System.Collections;

public class Entrance : MonoBehaviour 
{
	void Awake() 
	{
		ConfigManager.Instance.Init();
	}

	bool isShowed = false;
	public void ShowPath()
	{
		if (this.isShowed)
			return;
		this.isShowed = true;

		TrafficAIModel.Instance.InitEdit();

		var prefab = ResourceManager.Instance.Load<GameObject>(
			ResourceConfig.PREFAB_TRAFFICAIEDIT);
		if (UtilGameObject.Find(ResourceConfig.PREFAB_TRAFFICAIEDIT.GameObjectName, true) == null)
			UtilGameObject.CreateByGO(prefab);
	}
}
