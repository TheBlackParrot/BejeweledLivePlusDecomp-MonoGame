using System;
using System.Collections.Generic;
using BejeweledLivePlus.Bej3Graphics;
using BejeweledLivePlus.Misc;
using SexyFramework;
using SexyFramework.Graphics;
using SexyFramework.Misc;
using SexyFramework.Widget;
using Common = SexyFramework.Common;

namespace BejeweledLivePlus.UI
{
	internal class DigGoalEndLevelDialog : EndLevelDialog
	{
		public CurvedVal[] mTreasurePct = new CurvedVal[3];

		public double[] mTreasureDist = new double[3];

		public int[] mTreasurePauseTicks = new int[3];

		public List<Rect> mTreasureRectVector = new List<Rect>();

		public List<Rect> mBarRectVector = new List<Rect>();

		public Rect mDiamondRect = default(Rect);

		public EffectsManager mFXManager;

		public DigGoalEndLevelDialog(QuestBoard theBoard)
			: base(theBoard)
		{
			NudgeButtons((-40));
			DoTreasureAnim();
			for (int i = 0; i < 3; i++)
			{
				mTreasurePct[i] = new CurvedVal();
			}
			foreach (KeyValuePair<int, DialogButton> mBtn in mBtns)
			{
				mBtn.Value.mY += (40);
			}
			mFXManager = new EffectsManager(null);
			AddWidget(mFXManager);
			mFXManager.Resize(0, 0, (1600), (1200));
			mFXManager.mMouseVisible = false;
			mMouseInvisibleChildren.AddLast(mFXManager);
		}

		public override void KeyChar(char theChar)
		{
			base.KeyChar(theChar);
		}

		public override void Update()
		{
			base.Update();
			DigGoal digGoal = (DigGoal)((QuestBoard)mBoard).mQuestGoal;
			for (int i = 0; i < 3; i++)
			{
				if (mTreasureDist[i] > 0.0 && !mTreasurePct[i].HasBeenTriggered())
				{
					if (mTreasurePauseTicks[i] < (50))
					{
						mTreasurePauseTicks[i]++;
					}
					else
					{
						mTreasurePct[i].IncInVal();
					}
					break;
				}
			}
			if (mWidgetManager != null)
			{
				for (int j = 0; j < Common.size(mBarRectVector); j++)
				{
					string[] array = new string[3]
					{
						GlobalMembers._ID("Gold Collected", 215),
						GlobalMembers._ID("Diamonds Collected", 216),
						GlobalMembers._ID("Artifacts Collected", 217)
					};
					if (mBarRectVector[j].Contains(mWidgetManager.mLastMouseX, mWidgetManager.mLastMouseY))
					{
						GlobalMembers.gApp.mTooltipManager.RequestTooltip(this, array[j], string.Format(GlobalMembers._ID("${0}", 218), SexyFramework.Common.CommaSeperate(digGoal.mTreasureEarnings[j])), new Point(mBarRectVector[j].mX + mBarRectVector[j].mWidth / 2, mBarRectVector[j].mY + (20)), (400), 1, (25), null, null, 0, -1);
						break;
					}
				}
				for (int k = 0; k < Math.Min(Common.size(mTreasureRectVector), Common.size(digGoal.mCollectedArtifacts)); k++)
				{
					if (mTreasureRectVector[k].Contains(mWidgetManager.mLastMouseX, mWidgetManager.mLastMouseY))
					{
						DigGoal.ArtifactData artifactData = digGoal.mArtifacts[digGoal.mCollectedArtifacts[k]];
						GlobalMembers.gApp.mTooltipManager.RequestTooltip(this, artifactData.mName, string.Format(GlobalMembers._ID("${0}", 219), SexyFramework.Common.CommaSeperate(digGoal.mArtifactBaseValue * artifactData.mValue)), new Point(mTreasureRectVector[k].mX + mTreasureRectVector[k].mWidth / 2, mTreasureRectVector[k].mY + (20)), (400), 1, (25), null, null, 0, -1);
						break;
					}
				}
			}
			if (BejeweledLivePlus.Misc.Common.Rand() % (20000) < mDiamondRect.mWidth)
			{
				Effect effect = mFXManager.AllocEffect(Effect.Type.TYPE_SPARKLE_SHARD);
				effect.mDY *= (0.25f);
				float num = Math.Abs(GlobalMembersUtils.GetRandFloat());
				num *= num;
				effect.mX = mDiamondRect.mX + BejeweledLivePlus.Misc.Common.Rand() % mDiamondRect.mWidth;
				effect.mY = mDiamondRect.mY + BejeweledLivePlus.Misc.Common.Rand() % mDiamondRect.mHeight;
				effect.mColor = new Color((255), (255), (255), (255));
				effect.mIsAdditive = true;
				effect.mDScale = (0.015f);
				effect.mScale = (0.3f) + Math.Abs(GlobalMembersUtils.GetRandFloat()) * (0.2f);
				mFXManager.AddEffect(effect);
			}
		}

