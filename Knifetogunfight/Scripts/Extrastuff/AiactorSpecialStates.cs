using System;
using ItemAPI;
using UnityEngine;

namespace Knives
{
	public class AiactorSpecialStates : MonoBehaviour
	{

		public AiactorSpecialStates()
		{


			LootedByBaba = false;
			hitbyovercharger = false;
			RedTaped = false;
		}


	private void Start()
		{
			this.aIActor = base.GetComponent<AIActor>();
			
		}


		private void Update()
		{
			
		}

		public bool LootedByBaba = false;
		public bool hitbyovercharger = false;
		public bool RedTaped = false;
		
		private AIActor aIActor;
	}
}
