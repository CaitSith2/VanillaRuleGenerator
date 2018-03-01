using System.Collections.Generic;
using System.IO;
using VanillaRuleGenerator.Helpers;
using VanillaRuleGenerator.Properties;

namespace VanillaRuleGenerator
{
	public sealed partial class ManualGenerator
	{
		private readonly List<ManualFileName> _manualFileNames = new List<ManualFileName>()
		{
            //HTML Manuals
            new ManualFileName("Capacitor Discharge.html", Resources.Capacitor_Discharge),
			new ManualFileName("Complicated Wires.html", Resources.Complicated_Wires),
			new ManualFileName("Keypad.html", Resources.Keypads),
			new ManualFileName("Knob.html", Resources.Knobs),
			new ManualFileName("Maze.html", Resources.Mazes),
			new ManualFileName("Memory.html", Resources.Memory),
			new ManualFileName("Morse Code.html", Resources.Morse_Code),
			new ManualFileName("Password.html", Resources.Passwords),
			new ManualFileName("Simon Says.html", Resources.Simon_Says),
			new ManualFileName("The Button.html", Resources.The_Button),
			new ManualFileName("Venting Gas.html", Resources.Venting_Gas),
			new ManualFileName("Who’s on First.html", Resources.Whos_on_First),
			new ManualFileName("Wire Sequence.html", Resources.Wire_Sequences),
			new ManualFileName("Wires.html", Resources.Wires),
			new ManualFileName("index.html", Resources.index),
		};

