using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using VanillaRuleGenerator.Extensions;
using VanillaRuleGenerator.Helpers;
using VanillaRuleGenerator.Properties;
using VanillaRuleGenerator.Rules;
using VanillaRuleGenerator.Rules.BombGame;

namespace VanillaRuleGenerator
{
    public sealed partial class ManualGenerator
    {
        //public TextAsset[] ManualDataFiles;
        private Dictionary<string,object> HTMLManualGenerators = new Dictionary<string, object>();

	    private void CreateDirectories()
	    {
			try
			{
				if (!string.IsNullOrEmpty(ModAssemblyDirectory))
					Directory.CreateDirectory(ModAssemblyDirectory);
				if (!string.IsNullOrEmpty(ManualCacheDirectory))
					Directory.CreateDirectory(ManualCacheDirectory);
			}
			catch (Exception ex)
			{
				Debug.LogException(ex, "Failed to create specified directories:");
			}
		}

	    private static string ModAssemblyDirectory;
	    private static string ManualCacheDirectory;
		private ManualGenerator()
		{
			Initialize("ModAssemblies", "ModifiedVanillaManuals");
        }
        public static ManualGenerator Instance => Nested.instance;

        // ReSharper disable once ClassNeverInstantiated.Local
        private class Nested
        {
            static Nested() { }
            // ReSharper disable once InconsistentNaming
            internal static readonly ManualGenerator instance = new ManualGenerator();
        }

	    private void Initialize(string modAssemblyDirectory, string manualCacheDirectory)
	    {
			ModAssemblyDirectory = modAssemblyDirectory;
		    ManualCacheDirectory = manualCacheDirectory;
		    CreateDirectories();
		    AddResources();
		    for (var i = 0; i <= (int)HTMLManualNames.Index; i++)
		    {
			    HTMLManualGenerators[_manualFileNames[i].Name] = (HTMLManualNames)i;
		    }
		    LoadModAssemblies();
		}

	    public ManualGenerator(string modAssemblyDirectory, string manualCacheDirectory)
	    {
		    Initialize(modAssemblyDirectory, manualCacheDirectory);
	    }

	    private void LoadModAssemblies()
	    {
		    if (string.IsNullOrEmpty(ModAssemblyDirectory)) return;
			if (Directory.Exists(ModAssemblyDirectory))
			{
				foreach (var file in Directory.GetFiles(ModAssemblyDirectory, "*.dll"))
				{
					var modRuleGenerator = LoadModRuleGeneratorAssembly(file);
					if (modRuleGenerator == null)
						continue;
					var manualName = modRuleGenerator.GetHTMLFileName();
					HTMLManualGenerators[manualName] = modRuleGenerator;
				}
			}
		}

		public static void DebugLog(string message, params object[] args)
        {
            CommonReflectedTypeInfo.DebugLog(message, args);
        }

        private static Dictionary<string, ModRuleGenerator> _ruleGenerators = new Dictionary<string, ModRuleGenerator>();
        private static Dictionary<string, bool> _assemblyLoadFailure = new Dictionary<string, bool>();

        public static ModRuleGenerator LoadModRuleGeneratorAssembly(string assemblyName)
        {
            if (_assemblyLoadFailure.ContainsKey(assemblyName))
                return null;

	        if (_ruleGenerators.TryGetValue(assemblyName, out ModRuleGenerator ruleGenerator)) return ruleGenerator;
	        try
	        {
		        ruleGenerator = ModRuleGenerator.GetRuleGenerator(Assembly.LoadFrom(assemblyName));
		        if (ruleGenerator != null)
		        {
			        _ruleGenerators[assemblyName] = ruleGenerator;
		        }
		        else
		        {
			        _assemblyLoadFailure[assemblyName] = true;
			        return null;
		        }
	        }
	        catch (FileNotFoundException ex)
	        {
				Debug.LogException(ex, $"File {assemblyName} was not found:");
		        return null;
	        }
	        catch (FileLoadException ex)
	        {
		        Debug.LogException(ex, $"A FileLoadException happened while loading assembly {assemblyName}:");
		        return null;
	        }
	        catch (Exception ex)
	        {
		        Debug.LogException(ex, $"Could not load {assemblyName} due to an exception. It is being skipped for the rest of this session.");
		        _assemblyLoadFailure[assemblyName] = true;
		        return null;
	        }
	        return ruleGenerator;
        }

