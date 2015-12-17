using UnityEngine;
using System.Collections;


public class TrafficAIController : LK3DController<TrafficAIController, TrafficAIView> 
{
	protected override void InitNet()
	{
	}

	protected override void InitFirst()
	{
	}

	protected override void CloseNet()
	{
	}

	protected override void CloseEnd()
	{
	}

	public void ReInitAI()
	{
		this.view.ReInitTree();
	}
}
