using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;

public static class TrafficAIPrefabEditor 
{
	const string ArtPath = "Assets/Arts/Models/Role/";
	const string PrefabPath = "Assets/Resources/Prefabs/TrafficAI/";

	public static void GenerateRolePrefabs()
	{
		ConfigManager.Instance.Init();

		var roles = TrafficAIModel.Instance.GetRoles();
		for (int i = 0, max = roles.Count; i < max; i++)
		{
			var role = roles[i];
			GeneratePrefab(role);
		}
	}

	static void GeneratePrefab(TrafficAIVoRole data)
	{
		if (data.Actions.Count <= 0)
			return;

		// Prefab name
		var name = data.ResourceId;

		// Animator Controller
		var animatorController = CreateAnimator(
			name, 
			ArtPath, 
			data.Actions
		);
		if (animatorController == null)
			return;

		// FBX
		var fbxPath = ArtPath + data.ResourceId + ".FBX";
		var fbxPrefab = AssetDatabase.LoadAssetAtPath<GameObject>(fbxPath);
		var fbx = GameObject.Instantiate(fbxPrefab);
		var animator = fbx.GetComponent<Animator>();
		if (animator == null)
			animator = fbx.GetComponent<Animator>();
		animator.runtimeAnimatorController = animatorController;

		// Go
		var go = new GameObject();
		fbx.name = "FBX";
		fbx.transform.parent = go.transform;

		// Prefab
		var path = PrefabPath + name + ".prefab";
		var prefab = PrefabUtility.CreatePrefab(path, go) as GameObject;
		PrefabUtility.prefabInstanceUpdated(prefab);

		// Refresh
		AssetDatabase.Refresh();

		// Destroy
		GameObject.DestroyImmediate(go);
	}

	#region animator
	static Motion GetMotion(string name, string folder)
	{
		var path = folder + name + ".anim";
		var animation = AssetDatabase.LoadAssetAtPath<Motion>(path);
		return animation;
	}

	static UnityEditor.Animations.AnimatorController CreateAnimator(
		string name, 
		string folder, 
		List<string> actions
	)
	{
		// Creates the controller
		var controllerName = name + "_Controller.controller";
		var controllerPath = folder + controllerName;
		var controller = UnityEditor.Animations.AnimatorController.CreateAnimatorControllerAtPath(
			controllerPath
		);

		// Add parameters && states && Transitions
		for (int i = 0, max = actions.Count; i < max; i++)
		{
			var action = actions[i];

			// Parameter 
			controller.AddParameter(action, AnimatorControllerParameterType.Trigger);

			// State
			var rootStateMachine = controller.layers[0].stateMachine;
			var state = rootStateMachine.AddState(action);
			state.motion = GetMotion(name + "_" + action, folder);
			if (state.motion == null)
			{
				AssetDatabase.DeleteAsset(controllerPath);
				return null;
			}

			// Transition
			var transition = rootStateMachine.AddAnyStateTransition(state);
			transition.AddCondition(UnityEditor.Animations.AnimatorConditionMode.If, 0, action);
			transition.duration = 0;
		}

		return controller;
	}
	#endregion
}