        public static bool GenerateModManual(string assemblyName, int seed)
        {
            var ruleGenerator = LoadModRuleGeneratorAssembly(assemblyName);
            if (ruleGenerator == null) return false;

            var manualPath = ManualGenerator.GetManualPath(seed);
            ruleGenerator.WriteAllFiles(seed, manualPath);
            return true;
        }

        private void WriteComplicatedWiresManual(string path, ManualFileName file, ref List<ReplaceText> replacements, bool outputFile = true)
        {

            var lineTypes = new List<string>
            {
                "15,40,4,10",
                string.Empty,
                "3",
                "8"
            };
            var labels = new List<string>
            {
                "Wire has red\ncoloring",
                "Wire has blue\ncoloring",
                "Has ★ symbol",
                "LED is on"
            };

            var ruleset = _ruleManager.VennWireRuleSet;
            var cutInstructionList = new List<CutInstruction>
            {
                ruleset.RuleDict[new VennWireState(true, false, false, false)],
                ruleset.RuleDict[new VennWireState(false, true, false, false)],
                ruleset.RuleDict[new VennWireState(false, false, true, false)],
                ruleset.RuleDict[new VennWireState(false, false, false, true)],
                ruleset.RuleDict[new VennWireState(true, false, true, false)],
                ruleset.RuleDict[new VennWireState(true, true, false, false)],
                ruleset.RuleDict[new VennWireState(false, true, false, true)],
                ruleset.RuleDict[new VennWireState(false, false, true, true)],
                ruleset.RuleDict[new VennWireState(false, true, true, false)],
                ruleset.RuleDict[new VennWireState(true, false, false, true)],
                ruleset.RuleDict[new VennWireState(true, true, true, false)],
                ruleset.RuleDict[new VennWireState(true, true, false, true)],
                ruleset.RuleDict[new VennWireState(false, true, true, true)],
                ruleset.RuleDict[new VennWireState(true, false, true, true)],
                ruleset.RuleDict[new VennWireState(true, true, true, true)],
                ruleset.RuleDict[new VennWireState(false, false, false, false)]
            };

            var vennList = new List<string>();
            using (var enumerator = cutInstructionList.GetEnumerator())
            {
                while (enumerator.MoveNext())
                {
                    // ReSharper disable once SwitchStatementMissingSomeCases
                    switch (enumerator.Current)
                    {
                        case CutInstruction.Cut:
                            vennList.Add("C");
                            break;
                        case CutInstruction.DoNotCut:
                            vennList.Add("D");
                            break;
                        case CutInstruction.CutIfSerialEven:
                            vennList.Add("S");
                            break;
                        case CutInstruction.CutIfParallelPortPresent:
                            vennList.Add("P");
                            break;
                        case CutInstruction.CutIfTwoOrMoreBatteriesPresent:
                            vennList.Add("B");
                            break;
                    }
                }
            }

            var vennSVG = new SVGGenerator(800, 650);
            var legendSVG = new SVGGenerator(275, 200);
            vennSVG.Draw4SetVennDiagram(vennList, lineTypes);
            legendSVG.DrawVennDiagramLegend(labels, lineTypes);
            replacements.Add(new ReplaceText {Original = "VENNDIAGRAMSVGDATA", Replacement = vennSVG.ToString()});
            replacements.Add(new ReplaceText {Original = "VENNLEGENDSVGDATA", Replacement = legendSVG.ToString()});
            file.WriteFile(path, replacements, outputFile);
        }

        private void WriteMazesManual(string path, ManualFileName file, ref List<ReplaceText> replacements, bool outputFile = true)
        {
            var mazes = _ruleManager.MazeRuleSet.GetMazes();
            for (var i = 0; i < mazes.Count; i++)
            {
                replacements.Add(new ReplaceText {Original = $"MAZE{i}SVGDATA", Replacement = mazes[i].ToSVG().Replace("<svg ", "<svg class=\"maze\" ") });
            }
            file.WriteFile(path, replacements, outputFile);
        }

