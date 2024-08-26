/****************************************************************************
 * 2024.8 DESKTOP-D9T87KM
 ****************************************************************************/

using UnityEngine;
using UnityEngine.UI;
using QFramework;

namespace Battlegrounds
{
	public partial class MinionUIItem
	{
		[SerializeField] public UnityEngine.UI.Image bg;
		[SerializeField] public UnityEngine.UI.Text starTxt;
		[SerializeField] public UnityEngine.UI.Text nameTxt;
		[SerializeField] public UnityEngine.UI.Text atkTxt;
		[SerializeField] public UnityEngine.UI.Text hpTxt;

		public void Clear()
		{
			bg = null;
			starTxt = null;
			nameTxt = null;
			atkTxt = null;
			hpTxt = null;
		}

		public override string ComponentName
		{
			get { return "MinionUIItem";}
		}
	}
}
