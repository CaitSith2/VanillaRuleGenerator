﻿using System.Collections.Generic;
using System.Drawing;
using VanillaRuleGenerator.Properties;

namespace VanillaRuleGenerator.Rules
{
	public partial class SymbolPool
	{
		public static Bitmap GetImage(string symbol)
		{
			return ImageMap[symbol];
		}

		public static Dictionary<string, Bitmap> ImageMap = new Dictionary<string, Bitmap>
		{
			{
				"©",
				Resources._1_copyright
			},
			{
				"★",
				Resources._2_filledstar
			},
			{
				"☆",
				Resources._3_hollowstar
			},
			{
				"ټ",
				Resources._4_smileyface
			},
			{
				"Җ",
				Resources._5_doublek
			},
			{
				"Ω",
				Resources._6_omega
			},
			{
				"Ѭ",
				Resources._7_squidknife
			},
			{
				"Ѽ",
				Resources._8_pumpkin
			},
			{
				"ϗ",
				Resources._9_hookn
			},
			{
				"ϫ",
				Resources._10_teepee
			},
			{
				"Ϭ",
				Resources._11_six
			},
			{
				"Ϟ",
				Resources._12_squigglyn
			},
			{
				"Ѧ",
				Resources._13_at
			},
			{
				"ӕ",
				Resources._14_ae
			},
			{
				"Ԇ",
				Resources._15_meltedthree
			},
			{
				"Ӭ",
				Resources._16_euro
			},
			{
                /*"҈"*/"◌",
				Resources._17_circle
			},
			{
				"Ҋ",
				Resources._18_nwithhat
			},
			{
				"ѯ",
				Resources._19_dragon
			},
			{
				"¿",
				Resources._20_questionmark
			},
			{
				"¶",
				Resources._21_paragraph
			},
			{
				"Ͼ",
				Resources._22_rightc
			},
			{
				"Ͽ",
				Resources._23_leftc
			},
			{
				"Ψ",
				Resources._24_pitchfork
			},
			{
				"Ѫ",
				Resources._25_tripod
			},
			{
				"Ҩ",
				Resources._26_cursive
			},
			{
				"҂",
				Resources._27_tracks
			},
			{
				"Ϙ",
				Resources._28_balloon
			},
			{
				"ζ",
				Resources._29_weirdnose
			},
			{
				"ƛ",
				Resources._30_upsidedowny
			},
			{
				"ѣ",
				Resources._31_bt
			}
		};
	}
}