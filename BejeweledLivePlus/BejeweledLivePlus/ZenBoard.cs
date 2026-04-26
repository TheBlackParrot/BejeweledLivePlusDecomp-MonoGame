using BejeweledLivePlus.Misc;
using SexyFramework.Graphics;
using SexyFramework.Misc;

namespace BejeweledLivePlus
{
	public class ZenBoard : Board
	{
		public ZenBoard()
		{
			mParams["Title"] = "Zen";
		}

		public override int GetLevelPoints()
		{
			return 4375 + ((mLevel + 1) * 625);
		}

		public override void UnloadContent()
		{
			BejeweledLivePlusApp.UnloadContent("GamePlay_UI_Normal");
			base.UnloadContent();
		}

		public override bool WantsLevelBasedBackground()
		{
			return true;
		}

		public override bool AllowSpeedBonus()
		{
			return false;
		}

		public override bool WantAnnihilatorReplacement()
		{
			return true;
		}

		public override string GetSavedGameName()
		{
			return "zen.sav";
		}

		public override bool SupportsReplays()
		{
			return false;
		}
		
		public override bool AllowNoMoreMoves()
		{
			return false;
		}

		public override float GetModePointMultiplier()
		{
			return 1f;
		}

		public override float GetRankPointMultiplier()
		{
			return 0.4f;
		}

		public override void LoadContent(bool threaded)
		{
			if (threaded)
			{
				BejeweledLivePlusApp.LoadContentInBackground("GamePlay_UI_Normal");
			}
			else
			{
				BejeweledLivePlusApp.LoadContent("GamePlay_UI_Normal");
			}
			base.LoadContent(threaded);
		}

		public override void Draw(Graphics g)
		{
			base.Draw(g);
			base.DrawGameElements(g);
		}
		
		public override void Update()
		{
			base.Update();
			GlobalMembers.gApp.mProfile.mStats[2] = mPoints;
		}
		
		public override void ShowCompleted()
		{
			base.ShowCompleted();
			GlobalMembers.gApp.mMenus[2].SetVisible(false);
		}
		
		public override void SetVisible(bool isVisible)
		{
			if (isVisible && !mVisible)
			{
				GameWebSocket.Send("started");	
			}
			base.SetVisible(isVisible);
		}
		
		public override void GameOver(bool visible)
		{
			Piece pieceAtRowCol = GetPieceAtRowCol((int)(mRand.Next() % 8), (int)(mRand.Next() % 8));
			if (pieceAtRowCol != null)
			{
				Hypercubeify(pieceAtRowCol);
			}
		}
		
		public override void SwapSucceeded(SwapData theSwapData)
		{
			base.SwapSucceeded(theSwapData);
			Point point = default;
			Piece[,] array = mBoard;
			foreach (Piece piece in array)
			{
				if (piece != null && piece.mMoveCreditId == theSwapData.mPiece1.mMoveCreditId)
				{
					point.mX += (int)piece.CX();
					point.mY += (int)piece.CY();
				}
			}
			
			GameState.PointsNeededToClear = (GetLevelPointsTotal(), GetLevelPoints());
		}

		public override void Pause()
		{
			GameWebSocket.Send("paused");	
			base.Pause();
		}
		
		public override void Unpause()
		{
			GameWebSocket.Send("resumed");	
			base.Unpause();
		}
		
		public override bool LoadGame(Serialiser theBuffer)
		{
			return LoadGame(theBuffer, true);
		}

		public override bool LoadGame(Serialiser theBuffer, bool resetReplay)
		{
			bool result = base.LoadGame(theBuffer, resetReplay);

			GameState.Level = mLevel + 1;
			GameState.LevelPercentComplete = GetLevelPct();
			GameState.Score = mPoints;
			// (how many we have, needed to clear)
			GameState.PointsNeededToClear = (GetLevelPointsTotal(), GetLevelPoints());
			
			return result;
		}
	}
}
