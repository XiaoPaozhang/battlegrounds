/****************************************************************************
 * 2024.8 DESKTOP-D9T87KM
 ****************************************************************************/

using UnityEngine;
using UnityEngine.UI;
using QFramework;

namespace Battlegrounds
{
	public partial class MinionSlot
	{
		[SerializeField] public MinionUIItem MinionUIItem;

		public void Clear()
		{
			MinionUIItem = null;
		}

		public override string ComponentName
		{
			get { return "MinionSlot";}
		}
	}
}
