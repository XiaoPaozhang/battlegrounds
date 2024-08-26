using System;
using UnityEngine;
using UnityEngine.UI;
using QFramework;

namespace Battlegrounds
{
	// Generate Id:1ce9afe3-e713-4db2-a86d-9c57e255ad71
	public partial class DragPanel
	{
		public const string Name = "DragPanel";
		
		
		private DragPanelData mPrivateData = null;
		
		protected override void ClearUIComponents()
		{
			
			mData = null;
		}
		
		public DragPanelData Data
		{
			get
			{
				return mData;
			}
		}
		
		DragPanelData mData
		{
			get
			{
				return mPrivateData ?? (mPrivateData = new DragPanelData());
			}
			set
			{
				mUIData = value;
				mPrivateData = value;
			}
		}
	}
}
