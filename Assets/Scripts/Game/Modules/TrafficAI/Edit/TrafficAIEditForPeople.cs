using UnityEngine;
using System.Collections;


public class TrafficAIEditForPeople : TrafficAIEditForBase 
{
	protected override void Init()
	{
		this.type = TrafficAIModel.TypePeople;
	}
}
