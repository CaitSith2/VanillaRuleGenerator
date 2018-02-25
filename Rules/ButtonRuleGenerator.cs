﻿using System;
using System.Collections.Generic;
using VanillaRuleGenerator.Modules;
using VanillaRuleGenerator.Rules.BombGame;

namespace VanillaRuleGenerator.Rules
{
    public class ButtonRuleGenerator : AbstractRuleSetGenerator
    {
        protected void RemoveRedundantRules()
        {
            if (CommonReflectedTypeInfo.IsVanillaSeed)
                return;

            var remainder = RuleSet.RuleList;
            if (remainder[remainder.Count - 2].Solution == ButtonSolutions.Hold)
                remainder[remainder.Count - 2].Solution = SelectSolution(CreateSolutionsList(false));

            var twobattery = -1;
            var onebattery = -1;
            var solution = string.Empty;
            var samesolution = true;

            for (var i = remainder.Count - 1; i >= 0; i--)
            {
                if (remainder[i].GetQueryString().Contains("more than 2 batteries"))
                {

                    twobattery = i;
                    if (onebattery > -1)
                    {
                        samesolution &= remainder[i].GetSolutionString().Equals(solution);
                        break;
                    }
                    solution = remainder[i].GetSolutionString();
                }
                else if (remainder[i].GetQueryString().Contains("more than 1 battery"))
                {
                    onebattery = i;
                    if (twobattery > -1)
                    {
                        samesolution &= remainder[i].GetSolutionString().Equals(solution);
                        break;
                    }
                    solution = remainder[i].GetSolutionString();
                }
                if (solution == String.Empty)
                    continue;
                samesolution &= remainder[i].GetSolutionString().Equals(solution);
            }
            if (onebattery == -1 || twobattery == -1)
            {
                return;
            }

            if (onebattery < twobattery && remainder[onebattery].Queries.Count == 1)
            {
                if (rand.Next(2) == 0)
                {
                    remainder[onebattery].Queries.Add(PopQueryFromList(SecondaryQueryList));
                }
                else
                {
                    remainder[onebattery].Queries[0] = PopQueryFromList(PrimaryQueryList);
                }
            }
            else if (remainder[onebattery].Queries.Count == 1 && remainder[twobattery].Queries.Count == 1 && samesolution)
            {
                switch (rand.Next(7))
                {
                    //Add a secondary query to one or both of the battery rules
                    case 0:
                        remainder[onebattery].Queries.Add(PopQueryFromList(SecondaryQueryList));
                        break;
                    case 1:
                        remainder[twobattery].Queries.Add(PopQueryFromList(SecondaryQueryList));
                        break;
                    case 2:
                        remainder[twobattery].Queries.Add(PopQueryFromList(SecondaryQueryList));
                        goto case 0;

                    //Replace one or both of the battery rules with a new primary query
                    case 3:
                        remainder[onebattery].Queries[0] = PopQueryFromList(PrimaryQueryList);
                        break;
                    case 4:
                        remainder[twobattery].Queries[0] = PopQueryFromList(PrimaryQueryList);
                        break;
                    case 5:
                        remainder[twobattery].Queries[0] = PopQueryFromList(PrimaryQueryList);
                        goto case 3;

                    //Replace one of the solutions in between the minimum and maximum battery.
                    default:
                        var replacementsolution = remainder[onebattery].Solution == ButtonSolutions.Press ? ButtonSolutions.Hold : ButtonSolutions.Press;
                        if (Math.Abs(onebattery - twobattery) == 1)
                            remainder[Math.Min(onebattery, twobattery)].Solution = replacementsolution;
                        else
                            remainder[rand.Next(Math.Min(onebattery, twobattery), Math.Max(onebattery, twobattery))].Solution = replacementsolution;
                        break;
                }
            }
        }

        public ButtonRuleSet GenerateButtonRules(int seed)
        {
            RuleSet = (ButtonRuleSet)GenerateRuleSet(seed);
            RemoveRedundantRules();
            return RuleSet;
        }

