/****************************************************************************
 * 2024.8 DESKTOP-D9T87KM
 ****************************************************************************/

using UnityEngine;
using UnityEngine.UI;
using QFramework;

namespace Battlegrounds
{
	public partial class CardUIItem
	{
		[SerializeField] public UnityEngine.UI.Image bg;
		[SerializeField] public UnityEngine.UI.Image borderLine;
		[SerializeField] public UnityEngine.UI.Image star;
		[SerializeField] public UnityEngine.UI.Text starTxt;
		[SerializeField] public UnityEngine.UI.Image cardName;
		[SerializeField] public UnityEngine.UI.Text nameTxt;
		[SerializeField] public UnityEngine.UI.Image des;
		[SerializeField] public UnityEngine.UI.Text desTxt;
		[SerializeField] public UnityEngine.UI.Image attack;
		[SerializeField] public UnityEngine.UI.Text attackTxt;
		[SerializeField] public UnityEngine.UI.Image hp;
		[SerializeField] public UnityEngine.UI.Text hpTxt;
		[SerializeField] public UnityEngine.UI.Image CardBack;

		public void Clear()
		{
			bg = null;
			borderLine = null;
			star = null;
			starTxt = null;
			cardName = null;
			nameTxt = null;
			des = null;
			desTxt = null;
			attack = null;
			attackTxt = null;
			hp = null;
			hpTxt = null;
			CardBack = null;
		}

		public override string ComponentName
		{
			get { return "CardUIItem";}
		}
	}
}
