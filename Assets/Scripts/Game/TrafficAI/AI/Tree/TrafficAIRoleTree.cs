using UnityEngine;
using System.Collections;
using LKBehaviorTree;

public class TrafficAIRoleTree : BehaviorTree 
{
	public TrafficAIRole Role;
	public TrafficAIVoRole Data;

	public TrafficAIRoleTree()
	{
	}

	public TrafficAIRoleTree(TrafficAIRole role, TrafficAIVoRole data)
	{
		this.Role = role;
		this.Data = data;
	}

	protected override void Init()
	{
		// First Selector
		var selector = this.PushNode<CompositeSelector>();
		selector.AddCondition(
			new ConditionFunc(() => { return this.Role.TryDestroy(); })
		);
		selector.AddCondition(
			new ConditionFunc(() => { return this.Role.TryStop(); })
		);
		selector.AddCondition(
			this.CreateMoveTree()
		);
	}

	// Create move tree 
	protected virtual BehaviorTree CreateMoveTree()
	{
		var tree = new BehaviorTree();

		// Move
		tree.PushNode(new TaskFunc((delta) => { this.Role.TryMove(delta); }));

		// Selector after move
		var selectorAfterMove = tree.PushNode<CompositeSelector>();
		selectorAfterMove.AddCondition(
			new ConditionFunc(() => { return this.Role.TryMoveToNext(); })
		);

		return tree;
	}
}