        protected override AbstractRuleSet CreateRules(bool useDefault)
        {
            
            return GenerateButtonRuleSet(useDefault);
        }

        protected ButtonRuleSet GenerateButtonRuleSet(bool useDefault = false)
        {
            BuildQueryLists();
            var buttonRuleSet = new ButtonRuleSet();
            while (buttonRuleSet.RuleList.Count < MaxInitialRules && PrimaryQueryList.Count > 0)
            {
                var baseQuery = PopQueryFromList(PrimaryQueryList);
                var num = rand.Next(2);
                if (num == 0)
                {
                    buttonRuleSet.RuleList.Add(CreateRule(baseQuery, null, false));
                }
                for (var i = 0; i < num; i++)
                {
                    buttonRuleSet.RuleList.Add(CreateRule(baseQuery, PopQueryFromList(SecondaryQueryList), false));
                }
            }
            var rule = new Rule();
            rule.Queries.Add(OtherwiseQuery());
            rule.Solution = ButtonSolutions.Hold;
            rule.SolutionArgs = new Dictionary<string, object>();
            buttonRuleSet.RuleList.Add(rule);
            while (buttonRuleSet.HoldRuleList.Count < MaxHoldRules && IndicatorColorQueryList.Count > 0)
            {
                var baseQuery2 = PopQueryFromList(IndicatorColorQueryList);
                buttonRuleSet.HoldRuleList.Add(CreateRule(baseQuery2, null, true));
            }
            buttonRuleSet.HoldRuleList.Add(CreateRule(IndicatorOtherwiseQuery(), null, true));
            RuleSet = buttonRuleSet;
            return buttonRuleSet;
        }

        protected Query PopQueryFromList(List<Query> queries)
        {
            var index = rand.Next(queries.Count);
            var query = queries[index];
            queries.Remove(query);
            return query;
        }

        protected Query OtherwiseQuery()
        {
            var args = new Dictionary<string, object>();
            return new Query
            {
                Property = QueryableButtonProperty.ButtonOtherwise,
                Args = args
            };
        }

        protected Query IndicatorOtherwiseQuery()
        {
            var args = new Dictionary<string, object>();
            return new Query
            {
                Property = QueryableButtonProperty.IndicatorOtherwise,
                Args = args
            };
        }

