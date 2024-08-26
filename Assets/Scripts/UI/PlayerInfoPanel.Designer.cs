using System;
using UnityEngine;
using UnityEngine.UI;
using QFramework;

namespace Battlegrounds
{
	// Generate Id:ae0cded2-acf2-4c4b-8aa0-b474bf296201
	public partial class PlayerInfoPanel
	{
		public const string Name = "PlayerInfoPanel";
		
		[SerializeField]
		public ManaSlot ManaSlot;
		[SerializeField]
		public UnityEngine.UI.Text manaTxt;
		[SerializeField]
		public MinionSlot MinionSlot;
		[SerializeField]
		public UnityEngine.UI.Button turnBtn;
		[SerializeField]
		public UnityEngine.UI.Image ac;
		[SerializeField]
		public UnityEngine.UI.Text hp;
		[SerializeField]
		public HandCardSlot HandCardSlot;
		[SerializeField]
		public RectTransform cardStartPosition;
		
		private PlayerInfoPanelData mPrivateData = null;
		
		protected override void ClearUIComponents()
		{
			ManaSlot = null;
			manaTxt = null;
			MinionSlot = null;
			turnBtn = null;
			ac = null;
			hp = null;
			HandCardSlot = null;
			cardStartPosition = null;
			
			mData = null;
		}
		
		public PlayerInfoPanelData Data
		{
			get
			{
				return mData;
			}
		}
		
		PlayerInfoPanelData mData
		{
			get
			{
				return mPrivateData ?? (mPrivateData = new PlayerInfoPanelData());
			}
			set
			{
				mUIData = value;
				mPrivateData = value;
			}
		}
	}
}
