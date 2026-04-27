using SexyFramework;
using SexyFramework.Graphics;

namespace BejeweledLivePlus.UI
{
	internal class ClassicEndLevelDialog : EndLevelDialog
	{
		public ClassicBoard mClassicBoard;

		public ClassicEndLevelDialog(ClassicBoard theBoard)
			: base(theBoard)
		{
			mClassicBoard = theBoard;
			NudgeButtons((-40));
		}

		public override void Update()
		{
			base.Update();
		}

		public override void DrawStatsLabels(Graphics g)
		{
			string theString = GlobalMembers._ID("Level Achieved", 165);
			int theX = (230);
			int theY = (475);
			g.WriteString(theString, theX, theY, -1, -1);
			g.WriteString(GlobalMembers._ID("Best Move", 166), (230), (475) + (48), -1, -1);
			g.WriteString(GlobalMembers._ID("Longest Cascade", 167), (230), (475) + (48) * 2, -1, -1);
			g.WriteString(GlobalMembers._ID("Total Time", 168), (230), (475) + (48) * 3, -1, -1);
		}

		public override void DrawStatsText(Graphics g)
		{
			string theString = Common.CommaSeperate(mLevel + 1);
			int theX = (760);
			int theY = (475);
			g.WriteString(theString, theX, theY, -1, 1);
			g.WriteString(Common.CommaSeperate(mGameStats[25]), (760), (475) + (48), -1, 1);
			g.WriteString(Common.CommaSeperate(mGameStats[24]), (760), (475) + (48) * 2, -1, 1);
			int num = mGameStats[0];
			g.WriteString(string.Format(GlobalMembers._ID("{0}:{1:d2}", 169), num / 60, num % 60), (760), (475) + (48) * 3, -1, 1);
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
