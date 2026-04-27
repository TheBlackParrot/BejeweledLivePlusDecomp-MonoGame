using SexyFramework;
using SexyFramework.Graphics;

namespace BejeweledLivePlus.UI
{
	public class ButterflyEndLevelDialog : EndLevelDialog
	{
		public ButterflyBoard mButterflyBoard;

		public ButterflyEndLevelDialog(ButterflyBoard theBoard)
			: base(theBoard)
		{
			mButterflyBoard = theBoard;
			NudgeButtons((-40));
		}

		public override void Update()
		{
			base.Update();
		}

		public override void DrawStatsLabels(Graphics g)
		{
			string theString = GlobalMembers._ID("Butterflies Freed", 159);
			int theX = (230);
			int theY = (475);
			g.WriteString(theString, theX, theY, -1, -1);
			g.WriteString(GlobalMembers._ID("Best Move", 160), (230), (475) + (48), -1, -1);
			g.WriteString(GlobalMembers._ID("Best Butterfly Combo", 161), (230), (475) + (48) * 2, -1, -1);
			g.WriteString(GlobalMembers._ID("Total Time", 162), (230), (475) + (48) * 3, -1, -1);
		}

		public override void DrawStatsText(Graphics g)
		{
			string theString = Common.CommaSeperate(mGameStats[28]);
			int theX = (750);
			int theY = (475);
			g.WriteString(theString, theX, theY, -1, 1);
			g.WriteString(Common.CommaSeperate(mGameStats[25]), (750), (475) + (48), -1, 1);
			g.WriteString(Common.CommaSeperate(mGameStats[29]), (750), (475) + (48) * 2, -1, 1);
			int num = mGameStats[0];
			g.WriteString(string.Format(GlobalMembers._ID("{0}:{1:d2}", 163), num / 60, num % 60), (750), (475) + (48) * 3, -1, 1);
		}

		public override void DrawFrames(Graphics g)
		{
			g.Translate((0), (60));
			DrawLabeledStatsFrame(g);
			DrawLabeledHighScores(g);
			g.Translate((0), (-50));
			DrawSpecialGemDisplay(g);
		}

		public override void Draw(Graphics g)
		{
			base.Draw(g);
		}
	}
}
