using System;
using static VanillaRuleGenerator.Edgework.KMBombInfoExtensions;

namespace VanillaRuleGenerator.Edgework
{
	internal class JsonConvert
	{
		public static T DeserializeObject<T>(string query)
		{
			try
			{
				object data = Activator.CreateInstance<T>();
				switch (data)
				{
					case BatteryJSON battery:
						battery.numbatteries = int.Parse(query);
						break;
					case SerialNumberJSON serial:
						serial.serial = query;
						break;
					case IndicatorJSON indicator:
						indicator.label = query.Split(':')[0];
						indicator.@on = query.Split(':')[1];
						break;
					case ColorIndicatorJSON colorIndicator:
						colorIndicator.label = query.Split(':')[0];
						colorIndicator.label = query.Split(':')[1];
						break;
					case PortsJSON ports:
						ports.presentPorts = query.Split(new[] {","}, StringSplitOptions.RemoveEmptyEntries);
						break;

					default:
						return default(T);
				}
				return (T)data;
			}
			catch
			{
				return default(T);
			}
		}

		public static string SerializeObject(object T)
		{
			switch (T)
			{
				case BatteryJSON battery:
					return $"{battery.numbatteries}";
				case SerialNumberJSON serial:
					return $"{serial.serial}";
				case IndicatorJSON indicator:
					return $"{indicator.label}:{indicator.IsOn()}";
				case PortsJSON ports:
					return $"{string.Join(",", ports.presentPorts)}";
				case ColorIndicatorJSON colorIndicator:
					return $"{colorIndicator.label}:{colorIndicator.color}";
				default:
					return null;
			}
		}
	}
}