        private void WriteSimonSaysManual(string path, ManualFileName file, ref List<ReplaceText> replacements, bool outputFile = true)
        {
            var rules = _ruleManager.SimonRuleSet.RuleList;
            foreach (var keyValuePair in rules)
            {
                var colors = new[] { "RED", "BLUE", "GREEN", "YELLOW" };
                for (var i = 0; i < keyValuePair.Value.Count; i++)
                {
                    for (var j = 0; j < 4; j++)
                    {
                        replacements.Add(new ReplaceText() { Original = $"{keyValuePair.Key}{i}{colors[j]}", Replacement = keyValuePair.Value[i][j].ToString() });
                    }
                }
            }
            file.WriteFile(path, replacements, outputFile);
        }

        private void WritePasswordManual(string path, ManualFileName file, ref List<ReplaceText> replacements, bool outputFile = true)
        {
            var passwordrules = _ruleManager.PasswordRuleSet.possibilities;
            for (var i = 0; i < passwordrules.Count; i++)
            {
                replacements.Add(new ReplaceText { Original = $"PASSWORD{i:00}", Replacement = passwordrules[i] });
            }

            file.WriteFile(path, replacements, outputFile);
        }

        private void WriteNeedyKnobManual(string path, ManualFileName file, ref List<ReplaceText> replacements, bool outputFile = true)
        {
            var replacement = string.Empty;
            var currentDirection = string.Empty;
            foreach (var rule in _ruleManager.NeedyKnobRuleSet.Rules)
            {
                var direction = rule.Solution.Text;
                if (currentDirection != direction)
                {
                    replacement += $"                            <h4>{direction}:</h4>\n";
                    currentDirection = direction;
                }
                foreach (var query in rule.Queries)
                {
                    var leds = (bool[])query.Args[NeedyKnobRuleSet.LED_CONFIG_ARG_KEY];
                    replacement += "                            <table style=\"display: inline-table\">\n";
                    for (var i = 0; i < NeedyKnobRuleSetGenerator.LED_ROWS; i++)
                    {
                        replacement += "                                <tr>\n";
                        for (var j = 0; j < NeedyKnobRuleSetGenerator.LED_COLS; j++)
                        {
                            if (leds[NeedyKnobRuleSetGenerator.LED_COLS * i + j])
                            {
                                replacement += "                                <td>X</td>\n";
                            }
                            else
                            {
                                replacement += "                                <td>&nbsp;</td>\n";
                            }
                        }
                        replacement += "                                </tr>\n";
                    }
                    replacement += "                            </table>\n";
                }
            }


            replacements.Add(new ReplaceText { Original = "NEEDYKNOBLIGHTCONFIGURATION", Replacement = replacement });
            file.WriteFile(path, replacements, outputFile);

        }

        private void WriteKeypadsManual(string path, ManualFileName file, ref List<ReplaceText> replacements, bool outputFile = true)
        {
            var table = string.Empty;
            var rules = _ruleManager.KeypadRuleSet.PrecedenceLists;

            for (var i = 0; i < rules[0].Count; i++)
            {
                table += "                                <tr>\n";
                for (var j = 0; j < rules.Count; j++)
                {
                    table += "                                    <td class=\"keypad-table-column\"><img class=\"keypad-symbol-image\" src=\"";

                    var index = SymbolPool.Symbols.IndexOf(rules[j][i]);
                    table += _keypadFiles[index].Name.Replace(Path.DirectorySeparatorChar, '/');
                    table += "\"></img>";
                    table += "                                    </td>\n";
                    if (j == (rules.Count - 1))
                        break;
                    table += "                                    <td class=\"keypad-table-spacer\"></td>\n";
                }
                table += "                                </tr>\n";
            }
            replacements.Add(new ReplaceText() { Original = "<!--KEYPADTABLE GOES HERE-->", Replacement = table });
            foreach (var imagefile in _keypadFiles)
            {
                imagefile.WriteFile(path, outputFile);
            }

            file.WriteFile(path, replacements, outputFile);
        }

