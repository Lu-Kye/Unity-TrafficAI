using UnityEngine;
using UnityEditor;
using System.Reflection;
using System;

public static class TrafficAIMenu 
{
	[MenuItem("Game/TrafficAI/Start")]
	static void Start()
	{
		EditorApplication.OpenScene("Assets/Scenes/City.unity");
		EditorApplication.ExecuteMenuItem("Edit/Play");
	}

	[MenuItem("Game/TrafficAI/StartEdit")]
	static void StartEdit()
	{
		if (!Application.isPlaying)
		{
			Debug.LogError("Cant start edit cause you even didnt start running!");
			return;
		}

		// Model init
		ConfManager.Instance.InitConfigData();
		TrafficAIModel.Instance.InitEdit();
		
		var prefab = ResourceManager.only.Load<GameObject>(
			ResourceConfig.PREFAB_TRAFFICAIEDIT);
		if (UtilGameObject.Find(ResourceConfig.PREFAB_TRAFFICAIEDIT.GameObjectName, true) == null)
			UtilGameObject.CreateByGO(prefab);
	}

	[MenuItem("Game/TrafficAI/SaveEdit")]
	static void SaveEdit()
	{
		if (UtilGameObject.Find(ResourceConfig.PREFAB_TRAFFICAIEDIT.GameObjectName, true) == null)
		{
			Debug.LogError("Cant save edit cause you even didnt start edit!");
			return;
		}

		TrafficAIExporter.Export();
	}

	[MenuItem("Game/TrafficAI/Reload")]
	static void Reload()
	{
		if (UtilGameObject.Find(ResourceConfig.PREFAB_TRAFFICAIEDIT.GameObjectName, true) == null)
		{
			Debug.LogError("Cant reload cause you even didnt start edit!");
			return;
		}

		var go = UtilGameObject.Find(ResourceConfig.PREFAB_TRAFFICAIEDIT.GameObjectName, true);
		GameObject.DestroyImmediate(go);

		// Restart
		StartEdit();
	}

	[MenuItem("Game/TrafficAI/Clear")]
	static void Clear()
	{
		if (UtilGameObject.Find(ResourceConfig.PREFAB_TRAFFICAIEDIT.GameObjectName, true) == null)
		{
			Debug.LogError("Cant clear cause you even didnt start edit!");
			return;
		}

		var go = UtilGameObject.Find(ResourceConfig.PREFAB_TRAFFICAIEDIT.GameObjectName, true);
		var edit = go.GetComponent<TrafficAIEdit>();
		edit.Clear();

		TrafficAIModel.Instance.Clear();
	}

	[MenuItem("Game/TrafficAI/StartAI(Beta)")]
	static void StartAI()
	{
		if (UtilGameObject.Find(ResourceConfig.PREFAB_TRAFFICAIEDIT.GameObjectName, true) == null)
		{
			Debug.LogError("Cant start ai cause you even didnt start edit!");
			return;
		}

		TrafficAIModel.Instance.InitEditAI();
		TrafficAIController.Instance.ReInitAI();
	}
}
