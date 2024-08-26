/****************************************************************************
 * 2024.8 DESKTOP-D9T87KM
 ****************************************************************************/

using UnityEngine;
using UnityEngine.UI;
using QFramework;

namespace Battlegrounds
{
	public partial class HandCardSlot
	{
		[SerializeField] public CardUIItem CardUIItem;

		public void Clear()
		{
			CardUIItem = null;
		}

		public override string ComponentName
		{
			get { return "HandCardSlot";}
		}
	}
}