        private void WriteWhosOnFirstManual(string path, ManualFileName file, ref List<ReplaceText> replacements, bool outputFile = true)
        {
            var step1Precedentlist = _ruleManager.WhosOnFirstRuleSet.displayWordToButtonIndexMap;

            var replace = string.Empty;
            for (var i = 0; i < 5; i++)
            {
                replace += "                                <tr>\n";
                if (i == 4)
                {
                    replace += "                                    <td></td>\n";
                }
                for (var j = 0; j < ((i == 4) ? 4 : 6); j++)
                {
                    var word = WhosOnFirstRuleSet.DisplayWords[(i * 6) + j];
                    var index = step1Precedentlist[word].ToString();

                    replace += "                                    <td>\n";
                    replace += "                                        <table>\n";
                    replace += "                                            <tr>\n";
                    replace += "                                                <th class=\"whos-on-first-look-at-display\" colspan=\"2\">";
                    replace += word.Trim();
                    replace += "</th>\n";
                    replace += "                                            </tr>\n";
                    for (var k = 0; k < 3; k++)
                    {
                        replace += "                                            <tr>\n";
                        for (var l = 0; l < 2; l++)
                        {
                            if (index.Trim().Equals(((k * 2) + l).ToString()))
                            {
                                replace += "<td class=\"whos-on-first-look-at\"><img src=\"img/Who’s on First/eye-icon.png\" alt=\"Look At\" style=\"height: 1em;\" /></td>";
                            }
                            else
                            {
                                replace += "                                                <td><br /></td>\n";
                            }
                        }
                        replace += "                                            </tr>\n";
                    }

                    replace += "                                        </table>\n";
                    replace += "                                    </td>\n";
                }
                replace += "                                </tr>\n";
            }
            replacements.Add(new ReplaceText { Original = "LOOKATDISPLAYMAP", Replacement = replace });
            replace = string.Empty;

            foreach (var map in _ruleManager.WhosOnFirstRuleSet.precedenceMap)
            {
                replace += "                                <tr>\n";
                replace += "                                    <th>";
                replace += map.Key;
                replace += "</th>\n";
                replace += "                                    <td>";
                replace += string.Join(", ", map.Value.ToArray());
                replace += "</td>";
                replace += "                                </tr>\n";
            }
            replacements.Add(new ReplaceText { Original = "PRECEDENTMAP", Replacement = replace });

            file.WriteFile(path, replacements, outputFile);
        }

        private void WriteWireSequenceManual(string path, ManualFileName file, ref List<ReplaceText> replacements, bool outputFile = true)
        {
            var wiresequencetable = string.Empty;
            var wireLetters = new[] { "A", "B", "C" };
            for (var i = WireSequenceRuleSetGenerator.NUM_COLOURS - 1; i >= 0; i--)
            {
                var color = (WireColor)i;
                wiresequencetable += $"<table class=\'{color}'>";
                wiresequencetable += $"<tr><th colspan=\'2\' class=\'header\'>{color.ToString().Capitalize()} Wire Occurrences</th></tr>";
                wiresequencetable += "<tr><th class=\'first-col\'>Wire Occurrence</th><th class=\'second-col\'>Cut if connected to:</th></tr>";
                for (var j = 0; j < WireSequenceRuleSetGenerator.NumWiresPerColour; j++)
                {
                    wiresequencetable += $"<tr><td class=\'first-col\'>{Util.OrdinalWord(j + 1)}&nbsp;{color} occurrence</td><td class=\'second-col\'>";
                    var list = new List<string>();
                    for (var k = 0; k < WireSequenceRuleSetGenerator.NUM_PER_PAGE; k++)
                    {
                        if (_ruleManager.WireSequenceRuleSet.ShouldBeSnipped(color, j, k))
                        {
                            list.Add(wireLetters[k]);
                        }
                    }
                    for (var k = 0; k < list.Count; k++)
                    {
                        wiresequencetable += list[k];
                        if (k == list.Count - 2)
                            wiresequencetable += " or ";
                        else if (k < list.Count - 2 && list.Count > 2)
                            wiresequencetable += ", ";
                    }
                    if (list.Count == 0)
                        wiresequencetable += "Never Cut";
                    wiresequencetable += "</td></tr>";
                }
                wiresequencetable += "</table>\n";
            }

            replacements.Add(new ReplaceText { Original = "WIRESEQUENCETABLES", Replacement = wiresequencetable });
            file.WriteFile(path, replacements, outputFile);
        }