		private void AddResources()
		{
			ManualFileName[] resources =
			{
				//CSS
				new ManualFileName(Path.Combine("css", "dark-theme.css"), Resources.dark_theme),
				new ManualFileName(Path.Combine("css", "font.css"), Resources.font),
				new ManualFileName(Path.Combine("css", "jquery-ui.1.12.1.css"), Resources.jquery_ui_1_12_1),
				new ManualFileName(Path.Combine("css", "main.css"), Resources.main),
				new ManualFileName(Path.Combine("css", "main-before.css"), Resources.main_before),
				new ManualFileName(Path.Combine("css", "main-orig.css"), Resources.main_orig),
				new ManualFileName(Path.Combine("css", "normalize.css"), Resources.normalize),

				//Font
				new ManualFileName(Path.Combine("font", "SpecialElite.ttf"), Resources.SpecialElite),
				new ManualFileName(Path.Combine("font", "trebuc.ttf"), Resources.trebuc),
				new ManualFileName(Path.Combine("font", "trebucbd.ttf"), Resources.trebucbd),
				new ManualFileName(Path.Combine("font", "trebucbi.ttf"), Resources.trebucbi),
				new ManualFileName(Path.Combine("font", "trebucit.ttf"), Resources.trebucit),

				//img
				new ManualFileName(Path.Combine("img", "Bomb.svg"), Resources.Bomb),
				new ManualFileName(Path.Combine("img", "BombSide.svg"), Resources.BombSide),
				new ManualFileName(Path.Combine("img", "ktane-logo.png"), Resources.ktane_logo),
				new ManualFileName(Path.Combine("img", "page-bg-noise-01.png"), Resources.page_bg_noise_01),
				new ManualFileName(Path.Combine("img", "page-bg-noise-02.png"), Resources.page_bg_noise_02),
				new ManualFileName(Path.Combine("img", "page-bg-noise-03.png"), Resources.page_bg_noise_03),
				new ManualFileName(Path.Combine("img", "page-bg-noise-04.png"), Resources.page_bg_noise_04),
				new ManualFileName(Path.Combine("img", "page-bg-noise-05.png"), Resources.page_bg_noise_05),
				new ManualFileName(Path.Combine("img", "page-bg-noise-06.png"), Resources.page_bg_noise_06),
				new ManualFileName(Path.Combine("img", "page-bg-noise-07.png"), Resources.page_bg_noise_07),
				new ManualFileName(Path.Combine("img", "web-background.jpg"), Resources.web_background),
				new ManualFileName(Path.Combine("img", Path.Combine("appendix-batteries", "Battery-AA.svg")), Resources.Battery_AA),
				new ManualFileName(Path.Combine("img", Path.Combine("appendix-batteries", "Battery-D.svg")), Resources.Battery_D),
				new ManualFileName(Path.Combine("img", Path.Combine("appendix-ports", "DVI.svg")), Resources.DVI),
				new ManualFileName(Path.Combine("img", Path.Combine("appendix-ports", "Parallel.svg")), Resources.Parallel),
				new ManualFileName(Path.Combine("img", Path.Combine("appendix-ports", "PS2.svg")), Resources.PS2),
				new ManualFileName(Path.Combine("img", Path.Combine("appendix-ports", "RJ45.svg")), Resources.RJ45),
				new ManualFileName(Path.Combine("img", Path.Combine("appendix-ports", "Serial.svg")), Resources.Serial),
				new ManualFileName(Path.Combine("img", Path.Combine("appendix-ports", "StereoRCA.svg")), Resources.StereoRCA),
				new ManualFileName(Path.Combine("img", Path.Combine("Component", "Component.svg")), Resources.Component),
				new ManualFileName(Path.Combine("img", Path.Combine("Component", "IndicatorWidget.svg")), Resources.IndicatorWidget),
				new ManualFileName(Path.Combine("img", Path.Combine("Component", "NeedyComponent.svg")), Resources.NeedyComponent),
				new ManualFileName(Path.Combine("img", Path.Combine("Component", "TimerComponent.svg")), Resources.TimerComponent),
				new ManualFileName(Path.Combine("img", Path.Combine("Component", "Capacitor Discharge.svg")), Resources.Capacitor_Discharge1),
				new ManualFileName(Path.Combine("img", Path.Combine("Component", "Complicated Wires.svg")), Resources.Complicated_Wires1),
				new ManualFileName(Path.Combine("img", Path.Combine("Component", "Keypads.svg")), Resources.Keypads1),
				new ManualFileName(Path.Combine("img", Path.Combine("Component", "Knobs.svg")), Resources.Knobs1),
				new ManualFileName(Path.Combine("img", Path.Combine("Component", "Mazes.svg")), Resources.Mazes1),
				new ManualFileName(Path.Combine("img", Path.Combine("Component", "Memory.svg")), Resources.Memory1),
				new ManualFileName(Path.Combine("img", Path.Combine("Component", "Morse Code.svg")), Resources.Morse_Code1),
				new ManualFileName(Path.Combine("img", Path.Combine("Component", "Passwords.svg")), Resources.Passwords1),
				new ManualFileName(Path.Combine("img", Path.Combine("Component", "Simon Says.svg")), Resources.Simon_Says1),
				new ManualFileName(Path.Combine("img", Path.Combine("Component", "The Button.svg")), Resources.The_Button1),
				new ManualFileName(Path.Combine("img", Path.Combine("Component", "Venting Gas.svg")), Resources.Venting_Gas1),
				new ManualFileName(Path.Combine("img", Path.Combine("Component", "Who’s on First.svg")), Resources.Whos_on_First1),
				new ManualFileName(Path.Combine("img", Path.Combine("Component", "Wire Sequences.svg")), Resources.Wire_Sequences1),
				new ManualFileName(Path.Combine("img", Path.Combine("Component", "Wires.svg")), Resources.Wires1),
				new ManualFileName(Path.Combine("img", Path.Combine("Morsematics", "International_Morse_Code.svg")), Resources.International_Morse_Code),
				new ManualFileName(Path.Combine("img", Path.Combine("Simon Says", "SimonComponent_ColourLegend.svg")), Resources.Simon_Says1),
				new ManualFileName(Path.Combine("img", Path.Combine("Who’s on First", "eye-icon.png")), Resources.eye_icon),

				//js
				new ManualFileName(Path.Combine("js", "highlighter.js"), Resources.highlighter_js),
				new ManualFileName(Path.Combine("js", "jquery-ui.1.12.1.min.js"), Resources.jquery_3_1_1_min_js),
				new ManualFileName(Path.Combine("js", "jquery.3.1.1.min.js"), Resources.jquery_3_1_1_min_js),
			};
			_manualFileNames.AddRange(resources);
		}

