using UnityEngine;
using System.Collections;

public class TrafficAIRoleTreeFactory 
{
	// Create role tree
	public static TrafficAIRoleTree Create(TrafficAIRole role, TrafficAIVoRole data)
	{
		switch (data.Type)
		{
		case TrafficAIModel.TypeCar:
			return new TrafficAICarTree(role, data);

		case TrafficAIModel.TypePeople:
			return new TrafficAIPeopleTree(role, data);
		}
		return null;
	}
}
