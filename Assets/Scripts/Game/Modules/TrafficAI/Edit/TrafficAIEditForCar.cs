using UnityEngine;
using System.Collections;

public class TrafficAIEditForCar : TrafficAIEditForBase
{
	protected override void Init()
	{
		this.type = TrafficAIModel.TypeCar;
	}
}