        protected void BuildQueryLists()
        {
            PrimaryQueryList = new List<Query>();
            IndicatorColorQueryList = new List<Query>();
            SecondaryQueryList = new List<Query>();
            SecondaryHoldQueryList = new List<Query>();
            for (var i = 0; i < Enum.GetNames(typeof(ButtonColor)).Length; i++)
            {
                var colorDictionary = new Dictionary<string, object> {{"color", (ButtonColor) i}};
                var colorQuery = new Query
                {
                    Property = QueryableButtonProperty.IsButtonColor,
                    Args = colorDictionary
                };
                PrimaryQueryList.Add(colorQuery);
            }
            for (var j = 0; j < Enum.GetNames(typeof(BigButtonLEDColor)).Length; j++)
            {
                var dictionary2 = new Dictionary<string, object> {{"color", (BigButtonLEDColor) j}};
                var item2 = new Query
                {
                    Property = QueryableButtonProperty.IsIndicatorColor,
                    Args = dictionary2
                };
                IndicatorColorQueryList.Add(item2);
            }
            for (var k = 0; k < Enum.GetNames(typeof(ButtonInstruction)).Length; k++)
            {
                var dictionary3 = new Dictionary<string, object> {{"instruction", (ButtonInstruction) k}};
                var item3 = new Query
                {
                    Property = QueryableButtonProperty.IsButtonInstruction,
                    Args = dictionary3
                };
                SecondaryQueryList.Add(item3);
                SecondaryHoldQueryList.Add(item3);
            }
            for (var l = 1; l < 3; l++)
            {
                var dictionary4 = new Dictionary<string, object> {{"batteryCount", l}};
                var item4 = new Query
                {
                    Property = QueryableProperty.MoreThanXBatteries,
                    Args = dictionary4
                };
                PrimaryQueryList.Add(item4);
            }
            for (var m = 0; m < 3; m++)
            {
                var list = new List<string>(IndicatorLabels);
                var text = list[rand.Next(list.Count)];
                list.Remove(text);
                var dictionary5 = new Dictionary<string, object> {{"label", text}};
                var item5 = new Query
                {
                    Property = QueryableProperty.IndicatorXLit,
                    Args = dictionary5
                };
                SecondaryQueryList.Add(item5);
            }
            if (CommonReflectedTypeInfo.IsModdedSeed)
            {
                var list = new List<QueryableProperty>(QueryablePorts.PortList);
                for (var i = 0; i < 3; i++)
                {
                    var port = list[rand.Next(list.Count)];
                    list.Remove(port);
                    var args = new Dictionary<string, object>();
                    var portQuery = new Query
                    {
                        Property = port,
                        Args = args
                    };
                    SecondaryQueryList.Add(portQuery);
                }
                foreach (var port in list)
                {
                    var args = new Dictionary<string, object>();
                    var portQuery = new Query
                    {
                        Property = port,
                        Args = args
                    };
                    PrimaryQueryList.Add(portQuery);
                }
                PrimaryQueryList.Add(new Query {Property = QueryablePorts.EmptyPortPlate, Args = new Dictionary<string, object>()});
                SecondaryQueryList.Add(new Query {Property = QueryableProperty.DoesSerialNumberStartWithLetter, Args = new Dictionary<string, object>()});
                SecondaryQueryList.Add(new Query() {Property = QueryableProperty.IsSerialNumberOdd, Args = new Dictionary<string, object>()});
            }
        }

        protected Rule CreateRule(Query baseQuery, Query secondaryQuery, bool isHoldRule)
        {
            var rule = new Rule();
            rule.Queries.Add(baseQuery);
            if (secondaryQuery != null)
            {
                rule.Queries.Add(secondaryQuery);
            }
            var solutionArgs = new Dictionary<string, object>();
            rule.Solution = SelectSolution(!isHoldRule ? CreateSolutionsList() : CreateHoldSolutionsList());
            rule.SolutionArgs = solutionArgs;
            return rule;
        }

        protected List<Solution> CreateSolutionsList(bool holdAllowed = true)
        {
            var list = new List<Solution> {ButtonSolutions.Hold, ButtonSolutions.Press};
            if (!holdAllowed)
                list.Remove(ButtonSolutions.Hold);
            if (CommonReflectedTypeInfo.IsModdedSeed)
            {
                list.Add(TapWhenSecondsMatch);
                if (!solutionWeights.ContainsKey(TapWhenSecondsMatch))
                {
                    solutionWeights.Add(TapWhenSecondsMatch, 0.05f);
                }
            }
            if (!solutionWeights.ContainsKey(ButtonSolutions.Press))
            {
                solutionWeights.Add(ButtonSolutions.Press, 0.1f);
            }
            foreach (var key in list)
            {
                if (!solutionWeights.ContainsKey(key))
                {
                    solutionWeights.Add(key, 1f);
                }
            }
            return list;
        }

