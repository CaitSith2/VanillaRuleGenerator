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
            new ManualFileName("Capacitor Discharge.html", ResourcesSlim.Capacitor_Discharge),
			new ManualFileName("Complicated Wires.html", ResourcesSlim.Complicated_Wires),
			new ManualFileName("Keypad.html", ResourcesSlim.Keypads),
			new ManualFileName("Knob.html", ResourcesSlim.Knobs),
			new ManualFileName("Maze.html", ResourcesSlim.Mazes),
			new ManualFileName("Memory.html", ResourcesSlim.Memory),
			new ManualFileName("Morse Code.html", ResourcesSlim.Morse_Code),
			new ManualFileName("Password.html", ResourcesSlim.Passwords),
			new ManualFileName("Simon Says.html", ResourcesSlim.Simon_Says),
			new ManualFileName("The Button.html", ResourcesSlim.The_Button),
			new ManualFileName("Venting Gas.html", ResourcesSlim.Venting_Gas),
			new ManualFileName("Who’s on First.html", ResourcesSlim.Whos_on_First),
			new ManualFileName("Wire Sequence.html", ResourcesSlim.Wire_Sequences),
			new ManualFileName("Wires.html", ResourcesSlim.Wires),
			new ManualFileName("index.html", ResourcesSlim.index),
		};

		private void AddResources()
		{
			//Nothing to add in the slimmed down version.
		}

		private readonly List<ManualFileName> _keypadFiles = new List<ManualFileName>
		{
			new ManualFileName(Path.Combine("img", Path.Combine("Round Keypad", "1-copyright.png")), new byte[0]),
			new ManualFileName(Path.Combine("img", Path.Combine("Round Keypad", "2-filledstar.png")), new byte[0]),
			new ManualFileName(Path.Combine("img", Path.Combine("Round Keypad", "3-hollowstar.png")), new byte[0]),
			new ManualFileName(Path.Combine("img", Path.Combine("Round Keypad", "4-smileyface.png")), new byte[0]),
			new ManualFileName(Path.Combine("img", Path.Combine("Round Keypad", "5-doublek.png")), new byte[0]),
			new ManualFileName(Path.Combine("img", Path.Combine("Round Keypad", "6-omega.png")), new byte[0]),
			new ManualFileName(Path.Combine("img", Path.Combine("Round Keypad", "7-squidknife.png")), new byte[0]),
			new ManualFileName(Path.Combine("img", Path.Combine("Round Keypad", "8-pumpkin.png")), new byte[0]),
			new ManualFileName(Path.Combine("img", Path.Combine("Round Keypad", "9-hookn.png")), new byte[0]),
			new ManualFileName(Path.Combine("img", Path.Combine("Round Keypad", "10-teepee.png")), new byte[0]),
			new ManualFileName(Path.Combine("img", Path.Combine("Round Keypad", "11-six.png")), new byte[0]),
			new ManualFileName(Path.Combine("img", Path.Combine("Round Keypad", "12-squigglyn.png")), new byte[0]),
			new ManualFileName(Path.Combine("img", Path.Combine("Round Keypad", "13-at.png")), new byte[0]),
			new ManualFileName(Path.Combine("img", Path.Combine("Round Keypad", "14-ae.png")), new byte[0]),
			new ManualFileName(Path.Combine("img", Path.Combine("Round Keypad", "15-meltedthree.png")), new byte[0]),
			new ManualFileName(Path.Combine("img", Path.Combine("Round Keypad", "16-euro.png")), new byte[0]),
			new ManualFileName(Path.Combine("img", Path.Combine("Round Keypad", "17-circle.png")), new byte[0]),
			new ManualFileName(Path.Combine("img", Path.Combine("Round Keypad", "18-nwithhat.png")), new byte[0]),
			new ManualFileName(Path.Combine("img", Path.Combine("Round Keypad", "19-dragon.png")), new byte[0]),
			new ManualFileName(Path.Combine("img", Path.Combine("Round Keypad", "20-questionmark.png")), new byte[0]),
			new ManualFileName(Path.Combine("img", Path.Combine("Round Keypad", "21-paragraph.png")), new byte[0]),
			new ManualFileName(Path.Combine("img", Path.Combine("Round Keypad", "22-rightc.png")), new byte[0]),
			new ManualFileName(Path.Combine("img", Path.Combine("Round Keypad", "23-leftc.png")), new byte[0]),
			new ManualFileName(Path.Combine("img", Path.Combine("Round Keypad", "24-pitchfork.png")), new byte[0]),
			new ManualFileName(Path.Combine("img", Path.Combine("Round Keypad", "25-tripod.png")), new byte[0]),
			new ManualFileName(Path.Combine("img", Path.Combine("Round Keypad", "26-cursive.png")), new byte[0]),
			new ManualFileName(Path.Combine("img", Path.Combine("Round Keypad", "27-tracks.png")), new byte[0]),
			new ManualFileName(Path.Combine("img", Path.Combine("Round Keypad", "28-balloon.png")), new byte[0]),
			new ManualFileName(Path.Combine("img", Path.Combine("Round Keypad", "29-weirdnose.png")), new byte[0]),
			new ManualFileName(Path.Combine("img", Path.Combine("Round Keypad", "30-upsidedowny.png")), new byte[0]),
			new ManualFileName(Path.Combine("img", Path.Combine("Round Keypad", "31-bt.png")), new byte[0]),
		};
	}
}