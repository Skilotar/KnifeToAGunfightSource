using System;
using System.Collections.Generic;
using UnityEngine;

namespace GungeonAPI
{
	// Token: 0x0200002E RID: 46
	public class ShrineFakePrefab : Component
	{
		// Token: 0x0600018C RID: 396 RVA: 0x0000EE10 File Offset: 0x0000D010
		public static bool IsFakePrefab(UnityEngine.Object o)
		{
			bool flag = o is GameObject;
			bool flag2 = flag;
			bool result;
			if (flag2)
			{
				result = ShrineFakePrefab.ExistingFakePrefabs.Contains((GameObject)o);
			}
			else
			{
				bool flag3 = o is Component;
				result = (flag3 && ShrineFakePrefab.ExistingFakePrefabs.Contains(((Component)o).gameObject));
			}
			return result;
		}

		// Token: 0x0600018D RID: 397 RVA: 0x0000EE71 File Offset: 0x0000D071
		public static void MarkAsFakePrefab(GameObject obj)
		{
			ShrineFakePrefab.ExistingFakePrefabs.Add(obj);
		}

		// Token: 0x0600018E RID: 398 RVA: 0x0000EE80 File Offset: 0x0000D080
		public static GameObject Clone(GameObject obj)
		{
			bool flag = ShrineFakePrefab.IsFakePrefab(obj);
			bool activeSelf = obj.activeSelf;
			bool flag2 = activeSelf;
			bool flag3 = flag2;
			if (flag3)
			{
				obj.SetActive(false);
			}
			GameObject gameObject = UnityEngine.Object.Instantiate<GameObject>(obj);
			bool flag4 = activeSelf;
			bool flag5 = flag4;
			if (flag5)
			{
				obj.SetActive(true);
			}
			foreach (object obj2 in gameObject.GetComponentInChildren<Transform>())
			{
				UnityEngine.Object.DontDestroyOnLoad(gameObject);
			}
			ShrineFakePrefab.ExistingFakePrefabs.Add(gameObject);
			return gameObject;
		}

		// Token: 0x0600018F RID: 399 RVA: 0x0000EF34 File Offset: 0x0000D134
		public static UnityEngine.Object Instantiate(UnityEngine.Object o, UnityEngine.Object new_o)
		{
			bool flag = o is GameObject && ShrineFakePrefab.ExistingFakePrefabs.Contains((GameObject)o);
			bool flag2 = flag;
			if (flag2)
			{
				((GameObject)new_o).SetActive(true);
			}
			else
			{
				bool flag3 = o is Component && ShrineFakePrefab.ExistingFakePrefabs.Contains(((Component)o).gameObject);
				bool flag4 = flag3;
				if (flag4)
				{
					((Component)new_o).gameObject.SetActive(true);
				}
			}
			return new_o;
		}

		// Token: 0x040000D8 RID: 216
		internal static HashSet<GameObject> ExistingFakePrefabs = new HashSet<GameObject>();
	}
}
