using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class TrafficAIModel : LKModel<TrafficAIModel> 
{
	#region statics
	// Types
	public const int TypeCar = 1;
	public const int TypePeople = 2;

	// For id
	public const int IdBasic = 100;
	#endregion

	public bool IsEdit = false;

	TrafficAIVoNode LatestNode(int type)
	{
		var list = this.NodeList(type);
		if (list.Count <= 0)
			return null;

		return list[list.Count - 1];
	}

	public int LatestId(int type)
	{
		if (this.LatestNode(type) == null)
			return 0;
		else
			return (this.LatestNode(type).Id / IdBasic) * IdBasic;
	}

	public int EdgeId(int start, int end)
	{
		return int.Parse(start.ToString() + end.ToString());
	}

	protected override void Init()
	{
		// Node
		this.nodeDictDict[TypeCar] = new Dictionary<int, TrafficAIVoNode>();
		this.nodeDictDict[TypePeople] = new Dictionary<int, TrafficAIVoNode>();
		this.nodeListDict[TypeCar] = new List<TrafficAIVoNode>();
		this.nodeListDict[TypePeople] = new List<TrafficAIVoNode>();

		// Edge
		this.edgeDictDict[TypeCar] = new Dictionary<int, TrafficAIVoEdge>();
		this.edgeDictDict[TypePeople] = new Dictionary<int, TrafficAIVoEdge>();

		this.InitNodes();
		this.InitEdges();
		this.InitGraph();

		if (!this.IsEdit)
		{
			this.InitRoles();
			this.InitRoleSpawns();
		}
	}

	// For edit
	bool initEditAI = false;
	public void InitEditAI()
	{
		if (!this.IsEdit || this.initEditAI)
			return;
		this.initEditAI = true;

		this.InitRoles();
		this.InitRoleSpawns();
	}

	// For edit
	public void InitEdit()
	{
		this.IsEdit = true;

		// Init
		this.Init();

		// Init nodes
		this.InitEditNodes();

		// Init edges
		this.InitEditEdges();
	}

	// For edit
	public void Clear()
	{
		this.nodeDictDict.Clear();
		this.nodeListDict.Clear();
		this.edgeDictDict.Clear();
		this.roleDict.Clear();
		this.roleSpawnDict.Clear();
	}

	#region nodes
	// Nodes
	// { type, { id, node } }
	Dictionary<int, Dictionary<int, TrafficAIVoNode>> nodeDictDict = 
		new Dictionary<int, Dictionary<int, TrafficAIVoNode>>();
	public Dictionary<int, TrafficAIVoNode> NodeDict(int type)
	{
		return this.nodeDictDict[type];
	}
	
	// Already sorted by id
	// { type, nodes }
	Dictionary<int, List<TrafficAIVoNode>> nodeListDict =
		new Dictionary<int, List<TrafficAIVoNode>>();	
	public List<TrafficAIVoNode> NodeList(int type)
	{
		return this.nodeListDict[type];
	}
	
	public List<TrafficAIVoNode> Nodes
	{
		get
		{
			var result = new List<TrafficAIVoNode>();
			foreach (var nodes in this.nodeDictDict)
			{
				foreach (var node in nodes.Value)
				{
					result.Add(node.Value);
				}
			}
			return result;
		}
	}

	public void InitNodes()
	{
		var configs = ConfigManager.Instance.GetElementList<Sg_Traffic_Node>();
		for (int i = 0, max = configs.Count; i < max; i++)
		{
			var config = configs[i];
			this.InitNode(config);
		}

		this.nodeListDict[TypeCar].Sort((e1, e2) => e1.Id - e2.Id);
		this.nodeListDict[TypePeople].Sort((e1, e2) => e1.Id - e2.Id);
	}

	void InitNode(Sg_Traffic_Node config)
	{
		var type = config.type;
		var node = new TrafficAIVoNode(config);

		this.nodeListDict[type].Add(node);
		this.nodeDictDict[type].Add(node.Id, node);
	}

	// For edit
	public TrafficAIVoNode GetNode(int type, int id)
	{
		if (!this.nodeDictDict[type].ContainsKey(id))
			return null;
		return this.nodeDictDict[type][id];
	}

	// For edit
	public void InitEditNodes()
	{
	}

	// For edit
	public TrafficAIVoNode AddNode(int type, int? id = null)
	{
		var node = new TrafficAIVoNode();
		node.Type = type;

		// Init id
		if (id == null)
			node.Id = this.LatestId(type) + IdBasic;
		else
			node.Id = (int)id;

		if (this.nodeDictDict[type].ContainsKey(node.Id))
			return this.nodeDictDict[type][node.Id];

		// Add
		this.nodeListDict[type].Add(node);
		this.nodeDictDict[type].Add(node.Id, node);

		this.nodeListDict[type].Sort((e1, e2) => e1.Id - e2.Id);

		return node;
	}

	// For edit
	public void RemoveNode(TrafficAIVoNode node)
	{
		var type = node.Type;

		this.nodeListDict[type].Remove(node);
		this.nodeDictDict[type].Remove(node.Id);

		Debug.Log("TrafficModel::RemoveNode " + node.Id);
	}
	#endregion

	#region edges
	// For edit
	// { type, { id, edge } }
	Dictionary<int, Dictionary<int, TrafficAIVoEdge>> edgeDictDict =
		new Dictionary<int, Dictionary<int, TrafficAIVoEdge>>();	
	public List<TrafficAIVoEdge> EdgeList(int type)
	{
		return this.edgeDictDict[type].Values.ToList();
	}
	
	public List<TrafficAIVoEdge> Edges
	{
		get
		{
			var result = new List<TrafficAIVoEdge>();
			foreach (var edges in this.edgeDictDict)
			{
				foreach (var edge in edges.Value)
				{
					result.Add(edge.Value);
				}
			}
			return result;
		}
	}

	public void InitEdges()
	{
		var configs = ConfigManager.Instance.GetElementList<Sg_Traffic_Edge>();
		for (int i = 0, max = configs.Count; i < max; i++)
		{
			var config = configs[i];
			this.InitEdge(config);
		}
	}

	void InitEdge(Sg_Traffic_Edge config)
	{
		var type = config.type;
		var edge = new TrafficAIVoEdge(config);

		this.edgeDictDict[type].Add(edge.Id, edge);
	}

	// For edit
	public void InitEditEdges()
	{
	}

	// For edit
	public TrafficAIVoEdge AddEdge(int type)
	{
		var start = this.AddNode(type);
		start.IsEdge = true;
		var end = this.AddNode(type);
		end.IsEdge = true;

		var edge = new TrafficAIVoEdge();
		edge.Id = this.EdgeId(start.Id, end.Id);
		edge.Type = type;
		edge.Start = start;
		edge.End = end;

		// Add
		this.edgeDictDict[type].Add(edge.Id, edge);

		return edge;
	}

	// For edit
	public void AddEdge(TrafficAIVoNode start, TrafficAIVoNode end)
	{
		var type = start.Type;
		var edge = new TrafficAIVoEdge();
		edge.Type = type;
		edge.Start = start;
		edge.End = end;
		edge.Id = this.EdgeId(edge.StartId, edge.EndId);

		if (this.edgeDictDict[type].ContainsKey(edge.Id))
			return;

		// Add
		this.edgeDictDict[type].Add(edge.Id, edge);
	}

	// For edit
	public void RemoveEdge(TrafficAIVoEdge edge)
	{
		var type = edge.Type;
		var id = this.EdgeId(edge.StartId, edge.EndId);

		// Remove
		this.edgeDictDict[type].Remove(id);
		
		Debug.Log("TrafficModel::RemoveEdge " + id);
	}

	// For edit
	public void RemoveEdge(TrafficAIVoNode start, TrafficAIVoNode end)
	{
		var type = start.Type;
		var id = this.EdgeId(start.Id, end.Id);

		// Remove
		this.edgeDictDict[type].Remove(id);

		Debug.Log("TrafficModel::RemoveEdge " + id);
	}
	#endregion

	#region graph
	Dictionary<int, Digraph> graphDict = new Dictionary<int, Digraph>();

	public Digraph Graph(int type)
	{
		return this.graphDict[type];
	}
	
	void InitGraph()
	{
		this.InitGraph(TypeCar);
		this.InitGraph(TypePeople);
	}

	public void TestPath(int type, int start, int end)
	{
		this.InitGraph();

		var path = this.GetPath(type, start, end);
		var s = "path: ";
		path.ForEach(e => s += e.Id + "->"); 
		Debug.Log(s);
	}
	
	void InitGraph(int type)
	{
		var graph = this.graphDict[type] = new Digraph();	

		// Nodes
		var nodes = this.NodeList(type);
		for (int i = 0, max = nodes.Count; i < max; i++)
		{
			var node = nodes[i];
			graph.AddNode(node);
		}

		// Edges
		var edges = this.EdgeList(type);
		for (int i = 0, max = edges.Count; i < max; i++)
		{
			var edge = edges[i];
			if (edge.IsEdit)
			{
				var last = edge.StartId + edge.Size - 1;
				var end = edge.EndId;

				var start = edge.StartId;
				var next = start;

				while (start <= last)
				{
					next = start + 1;
					if (next > last)
						next = end;

					var startNode = this.GetNode(type, start);
					var endNode = this.GetNode(type, next);
					if (startNode == null)
					{
						Debug.LogError("TrafficAIModel::InitGraph error start node not found id: " + start);
						this.edgeDictDict[type].Remove(edge.Id);
						break;
					}
					if (endNode == null)
					{
						Debug.LogError("TrafficAIModel::InitGraph error end node not found id: " + next);
						this.edgeDictDict[type].Remove(edge.Id);
						break;
					}

					graph.AddEdge(startNode, endNode, edge.Distance);
					start ++;
				}
			}
			else
			{
				var startNode = edge.Start;
				var endNode = edge.End;
				if (startNode == null)
				{
					Debug.LogError("TrafficAIModel::InitGraph error start node not found id:" + edge.StartId);
					this.edgeDictDict[type].Remove(edge.Id);
					continue;
				}
				if (endNode == null)
				{
					Debug.LogError("TrafficAIModel::InitGraph error end node not found id: " + edge.EndId);
					this.edgeDictDict[type].Remove(edge.Id);
					continue;
				}
				
				graph.AddEdge(edge.Start, edge.End, edge.Dis);
			}
		}
	}

	/// <summary>
	/// Gets the shortest path.
	/// </summary>
	public List<TrafficAIVoNode> GetPath(int type, int startId, int endId)
	{
		var path = new List<TrafficAIVoNode>();
		var start = this.GetNode(type, startId);
		var end = this.GetNode(type, endId);
		if (start == null)
		{
			Debug.LogError("TrafficAIModel::GetPath error start node is null id: " + startId);
			return path;
		}
		if (end == null)
		{
			Debug.LogError("TrafficAIModel::GetPath error end node is null id: " + endId);
			return path;
		}

		var graph = this.Graph(type);

		graph.Dijkstra(start, end, out path);

		return path;
	}
	#endregion

	#region role
	Dictionary<int, TrafficAIVoRole> roleDict = new Dictionary<int, TrafficAIVoRole>();

	void InitRoles()
	{
		var configs = ConfigManager.Instance.GetElementList<Sg_Traffic_Role>();
		for (int i = 0, max = configs.Count; i < max; i++)
		{
			var config = configs[i];
			this.InitRole(config);
		}
	}

	void InitRole(Sg_Traffic_Role config)
	{
		var data = new TrafficAIVoRole(config);
		this.roleDict.Add(data.Id, data);
	}

	public List<TrafficAIVoRole> GetRoles()
	{
		return this.roleDict.Values.ToList();
	}

	public TrafficAIVoRole GetRole(int id)
	{
		if (!this.roleDict.ContainsKey(id))
			return null;
		return this.roleDict[id];
	}
	#endregion

	#region role spawn
	Dictionary<int, TrafficAIVoRoleSpawn> roleSpawnDict = new Dictionary<int, TrafficAIVoRoleSpawn>();

	void InitRoleSpawns()
	{
		var configs = ConfigManager.Instance.GetElementList<Sg_Traffic_Role_Spawn>();
		for (int i = 0, max = configs.Count; i < max; i++)
		{
			var config = configs[i];
			this.InitRoleSpawn(config);
		}
	}

	void InitRoleSpawn(Sg_Traffic_Role_Spawn config)
	{
		var data = new TrafficAIVoRoleSpawn(config);
		this.roleSpawnDict.Add(data.Id, data);
	}

	public List<TrafficAIVoRoleSpawn> GetRoleSpawns()
	{
		return this.roleSpawnDict.Values.ToList();
	}
	#endregion
}
