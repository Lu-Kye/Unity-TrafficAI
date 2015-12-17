using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
using System.Text;
using System.IO;

/// <summary>
/// Traffic AI exporter.
/// - Export edit data 
/// - Save data to json file
/// </summary>
public class TrafficAIExporter
{
	/// <summary>
	/// Export this instance.
	/// </summary>
	public static void Export()
	{
		// Nodes
		ExportNodes();

		// Edges
		ExportEdges();
	}

	static void ExportNodes()
	{
		var data = GetNodeData();
		var path = GetNodeFilePath();
		WriteData(path, data);
	}

	static void ExportEdges()
	{
		var data = GetEdgeData();
		var path = GetEdgeFilePath();
		WriteData(path, data);
	}

	#region node
	static string GetNodeData()
	{
		var sb = new StringBuilder();
		sb.Append('{');

		var nodes = TrafficAIModel.Instance.Nodes;
		foreach (var node in nodes)
		{
			if (node.Edit == null)
				continue;

			sb.Append('"');
			sb.Append(node.Id);
			// Add type
			sb.Append(node.Type);
			sb.Append('"');
			sb.Append(':');
			sb.Append(node.Json);
			if (nodes.IndexOf(node) != nodes.Count - 1)
				sb.Append(",");
		}

		sb.Append('}');
		return sb.ToString();
	}

	static string GetNodeFilePath()
	{
		var assetPath = Application.dataPath;
		var targetPath = Path.Combine(assetPath, "Resources");
		targetPath = Path.Combine(targetPath, ResourceConfig.JSON_VO_TRAFFIC_NODE.Path);
		return targetPath + ".json";
	}
	#endregion

	#region edge
	static string GetEdgeData()
	{
		var sb = new StringBuilder();
		sb.Append('{');
		
		// Car
		var edges = TrafficAIModel.Instance.Edges;
		foreach (var edge in edges)
		{
			sb.Append('"');
			sb.Append(edge.Id);
			// Add type
			sb.Append(edge.Type);
			sb.Append('"');
			sb.Append(':');
			sb.Append(edge.Json);
			if (edges.IndexOf(edge) != edges.Count - 1)
				sb.Append(",");
		}
		
		sb.Append('}');
		return sb.ToString();
	}
	
	static string GetEdgeFilePath()
	{
		var assetPath = Application.dataPath;
		var targetPath = Path.Combine(assetPath, "Resources");
		targetPath = Path.Combine(targetPath, ResourceConfig.JSON_VO_TRAFFIC_EDGE.Path);
		return targetPath + ".json";
	}
	#endregion

	/// <summary>
	/// Writes the json data into target json file
	/// </summary>
	/// <param name="data">Data.</param>
	static void WriteData(string path, string data)
	{
		StreamWriter sw = new StreamWriter(path);
		sw.Write(data);
		sw.Close();
		AssetDatabase.Refresh();
	}
}
