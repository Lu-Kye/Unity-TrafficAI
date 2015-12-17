using UnityEngine;
using UnityEditor;

public class TrafficAIEditForBaseEditor : Editor 
{
	string startId = "";
	string endId = "";

	public override void OnInspectorGUI() 
	{
		serializedObject.Update();

		if (GUILayout.Button("Add Edge"))
			this.Select(this.AddEdge());

		if (GUILayout.Button("Add Node"))
			this.Select(this.AddNode());

		GUILayout.BeginHorizontal();
		startId = GUILayout.TextField(startId, GUILayout.Width(100f));
		endId = GUILayout.TextField(endId, GUILayout.Width(100f));
		if (GUILayout.Button("Test Path", GUILayout.Width(100f)))
			(serializedObject.targetObject as TrafficAIEditForBase).TestPath(int.Parse(startId), int.Parse(endId));
		GUILayout.EndHorizontal();

		serializedObject.ApplyModifiedProperties();
	}

	void Select(GameObject go)
	{
		var gos = new Object[1];
		gos[0] = go;
		Selection.objects = gos;
	}

	GameObject AddEdge()
	{
		var script = serializedObject.targetObject as TrafficAIEditForBase;
		return script.AddEdge().gameObject;
	}

	GameObject AddNode()
	{
		var script = serializedObject.targetObject as TrafficAIEditForBase;
		return script.AddNode().gameObject;
	}
}