		public override void DrawStatsLabels(Graphics g)
		{
			g.PushState();
			g.SetColor(new Color(-1));
			g.SetFont(GlobalMembersResources.FONT_DIALOG);
			((ImageFont)g.GetFont()).PushLayerColor("Outline", new Color((4210688)));
			((ImageFont)g.GetFont()).PushLayerColor("Glow", new Color(0, 0, 0, 0));
			g.SetColor(new Color((16763736)));
			g.Translate((230), (450));
			string theString = GlobalMembers._ID("Max Depth", 204);
			g.WriteString(theString, 0, 0, -1, -1);
			g.WriteString(GlobalMembers._ID("Total Time", 205), 0, (48), -1, -1);
			g.WriteString(GlobalMembers._ID("Best Move", 206), 0, (48) * 2, -1, -1);
			g.WriteString(GlobalMembers._ID("Best Treasure", 207), 0, (48) * 3, -1, -1);
			((ImageFont)g.GetFont()).PopLayerColor("Outline");
			((ImageFont)g.GetFont()).PopLayerColor("Glow");
			g.PopState();
		}

		public override void DrawStatsText(Graphics g)
		{
			DigGoal digGoal = (DigGoal)((QuestBoard)mBoard).mQuestGoal;
			g.PushState();
			g.SetColor(new Color(-1));
			g.SetFont(GlobalMembersResources.FONT_DIALOG);
			((ImageFont)g.GetFont()).PushLayerColor("Outline", new Color((4210688)));
			((ImageFont)g.GetFont()).PushLayerColor("Glow", new Color(0, 0, 0, 0));
			g.SetColor(new Color((16763736)));
			g.Translate((545), (450));
			string theString = string.Format(GlobalMembers._ID("{0} m", 208), SexyFramework.Common.CommaSeperate(digGoal.GetGridDepth() * 10));
			int theX = (220);
			int theY = (0);
			g.WriteString(theString, theX, theY, -1, 1);
			int num = mGameStats[0];
			g.WriteString(string.Format(GlobalMembers._ID("{0}:{1:d2}", 209), num / 60, num % 60), (220), (0) + (48), -1, 1);
			g.WriteString(string.Format(GlobalMembers._ID("${0}", 210), SexyFramework.Common.CommaSeperate(mGameStats[25])), (220), (0) + (48) * 2, -1, 1);
			int num2 = 0;
			string arg = GlobalMembers._ID("No Treasures", 211);
			for (int i = 0; i < Common.size(digGoal.mCollectedArtifacts); i++)
			{
				DigGoal.ArtifactData artifactData = digGoal.mArtifacts[digGoal.mCollectedArtifacts[i]];
				if (digGoal.mArtifactBaseValue * artifactData.mValue >= num2)
				{
					arg = artifactData.mName;
					num2 = digGoal.mArtifactBaseValue * artifactData.mValue;
				}
			}
			g.WriteString(string.Format(GlobalMembers._ID("${0}", 212), SexyFramework.Common.CommaSeperate(num2)), (220), (0) + (48) * 3, -1, 1);
			g.WriteString(string.Format(GlobalMembers._ID("({0})", 213), arg), (-40), (0) + (48) * 4, -1, 0);
			((ImageFont)g.GetFont()).PopLayerColor("Outline");
			((ImageFont)g.GetFont()).PopLayerColor("Glow");
			g.PopState();
		}

		public override void DrawFrames(Graphics g)
		{
			g.PushState();
			g.DrawImageBox(new Rect((195), (385) - (0), (600), GlobalMembersResourcesWP.IMAGE_GAMEOVER_SECTION_LABEL.GetHeight()), GlobalMembersResourcesWP.IMAGE_GAMEOVER_SECTION_LABEL);
			g.DrawImage(GlobalMembersResourcesWP.IMAGE_GAMEOVER_DIG_BOX, (195), (720));
			g.PopState();
			g.SetColor(new Color(-1));
			g.SetFont(GlobalMembersResources.FONT_DIALOG);
			((ImageFont)g.GetFont()).PushLayerColor("Main", new Color((8931352)));
			((ImageFont)g.GetFont()).PushLayerColor("OUTLINE", new Color((16777215)));
			((ImageFont)g.GetFont()).PushLayerColor("GLOW", new Color(0, 0, 0, 0));
			g.WriteString(GlobalMembers._ID("Treasure Found:", 214), (800), (766));
			((ImageFont)g.GetFont()).PopLayerColor("Main");
			((ImageFont)g.GetFont()).PopLayerColor("OUTLINE");
			((ImageFont)g.GetFont()).PopLayerColor("GLOW");
			g.PushState();
			g.Translate((0), (-8));
			DrawLabeledStatsFrame(g);
			g.PopState();
			g.PushState();
			g.Translate((0), (-8));
			DrawLabeledHighScores(g);
			g.PopState();
		}

