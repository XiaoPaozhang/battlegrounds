using System;
using UnityEngine;
using UnityEngine.UI;
using QFramework;

namespace Battlegrounds
{
	// Generate Id:fe7fc1ba-6c6c-4930-b467-5a62f9e1a2c5
	public partial class ShopInfoPanel
	{
		public const string Name = "ShopInfoPanel";
		
		[SerializeField]
		public MinionSlot MinionSlot;
		[SerializeField]
		public UnityEngine.UI.Button upgradeBtn;
		[SerializeField]
		public UnityEngine.UI.Text upgradeCostTxt;
		[SerializeField]
		public UnityEngine.UI.Text starTxt;
		[SerializeField]
		public UnityEngine.RectTransform recoveryArea;
		[SerializeField]
		public UnityEngine.UI.Button refreshBtn;
		[SerializeField]
		public UnityEngine.UI.Text refreshCostTxt;
		[SerializeField]
		public UnityEngine.UI.Button lockBtn;
		
		private ShopInfoPanelData mPrivateData = null;
		
		protected override void ClearUIComponents()
		{
			MinionSlot = null;
			upgradeBtn = null;
			upgradeCostTxt = null;
			starTxt = null;
			recoveryArea = null;
			refreshBtn = null;
			refreshCostTxt = null;
			lockBtn = null;
			
			mData = null;
		}
		
		public ShopInfoPanelData Data
		{
			get
			{
				return mData;
			}
		}
		
		ShopInfoPanelData mData
		{
			get
			{
				return mPrivateData ?? (mPrivateData = new ShopInfoPanelData());
			}
			set
			{
				mUIData = value;
				mPrivateData = value;
			}
		}
	}
}