		private readonly List<ManualFileName> _keypadFiles = new List<ManualFileName>
		{
			new ManualFileName(Path.Combine("img", Path.Combine("Round Keypad", "1-copyright.png")), Resources._1_copyright),
			new ManualFileName(Path.Combine("img", Path.Combine("Round Keypad", "2-filledstar.png")), Resources._2_filledstar),
			new ManualFileName(Path.Combine("img", Path.Combine("Round Keypad", "3-hollowstar.png")), Resources._3_hollowstar),
			new ManualFileName(Path.Combine("img", Path.Combine("Round Keypad", "4-smileyface.png")), Resources._4_smileyface),
			new ManualFileName(Path.Combine("img", Path.Combine("Round Keypad", "5-doublek.png")), Resources._5_doublek),
			new ManualFileName(Path.Combine("img", Path.Combine("Round Keypad", "6-omega.png")), Resources._6_omega),
			new ManualFileName(Path.Combine("img", Path.Combine("Round Keypad", "7-squidknife.png")), Resources._7_squidknife),
			new ManualFileName(Path.Combine("img", Path.Combine("Round Keypad", "8-pumpkin.png")), Resources._8_pumpkin),
			new ManualFileName(Path.Combine("img", Path.Combine("Round Keypad", "9-hookn.png")), Resources._9_hookn),
			new ManualFileName(Path.Combine("img", Path.Combine("Round Keypad", "10-teepee.png")), Resources._10_teepee),
			new ManualFileName(Path.Combine("img", Path.Combine("Round Keypad", "11-six.png")), Resources._11_six),
			new ManualFileName(Path.Combine("img", Path.Combine("Round Keypad", "12-squigglyn.png")), Resources._12_squigglyn),
			new ManualFileName(Path.Combine("img", Path.Combine("Round Keypad", "13-at.png")), Resources._13_at),
			new ManualFileName(Path.Combine("img", Path.Combine("Round Keypad", "14-ae.png")), Resources._14_ae),
			new ManualFileName(Path.Combine("img", Path.Combine("Round Keypad", "15-meltedthree.png")), Resources._15_meltedthree),
			new ManualFileName(Path.Combine("img", Path.Combine("Round Keypad", "16-euro.png")), Resources._16_euro),
			new ManualFileName(Path.Combine("img", Path.Combine("Round Keypad", "17-circle.png")), Resources._17_circle),
			new ManualFileName(Path.Combine("img", Path.Combine("Round Keypad", "18-nwithhat.png")), Resources._18_nwithhat),
			new ManualFileName(Path.Combine("img", Path.Combine("Round Keypad", "19-dragon.png")), Resources._19_dragon),
			new ManualFileName(Path.Combine("img", Path.Combine("Round Keypad", "20-questionmark.png")), Resources._20_questionmark),
			new ManualFileName(Path.Combine("img", Path.Combine("Round Keypad", "21-paragraph.png")), Resources._21_paragraph),
			new ManualFileName(Path.Combine("img", Path.Combine("Round Keypad", "22-rightc.png")), Resources._22_rightc),
			new ManualFileName(Path.Combine("img", Path.Combine("Round Keypad", "23-leftc.png")), Resources._23_leftc),
			new ManualFileName(Path.Combine("img", Path.Combine("Round Keypad", "24-pitchfork.png")), Resources._24_pitchfork),
			new ManualFileName(Path.Combine("img", Path.Combine("Round Keypad", "25-tripod.png")), Resources._25_tripod),
			new ManualFileName(Path.Combine("img", Path.Combine("Round Keypad", "26-cursive.png")), Resources._26_cursive),
			new ManualFileName(Path.Combine("img", Path.Combine("Round Keypad", "27-tracks.png")), Resources._27_tracks),
			new ManualFileName(Path.Combine("img", Path.Combine("Round Keypad", "28-balloon.png")), Resources._28_balloon),
			new ManualFileName(Path.Combine("img", Path.Combine("Round Keypad", "29-weirdnose.png")), Resources._29_weirdnose),
			new ManualFileName(Path.Combine("img", Path.Combine("Round Keypad", "30-upsidedowny.png")), Resources._30_upsidedowny),
			new ManualFileName(Path.Combine("img", Path.Combine("Round Keypad", "31-bt.png")), Resources._31_bt),
		};
	}
}