        private void WriteMorseCodeManaul(string path, ManualFileName file, ref List<ReplaceText> replacements, bool outputFile = true)
        {
            var worddict = _ruleManager.MorseCodeRuleSet.WordDict;
            var validFreqs = _ruleManager.MorseCodeRuleSet.ValidFrequencies;
            var morsecodetable = validFreqs.Aggregate(string.Empty, (current, freq) => current + $"<tr><td>{worddict[freq]}</td><td>3.{freq} MHz</td></tr>\n");
            replacements.Add(new ReplaceText { Original = "MORSECODELOOKUP", Replacement = morsecodetable });
            file.WriteFile(path, replacements, outputFile);
        }

        private void WriteWiresManual(string path, ManualFileName file, ref List<ReplaceText> replacements, bool outputFile = true)
        {
            var wirecuttinginstructions = string.Empty;
            var wirerules = _ruleManager.WireRuleSet.RulesDictionary;

            foreach (var rules in wirerules)
            {
                var rule = new List<Rule>(rules.Value);

                wirecuttinginstructions += $"<tr><td><strong><em>{rules.Key} wires:</em></strong><br />\n";
                if (rule.Count == 1)
                {
                    wirecuttinginstructions += $"{rule[0].GetSolutionString()}.\n";
                }
                else
                {
                    wirecuttinginstructions += $"If {rule[0].GetQueryString()}, {rule[0].GetSolutionString()}.\n";
                    for (var i = 1; i < rule.Count - 1; i++)
                    {
                        wirecuttinginstructions += $"<br />Otherwise, If {rule[i].GetQueryString()}, {rule[i].GetSolutionString()}.\n";
                    }
                    wirecuttinginstructions += $"<br />Otherwise, {rule.Last().GetSolutionString()}.\n";
                }
            }


            replacements.Add(new ReplaceText { Original = "WIRECUTTINGINSTRUCTIONS", Replacement = wirecuttinginstructions });
            file.WriteFile(path, replacements, outputFile);
        }

        private void WriteMemoryManual(string path, ManualFileName file, ref List<ReplaceText> replacements, bool outputFile = true)
        {
            var memoryinstructions = string.Empty;

            foreach (var stage in _ruleManager.MemoryRuleSet.RulesDictionary)
            {
                memoryinstructions += $"                        <h4>Stage {stage.Key + 1}:</h4><p>";
                for (var i = 0; i < stage.Value.Count; i++)
                {
                    memoryinstructions += $"If {stage.Value[i].GetQueryString()}, {stage.Value[i].GetSolutionString()}.<br />";
                }
                memoryinstructions += "</p>\n";
            }

            replacements.Add(new ReplaceText { Original = "MEMORYRULES", Replacement = memoryinstructions });
            file.WriteFile(path, replacements, outputFile);
        }

        private void WriteButtonManual(string path, ManualFileName file, ref List<ReplaceText> replacements, bool outputFile = true)
        {
            var initial = string.Empty;
            var onhold = string.Empty;
            var portsused = false;

            foreach (var press in _ruleManager.ButtonRuleSet.RuleList)
            {
                portsused = press.Queries.Aggregate(portsused, (current, query) => current | query.Property == QueryablePorts.EmptyPortPlate);
                portsused = press.Queries.Aggregate(portsused, (current, query) => current | QueryablePorts.PortList.Contains(query.Property));
            }

            initial = _ruleManager.ButtonRuleSet.RuleList.Aggregate(initial, (current, press) => current + $"<li>If {press.GetQueryString()}, {press.GetSolutionString()}.</li>\n");
            onhold = _ruleManager.ButtonRuleSet.HoldRuleList.Aggregate(onhold, (current, hold) => current + $"<li><em>{hold.GetQueryString()}</em> {hold.GetSolutionString()}</li>\n");

            replacements.Add(new ReplaceText { Original = "APPENDIXCREFERENCE", Replacement = portsused ? "<br />See Appendix C for port identification reference." : "" });
            replacements.Add(new ReplaceText { Original = "INITIALBUTTONRULES", Replacement = initial });
            replacements.Add(new ReplaceText { Original = "ONBUTTONHOLDRULES", Replacement = onhold });
            file.WriteFile(path, replacements, outputFile);
        }

