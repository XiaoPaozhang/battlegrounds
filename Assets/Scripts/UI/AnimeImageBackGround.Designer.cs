using System;
using UnityEngine;
using UnityEngine.UI;
using QFramework;

namespace Battlegrounds
{
	// Generate Id:4a4e38cf-858d-4002-b4b9-43cb6bd8915e
	public partial class AnimeImageBackGround
	{
		public const string Name = "AnimeImageBackGround";
		
		[SerializeField]
		public UnityEngine.UI.Image BackGround;
		
		private AnimeImageBackGroundData mPrivateData = null;
		
		protected override void ClearUIComponents()
		{
			BackGround = null;
			
			mData = null;
		}
		
		public AnimeImageBackGroundData Data
		{
			get
			{
				return mData;
			}
		}
		
		AnimeImageBackGroundData mData
		{
			get
			{
				return mPrivateData ?? (mPrivateData = new AnimeImageBackGroundData());
			}
			set
			{
				mUIData = value;
				mPrivateData = value;
			}
		}
	}
}