		public override void Draw(Graphics g)
		{
			g.SetFont(GlobalMembersResources.FONT_DIALOG);
			Utils.SetFontLayerColor((ImageFont)g.GetFont(), "Main", new Color(255, 255, 255, 255));
			Utils.SetFontLayerColor((ImageFont)g.GetFont(), "OUTLINE", new Color(255, 255, 255, 255));
			Utils.SetFontLayerColor((ImageFont)g.GetFont(), "GLOW", new Color(0, 0, 0, 0));
			g.DrawImageBox(new Rect((85), (0), (1430), (1200)), GlobalMembersResourcesWP.IMAGE_GAMEOVER_DIALOG);
			g.DrawImage(GlobalMembersResourcesWP.IMAGE_GAMEOVER_STAMP, (int)GlobalMembers.S(GlobalMembersResourcesWP.ImgXOfs(ResourceId.IMAGE_GAMEOVER_STAMP_ID) - 160f), (int)GlobalMembers.S(GlobalMembersResourcesWP.ImgYOfs(ResourceId.IMAGE_GAMEOVER_STAMP_ID) + 0f));
			g.SetColor(new Color(-1));
			g.SetFont(GlobalMembersResources.FONT_HEADER);
			((ImageFont)g.GetFont()).PushLayerColor("Main", new Color((8931352)));
			((ImageFont)g.GetFont()).PushLayerColor("LAYER_2", new Color((15253648)));
			((ImageFont)g.GetFont()).PushLayerColor("LAYER_3", new Color(0, 0, 0, 0));
			g.WriteString(string.Format(GlobalMembers._ID("Treasure Excavated:", 220)), (800), (140));
			((ImageFont)g.GetFont()).PopLayerColor("Main");
			((ImageFont)g.GetFont()).PopLayerColor("LAYER_2");
			((ImageFont)g.GetFont()).PopLayerColor("LAYER_3");
			((ImageFont)g.GetFont()).PushLayerColor("Main", new Color((16777215)));
			((ImageFont)g.GetFont()).PushLayerColor("LAYER_2", new Color((11558960)));
			((ImageFont)g.GetFont()).PushLayerColor("LAYER_3", new Color(0, 0, 0, 0));
			g.WriteString(string.Format(GlobalMembers._ID("${0}", 221), SexyFramework.Common.CommaSeperate((int)((double)mBoard.mPoints * (double)mCountupPct))), (800), (220));
			((ImageFont)g.GetFont()).PopLayerColor("Main");
			((ImageFont)g.GetFont()).PopLayerColor("LAYER_2");
			((ImageFont)g.GetFont()).PopLayerColor("LAYER_3");
			g.SetColor(new Color(-1));
			DrawFrames(g);
			int num = 0;
			int num2 = 0;
			DigGoal digGoal = (DigGoal)((QuestBoard)mBoard).mQuestGoal;
			for (int i = 0; i < 3; i++)
			{
				num = Math.Max(digGoal.mTreasureEarnings[i], num);
				num2 += digGoal.mTreasureEarnings[i];
			}
			((ImageFont)g.GetFont()).PushLayerColor("Outline", new Color((4210688)));
			((ImageFont)g.GetFont()).PushLayerColor("Glow", new Color(0, 0, 0, 0));
			g.SetColor(new Color((16763736)));
			Color[] array = new Color[3]
			{
				new Color((16776960)),
				new Color((11599871)),
				new Color((16777215))
			};
			Image[] array2 = new Image[3]
			{
				GlobalMembersResourcesWP.IMAGE_GAMEOVER_DIG_BAR_GOLD,
				GlobalMembersResourcesWP.IMAGE_GAMEOVER_DIG_BAR_GEMS,
				GlobalMembersResourcesWP.IMAGE_GAMEOVER_DIG_BAR_TREASURE
			};
			mTreasureRectVector.Clear();
			mBarRectVector.Clear();
			if (num > 0)
			{
				for (int j = 0; j < 3; j++)
				{
					int num3 = (190) + (760) * digGoal.mTreasureEarnings[j] / num;
					int num4 = (int)((double)num3 * (double)mCountupPct);
					if (j == 1)
					{
						mDiamondRect = new Rect((198), (790) + j * (92), (int)((float)num4 / (1f) - (float)(105)), (68));
					}
					g.DrawImage(array2[j], (198), (780) + j * (92), new Rect(array2[j].mWidth - num4, 0, num4, array2[j].mHeight));
					g.SetColor(array[j]);
					g.WriteString(string.Format(GlobalMembers._ID("${0}", 223), SexyFramework.Common.CommaSeperate(digGoal.mTreasureEarnings[j])), (1380), (840) + j * (92), -1, 1);
					Color color = array[j];
					color.mAlpha = (int)((double)color.mAlpha * Math.Max(0.0, Math.Min(1.0, (double)mCountupPct * (2.0) - (0.9))));
					g.SetColor(color);
					g.WriteString(string.Format(GlobalMembers._ID("{0}%", 222), (int)((double)digGoal.mTreasureEarnings[j] * 100.0 / (double)num2 + 0.5)), (125) + num4, (840) + j * (92), -1, 0);
					if (j == 2 && Common.size(digGoal.mCollectedArtifacts) != 0)
					{
						int num5 = Common.size(digGoal.mCollectedArtifacts);
						for (int k = 0; k < num5; k++)
						{
							DigGoal.ArtifactData artifactData = digGoal.mArtifacts[digGoal.mCollectedArtifacts[k]];
							float num6 = MathF.Min((0.5f), MathF.Max((0.25f), (0.75f) - (float)num5 / (float)(num3 - (135)) * (22f)));
							int num7 = (210) + (int)((double)(num4 - (135)) * ((double)k + 0.5) / (double)num5);
							int num8 = (1010);
							if ((double)num6 < (0.26))
							{
								num7 = (210) + (int)((double)(num4 - (135)) * ((double)(k / 2) + 0.5) / (double)((num5 + 1) / 2));
								num8 = ((k % 2 != 0) ? (num8 + (20)) : (num8 - (20)));
							}
							g.SetColorizeImages(true);
							g.SetColor(mCountupPct);
							Transform transform = new Transform();
							transform.Scale(num6, num6);
							g.DrawImageTransform(GlobalMembersResourcesWP.GetImageById(artifactData.mImageId), transform, num7, num8);
							g.SetColorizeImages(false);
							mTreasureRectVector.Add(new Rect((int)((float)num7 - (float)(80) * num6), (int)((float)num8 - (float)(80) * num6), (int)((float)(160) * num6), (int)((float)(160) * num6)));
						}
					}
					else
					{
						mBarRectVector.Add(new Rect((198), (790) + j * (92), num4 - (10), (90)));
					}
				}
			}
			((ImageFont)g.GetFont()).PopLayerColor("Outline");
			((ImageFont)g.GetFont()).PopLayerColor("Glow");
		}