        private void WriteHTML(string path, ManualFileName file, ref List<ReplaceText> replacements, bool outputFile=true)
        {
            if (string.IsNullOrEmpty(file.Name))
                return;
            switch (file.Name)
            {
                case "The Button.html":
                    WriteButtonManual(path, file, ref replacements, outputFile);
                    break;
                case "Memory.html":
                    WriteMemoryManual(path, file, ref replacements, outputFile);
                    break;
                case "Wires.html":
                    WriteWiresManual(path, file, ref replacements, outputFile);
                    break;
                case "Wire Sequence.html":
                    WriteWireSequenceManual(path, file, ref replacements, outputFile);
                    break;
                case "Morse Code.html":
                    WriteMorseCodeManaul(path, file, ref replacements, outputFile);
                    break;
                case "Who’s on First.html":
                    WriteWhosOnFirstManual(path, file, ref replacements, outputFile);
                    break;
                case "Knob.html":
                    WriteNeedyKnobManual(path, file, ref replacements, outputFile);
                    break;
                case "Password.html":
                    WritePasswordManual(path, file, ref replacements, outputFile);
                    break;
                case "Simon Says.html":
                    WriteSimonSaysManual(path, file, ref replacements, outputFile);
                    break;
                case "Keypad.html":
                    WriteKeypadsManual(path, file, ref replacements, outputFile);
                    break;
                case "Maze.html":
                    WriteMazesManual(path, file, ref replacements, outputFile);
                    break;
                case "Complicated Wires.html":
                    WriteComplicatedWiresManual(path, file, ref replacements, outputFile);
                    break;
                case "index.html":
                    file.WriteFile(path, replacements, outputFile);
                    break;
                default:
                    file.WriteFile(path, outputFile);
                    break;
            }
        }

        private RuleManager _ruleManager => RuleManager.Instance;

        private static readonly List<int> PreviousSeeds = new List<int>();

        public static string GetManualPath(int seed)
        {
	        return string.IsNullOrEmpty(ManualCacheDirectory) 
				? null 
				: Path.Combine(ManualCacheDirectory, seed.ToString());
        }

        private bool InitializeManaulWriting(int seed, out List<ReplaceText> replacements, out string path, bool forceRewriteSeed=false)
        {
            if (!forceRewriteSeed)
            {
                if (PreviousSeeds.Contains(seed))
                {
                    if (seed != 1)
                        DebugLog($"Manual already written for seed #{seed}.");
                    replacements = null;
	                path = null;
                    return false; //Seed 1 is the Original Vanilla seed.
                }
                PreviousSeeds.Add(seed);
            }

            _ruleManager.Initialize(seed);

            path = GetManualPath(seed);
            //if (Directory.Exists(path))
            //    return;

            DebugLog($"Printing the Rules for seed #{seed}");
            _ruleManager.CurrentRules.PrintRules();

            if (_manualFileNames == null)
            {
                DebugLog("Can't write any manuals :(");
                replacements = null;
                return false;
            }

            replacements = new List<ReplaceText>
            {
                new ReplaceText {Original = "VANILLAMODIFICATIONSEED", Replacement = seed.ToString()},
                new ReplaceText {Original = "<span class=\"page-header-doc-title\">Keep Talking and Nobody Explodes v. 1</span>", Replacement = $"<span class=\"page-header-doc-title\">Keep Talking and Nobody Explodes - Seed #{seed}</span>"},
                new ReplaceText {Original = "<span class=\"page-header-doc-title\">Keep Talking and Nobody Explodes</span>", Replacement = $"<span class=\"page-header-doc-title\">Keep Talking and Nobody Explodes - Seed #{seed}</span>"}
            };

            return true;
        }

