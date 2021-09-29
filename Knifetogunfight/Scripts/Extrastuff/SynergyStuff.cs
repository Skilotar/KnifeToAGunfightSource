using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Knives
{
	// Token: 0x020000AF RID: 175
	internal class SynergyFormInitialiser
	{
		public static void AddSynergyForms()
		{
			
			AdvancedTransformGunSynergyProcessor advancedTransformGunSynergyProcessor = (PickupObjectDatabase.GetById(MonkeyBarrel.mbID) as Gun).gameObject.AddComponent<AdvancedTransformGunSynergyProcessor>();
			advancedTransformGunSynergyProcessor.NonSynergyGunId = MonkeyBarrel.mbID;
			advancedTransformGunSynergyProcessor.SynergyGunId = MBSynergyForm.mbakID;
			advancedTransformGunSynergyProcessor.SynergyToCheck = "Apex Predator";

		}
	}
}