		public void DoTreasureAnim()
		{
			DigGoal digGoal = (DigGoal)((QuestBoard)mBoard).mQuestGoal;
			double num = 0.0;
			for (int i = 0; i < 3; i++)
			{
				mTreasureDist[i] = 0.0;
				mTreasurePauseTicks[i] = 0;
				num += (double)digGoal.mTreasureEarnings[i];
			}
			if (!(num > 0.0))
			{
				return;
			}
			double num2 = (0.1);
			double num3 = Math.Min(1.0, (0.6) + (0.4) * num / (double)(1000000));
			num3 *= (1.2);
			for (int j = 0; j < 3; j++)
			{
				if ((double)digGoal.mTreasureEarnings[j] > 0.0 && (double)digGoal.mTreasureEarnings[j] / num < num2)
				{
					mTreasureDist[j] = num2;
					num3 -= num2;
				}
			}
			for (int k = 0; k < 3; k++)
			{
				if (mTreasureDist[k] == 0.0)
				{
					mTreasureDist[k] = (double)digGoal.mTreasureEarnings[k] / num * num3;
				}
				if (mTreasureDist[k] > 0.0)
				{
					mTreasurePct[k].SetCurve(("b-0,1,0.01,1,####        J~### V~###"));
					if ((1) == 1)
					{
						mTreasurePct[k].mIncRate *= 1.0 / (mTreasureDist[k] / num3);
					}
				}
			}
		}
	}
}
