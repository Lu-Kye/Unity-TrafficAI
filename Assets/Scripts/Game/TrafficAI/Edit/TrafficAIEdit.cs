using UnityEngine;
using System.Collections;


public class TrafficAIEdit : TrafficAIEditBase 
{
	public static TrafficAIEdit Instance;

	#region gameobject
	public GameObject Arrows;
	public LineRenderer Arrow;
	public TrafficAIEditForCar ForCar;
	public TrafficAIEditForPeople ForPeople;
	#endregion

	protected override void Init()
	{
		Instance = this;

		this.InitNodes();
		this.InitEdges();
	}

	protected override void OnDestroy()
	{
		Instance = null;
	}

	public LineRenderer AddArrow()
	{
		var prefab = this.Arrow.gameObject;
		var parent = this.Arrows;
		var go = UtilGameObject.CreateByGO(prefab, parent);
		var script = go.GetComponent<LineRenderer>();
		return script;
	}

	public void Clear()
	{
		this.ClearForCar();
		this.ClearForPeople();
	}

	public void ClearForCar()
	{
		UtilGameObject.DeleteChilds(this.ForCar.gameObject, true);
	}

	public void ClearForPeople()
	{
		UtilGameObject.DeleteChilds(this.ForPeople.gameObject, true);
	}

	#region nodes
	void InitNodes()
	{
		this.InitNodes(TrafficAIModel.TypeCar);
		this.InitNodes(TrafficAIModel.TypePeople);
	}

	void InitNodes(int type)
	{
		var nodes = TrafficAIModel.Instance.NodeList(type);
		foreach (var node in nodes)
		{
			if (!node.IsEdge)
			{
				switch (type)
				{
				case TrafficAIModel.TypeCar:
					this.ForCar.AddNode(node);
					break;
				
				case TrafficAIModel.TypePeople:
					this.ForPeople.AddNode(node);
					break;
				}
			}
		}
	}
	#endregion

	#region edges
	void InitEdges()
	{
		// Car
		this.InitEdges(TrafficAIModel.TypeCar);
		this.InitEdges(TrafficAIModel.TypePeople);
	}

	void InitEdges(int type)
	{
		var edges = TrafficAIModel.Instance.EdgeList(type);

		// First init edit edges
		foreach (var edge in edges)
		{
			if (edge.IsEdit)
			{
				switch (type)
				{
				case TrafficAIModel.TypeCar:
					this.ForCar.AddEdge(edge);
					break;
					
				case TrafficAIModel.TypePeople:
					this.ForPeople.AddEdge(edge);
					break;
				}
			}
		}

		// Next init not edit edges
		foreach (var edge in edges)
		{
			if (!edge.IsEdit)
			{
				switch (type)
				{
				case TrafficAIModel.TypeCar:
					this.ForCar.AddUneditEdge(edge);
					break;
					
				case TrafficAIModel.TypePeople:
					this.ForPeople.AddUneditEdge(edge);
					break;
				}
			}
		}
	}
	#endregion
}
