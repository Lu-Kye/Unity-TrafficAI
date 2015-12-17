using UnityEngine;
using System.Collections;

public class TrafficAIView : LK3DView<TrafficAIView, TrafficAIController> 
{
	TrafficAITree tree = new TrafficAITree();

	protected override void Init()
	{
		this.controller = TrafficAIController.Instance;
		this.controller.InitView(this);

		this.tree.Run();
	}

	public void ReInitTree()
	{
		this.tree = new TrafficAITree();
		this.tree.Run();
	}

	protected override void Close()
	{
	}

	void Update()
	{
		this.tree.Update();
	}
}