        protected List<Solution> CreateHoldSolutionsList()
        {
            var list = new List<Solution>
            {
                ButtonSolutions.ReleaseOnTimerText("5"),
                ButtonSolutions.ReleaseOnTimerText("1"),
                ButtonSolutions.ReleaseOnTimerText("2"),
                ButtonSolutions.ReleaseOnTimerText("3"),
                ButtonSolutions.ReleaseOnTimerText("4")
            };
            if (CommonReflectedTypeInfo.IsModdedSeed)
            {
                list.AddRange(new []
                {
                    ButtonSolutions.ReleaseOnTimerText("6"),
                    ButtonSolutions.ReleaseOnTimerText("7"),
                    ButtonSolutions.ReleaseOnTimerText("8"),
                    ButtonSolutions.ReleaseOnTimerText("9"),
                    ButtonSolutions.ReleaseOnTimerText("0")
                });
                for (var i = 0; i < 10; i++)
                {
                    list.Add(ReleaseWhenLeastSignificantSecondIs(i));
                }
                list.Add(ReleaseAtAnyTime);
                list.Add(ReleaseOneSecondAfterSecondsAddToMultipleOfFour);
                list.Add(ReleaseWhenSecondsDigitsAddsToFive);
                list.Add(ReleaseWhenSecondsDigitsAddsToSeven);
                list.Add(ReleaseWhenSecondsDigitsAddsToThreeOrThirteen);
                list.Add(ReleaseWhenSecondsIsMultipleOfSeven);
                list.Add(ReleaseWhenSecondsPrimeOrZero);
            }
            foreach (var key in list)
            {
                if (!solutionWeights.ContainsKey(key))
                {
                    solutionWeights.Add(key, 1f);
                }
            }
            return list;
        }

        protected static List<string> IndicatorLabels = new List<string>
        {
            "SND",
            "CLR",
            "CAR",
            "IND",
            "FRQ",
            "SIG",
            "NSA",
            "MSA",
            "TRN",
            "BOB",
            "FRK"
        };

        public static Solution ReleaseWhenSecondsDigitsAddsToSeven = new Solution
        {
            Text = "release when the two seconds digits add up to 7.",
            SolutionMethod = (BombComponent comp, Dictionary<string, object> args) => 10
        };

        public static Solution ReleaseWhenSecondsDigitsAddsToThreeOrThirteen = new Solution
        {
            Text = "release when the two seconds digits add up to 3 or 13.",
            SolutionMethod = (BombComponent comp, Dictionary<string, object> args) => 11
        };

        public static Solution ReleaseWhenSecondsDigitsAddsToFive = new Solution
        {
            Text = "release when the two seconds digits add up to 5.",
            SolutionMethod = (BombComponent comp, Dictionary<string, object> args) => 12
        };

        public static Solution ReleaseWhenSecondsIsMultipleOfSeven = new Solution
        {
            Text = "release when the number of seconds remaining is a multiple of 7.",
            SolutionMethod = (BombComponent comp, Dictionary<string, object> args) => 13
        };

        public static Solution ReleaseWhenSecondsPrimeOrZero = new Solution
        {
            Text = "release when the number of seconds displayed is either prime or 0.",
            SolutionMethod = (BombComponent comp, Dictionary<string, object> args) => 14
        };

        public static Solution ReleaseOneSecondAfterSecondsAddToMultipleOfFour = new Solution
        {
            Text = "release one second after the two seconds digits add up to a multiple of 4.",
            SolutionMethod = (BombComponent comp, Dictionary<string, object> args) => 15
        };

        public static Solution ReleaseWhenLeastSignificantSecondIs(int seconds)
        {
            return new Solution
            {
                Text = $"release when right most seconds digit is {seconds}.",
                SolutionMethod = (BombComponent comp, Dictionary<string, object> args) => (16 + seconds)
            };
        }

        public static Solution ReleaseAtAnyTime = new Solution
        {
            Text = "release at any time.",
            SolutionMethod = (BombComponent comp, Dictionary<string, object> args) => 27
        };

        public static Solution TapWhenSecondsMatch = new Solution()
        {
            Text = "press and immediately release when the two seconds digits on the timer match.",
            SolutionMethod = (BombComponent comp, Dictionary<string, object> args) => 2
        };

        protected List<Query> PrimaryQueryList;
        protected List<Query> SecondaryQueryList;
        protected List<Query> IndicatorColorQueryList;
        protected List<Query> SecondaryHoldQueryList;
        private const int MaxInitialRules = 6;
        private const int MaxHoldRules = 3;
        protected ButtonRuleSet RuleSet;
    }
}