using System;
using UnityEngine;
using UnityEngine.UI;
using QFramework;

namespace Battlegrounds
{
	// Generate Id:65681d98-6677-445f-9b60-a8048c4c2380
	public partial class EnemyInfoPanel
	{
		public const string Name = "EnemyInfoPanel";
		
		[SerializeField]
		public RectTransform MinionSlot;
		[SerializeField]
		public UnityEngine.UI.Image ac;
		[SerializeField]
		public UnityEngine.UI.Text hp;
		
		private EnemyInfoPanelData mPrivateData = null;
		
		protected override void ClearUIComponents()
		{
			MinionSlot = null;
			ac = null;
			hp = null;
			
			mData = null;
		}
		
		public EnemyInfoPanelData Data
		{
			get
			{
				return mData;
			}
		}
		
		EnemyInfoPanelData mData
		{
			get
			{
				return mPrivateData ?? (mPrivateData = new EnemyInfoPanelData());
			}
			set
			{
				mUIData = value;
				mPrivateData = value;
			}
		}
	}
}
