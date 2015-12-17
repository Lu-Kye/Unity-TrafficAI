using UnityEngine;
using System.Collections;
using Soul;

public class TrafficAIEditForCar : TrafficAIEditForBase
{
	protected override void Init()
	{
		this.type = TrafficAIModel.TypeCar;
	}
}