        public string GetHTMLManual(int seed, HTMLManualNames name)
        {
            if (name < 0 || name > HTMLManualNames.Index)
                return null;
			if (!InitializeManaulWriting(seed, out List<ReplaceText> replacements, out string _, true))
				return null;
			for (var i = name == HTMLManualNames.Index ? 0 : (int)name; i <= (int) name; i++)
            {
                WriteHTML(null, _manualFileNames[i], ref replacements, false);
            }
            return _manualFileNames[(int) name].ToString(replacements);
        }

        public string GetHTMLManual(int seed, string name, bool write=false)
        {
	        if (string.IsNullOrEmpty(name)) return string.Empty;
	        if (write && !string.IsNullOrEmpty(GetManualPath(seed)) && File.Exists(Path.Combine(GetManualPath(seed), name)))
	        {
		        try
		        {
					return File.ReadAllText(Path.Combine(GetManualPath(seed), name));
		        }
		        catch
		        {
		        }
	        }

	        LoadModAssemblies();
			if (!HTMLManualGenerators.TryGetValue(name, out object generator))
				return string.Empty;

	        string manual;
			switch (generator)
			{
				case HTMLManualNames names:
					manual = GetHTMLManual(seed, names);
					break;
				case ModRuleGenerator modRuleGenerator:
					manual = modRuleGenerator.GetHTML(seed);
					break;
				default:
					return string.Empty;
			}
	        if (write && !string.IsNullOrEmpty(GetManualPath(seed)))
	        {
		        try
		        {
			        if (!Directory.Exists(GetManualPath(seed)))
				        Directory.CreateDirectory(GetManualPath(seed));
			        File.WriteAllText(Path.Combine(GetManualPath(seed), name), manual);
		        }
		        catch
		        {
		        }
	        }
	        return manual;
        }

	    public string[] GetHTMLFileNames()
	    {
		    LoadModAssemblies();
			return HTMLManualGenerators.Select(x => x.Key).ToArray();
	    }

	    public string[] GetVanillaHTMLFileNames()
	    {
		    return HTMLManualGenerators.Where(x => x.Value is HTMLManualNames htmlManualName && htmlManualName != HTMLManualNames.Index).Select(x => x.Key).OrderBy(x => x).ToArray();
	    }

	    public string[] GetModHTMLFileNames()
	    {
		    LoadModAssemblies();
		    return HTMLManualGenerators.Where(x => x.Value is ModRuleGenerator).Select(x => x.Key).OrderBy(x => x).ToArray();
	    }


        public void WriteIndexManaul(int seed)
        {
			InitializeManaulWriting(seed, out List<ReplaceText> replacements, out string path);
			if (string.IsNullOrEmpty(path))
                return;
            foreach (var manual in _manualFileNames)
            {
                WriteHTML(path, manual, ref replacements, !manual.Name.EndsWith(".html") || manual.Name.Equals("index.html") );
            }
        }

        public void WriteManual(int seed)
        {
			InitializeManaulWriting(seed, out List<ReplaceText> replacements, out string path);
			if (string.IsNullOrEmpty(path))
                return;

            foreach (var manual in _manualFileNames)
            {
                WriteHTML(path, manual, ref replacements);
            }
        }

        public void WriteModManuals(int seed, bool writeSupportFiles=true)
        {
            var path = GetManualPath(seed);
            var wroteMod = false;
            foreach (var kvp in HTMLManualGenerators.Where(x => x.Value is ModRuleGenerator))
            {
                var modRuleGenerator = kvp.Value as ModRuleGenerator;
                if (modRuleGenerator == null) continue;
                wroteMod = true;
                modRuleGenerator.WriteAllFiles(seed, path);
            }
            if (!wroteMod || !writeSupportFiles) return;

            foreach (var manual in _manualFileNames.Where(x => !HTMLManualGenerators.ContainsKey(x.Name)))
            {
                manual.WriteFile(path, true);
            }
        }

        public void WriteAllManuals(int seed)
        {
            WriteManual(seed);
            WriteModManuals(seed, false);
        }
    }

    public enum HTMLManualNames
    {
        CapacitorDischarge,
        ComplicatedWires,
        Keypads,
        Knobs,
        Mazes,
        Memory,
        MorseCode,
        Passwords,
        SimonSays,
        TheButton,
        VentingGas,
        WhosOnFirst,
        WireSequences,
        Wires,
        Index
    }
}