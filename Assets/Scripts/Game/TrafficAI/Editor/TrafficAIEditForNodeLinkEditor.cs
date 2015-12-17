using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(TrafficAIEditNodeLink))]
public class TrafficAIEditForNodeLinkEditor : Editor 
{
	public int Id;
	public string input = "";

	public override void OnInspectorGUI()
	{
		serializedObject.Update();

		var script = serializedObject.targetObject as TrafficAIEditNodeLink;
		var node = script.Node;

		GUILayout.BeginHorizontal();

		this.input = GUILayout.TextField(this.input, GUILayout.Width(60f));
		int.TryParse(this.input, out this.Id);

		if (GUILayout.Button("Add", GUILayout.Width(60f)))
			this.Add();

		if (GUILayout.Button("Delete", GUILayout.Width(60f)))
			this.Delete();

		GUILayout.EndHorizontal();

		serializedObject.ApplyModifiedProperties();
	}

	void Add()
	{
		var script = serializedObject.targetObject as TrafficAIEditNodeLink;
		script.AddNext(this.Id);
	}

	void Delete()
	{
		var script = serializedObject.targetObject as TrafficAIEditNodeLink;
		script.DeleteNext(this.Id);
	}
}
