using SCADA_CORE.Database;
using SCADA_CORE.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Web;
using System.Xml.Linq;

namespace SCADA_CORE
{
    public class TagProcessing
    {
		public static UserContext userdb = new UserContext();
		public static TagValueContext tagdb = new TagValueContext();
		public static AlarmsContext alarmdb = new AlarmsContext();

		private static readonly object locker = new object();

		static XElement xmlDataTags;
		static XElement xmlDataAlarms;

		static Dictionary<string, TagInfo> TagConfig;
		static Dictionary<string, double> TagCurrentValues;
		public static Dictionary<string, double> TagOutputValues;
		static Dictionary<string, Thread> TagProcessingThreads;

		static Dictionary<string, Alarm> AlarmConfing = new Dictionary<string, Alarm>();

		public static Dictionary<string, string> AddressKeys = new Dictionary<string, string>();
		public static Dictionary<string, double> AddressValues = new Dictionary<string, double>()
		{ { "0", -1 }, { "1", -1 }, { "2", -1 }, { "3", -1 }, { "4", -1 } };



		public static void Init()
		{
			//read tags
			xmlDataTags = File.Exists(@"C:\Config\scadaConfig.txt") ? XElement.Load(@"C:\Config\scadaConfig.txt") : new XElement("a");
			TagConfig =
				((from d in xmlDataTags.Descendants("analogTags")
				  let ad = d.Descendants("tag")
				  from at in ad
				  select new AITag
				  {
					  TagId = at.Value,
					  Description = at.Attribute("description").Value,
					  Driver = at.Attribute("driver").Value,
					  IOAddress = at.Attribute("ioAddress").Value,
					  ScanTime = int.Parse(at.Attribute("scanTime").Value),
					  OnScan = bool.Parse(at.Attribute("onScan").Value),
					  LowLimit = int.Parse(at.Attribute("lowLimit").Value),
					  HighLimit = int.Parse(at.Attribute("highLimit").Value),
					  Units = at.Attribute("units").Value
				  } as TagInfo).ToList().Union((from d in xmlDataTags.Descendants("digitalTags")
												let ad = d.Descendants("tag")
												from at in ad
												select new DITag
												{
													TagId = at.Value,
													Description = at.Attribute("description").Value,
													Driver = at.Attribute("driver").Value,
													IOAddress = at.Attribute("ioAddress").Value,
													ScanTime = int.Parse(at.Attribute("scanTime").Value),
													OnScan = bool.Parse(at.Attribute("onScan").Value)
												} as TagInfo))).ToDictionary(x => x.TagId, x => x);

			TagCurrentValues = TagConfig.ToDictionary(x => x.Key, x => -1000.0);
			TagOutputValues = new Dictionary<string, double>();
			TagProcessingThreads = TagConfig.ToDictionary(x => x.Key, x => { 
			Thread t = new Thread(() => ScanTag(x.Value.TagId)); 
										t.Start(); return t; 
				});

			// read alarms
			xmlDataAlarms = File.Exists(@"C:\Config\alarmConfig.txt") ? XElement.Load(@"C:\Config\alarmConfig.txt") : new XElement("b");
			AlarmConfing =
				((from a in xmlDataTags.Descendants("alarms")
				  let alarm = a.Descendants("alarm")
				  from al in alarm
				  select new Alarm
				  {
					  Type = al.Attribute("type").Value,
					  Priority = int.Parse(al.Attribute("priority").Value),
					  Time = DateTime.Parse(al.Attribute("time").Value),
					  Activated = bool.Parse(al.Attribute("activated").Value),
					  TagId = al.Value

				  })).ToList().ToDictionary(x => x.TagId, x => x);
		}

		public static string AddDigitalInputTag(string tagId, string description, string driver, string ioAddress, int scanTime, bool onScan)
		{
			if (!TagConfig.Keys.Contains(tagId))
			{
				DITag di = new DITag()
				{
					TagId = tagId,
					Description = description,
					Driver = driver,
					IOAddress = ioAddress,
					ScanTime = scanTime,
					OnScan = onScan
				};

				lock (locker)
				{
					TagConfig.Add(di.TagId, di);
					TagCurrentValues.Add(di.TagId, di.GetValue());

					tagdb.Tags.Add(new TagValueInfo(di.TagId, 555));
					tagdb.SaveChanges();

					TagProcessingThreads.Add(di.TagId, new Thread(() => ScanTag(di.TagId)));
					TagProcessingThreads[tagId].Start();
				}

				UpdateConfig();

				return "Tag added successfully";
			}

			return "Tag already exists";
		}

		public static string AddDigitalOutputTag(string tagId, string description, string ioAddress, int initValue)
		{
			
			if (!TagOutputValues.ContainsKey(tagId)){
				TagOutputValues.Add(tagId, initValue);

				tagdb.Tags.Add(new TagValueInfo(tagId, initValue));
				tagdb.SaveChanges();

				return "Successfully added analog output tag!";
			}
			return "Tag already exists";
		}

		public static string AddAnalogInputTag(string tagId, string description, string driver, string ioAddress, int scanTime, bool onScan, int lowLimit, int highLimit, string units)
		{
			if (!TagConfig.Keys.Contains(tagId))
			{
				AITag ai = new AITag()
				{
					TagId = tagId,
					Description = description,
					Driver = driver,
					IOAddress = ioAddress,
					ScanTime = scanTime,
					OnScan = onScan,
					LowLimit = lowLimit,
					HighLimit = highLimit,
					Units = units
				};

				lock (locker)
				{
					TagConfig.Add(ai.TagId, ai);
					TagCurrentValues.Add(ai.TagId, ai.GetValue());

					tagdb.Tags.Add(new TagValueInfo(ai.TagId, 999));
					tagdb.SaveChanges();

					TagProcessingThreads.Add(ai.TagId, new Thread(() => ScanTag(ai.TagId)));
					TagProcessingThreads[tagId].Start();
				}
				
				UpdateConfig();

				return "Tag added successfully";
			}
			return "Tag not found";
		}

		public static string AddAlarm(string tagId, string type, int priority)
		{
			//make an alarm in 30 seconds
			AlarmConfing.Add(tagId, new Alarm(type, priority, DateTime.Now.AddSeconds(30), tagId, false));
			return "Successfully added an alarm.";

		}

		public static string RemoveAlarm(string tagId)
        {
            throw new NotImplementedException();
        }


        public static string AddAnalogOutputTag(string tagId, string description, string ioAddress, int initValue, int lowlimit, int highLimit)
		{
            if (!TagOutputValues.ContainsKey(tagId)){
				TagOutputValues.Add(tagId, initValue);

				tagdb.Tags.Add(new TagValueInfo(tagId, initValue));
				tagdb.SaveChanges();

				return "Successfully added analog output tag!";
			}
			return "Tag already exists";
		}

        internal static string RemoveTag(string tagId)
        {
			if (TagConfig.Keys.Contains(tagId))
			{
				lock (locker)
				{
					TagProcessingThreads[tagId].Abort();
					TagConfig.Remove(tagId);
					TagCurrentValues.Remove(tagId);
					TagProcessingThreads.Remove(tagId);
				}

				UpdateConfig();

				return "Tag removed successfully";
			}
            if (TagOutputValues.Remove(tagId)){
				return "Output tag removed successfully";
			}
			return "Tag with specified id does not exist";
		}
		
		public static string SwitchScanMode(string tagId)
		{
			if (TagConfig.Keys.Contains(tagId))
			{
				lock (locker)
					TagConfig[tagId].OnScan = !TagConfig[tagId].OnScan;

				UpdateConfig();

				return "Tag mode switched successfully";
			}

			return "Tag with specified id does not exist";
		}

		public static string ChangeOutputValue(string tagId, double newValue)
		{
			if (TagOutputValues.Keys.Contains(tagId))
			{
				TagOutputValues[tagId] = newValue;

				return "Tag value changed successfully";
			}

			return "Tag with specified id does not exist";
		}

		public static void ScanTag(string tagId)
		{
			Tag tag = new Tag();

			while (true)
			{
				lock (locker)
				{
					tag.Info = TagConfig[tagId];
					tag.Value = TagCurrentValues[tagId];
				}

				double currentValue = tag.Info.GetValue();

				if (currentValue != tag.Value)
				{
					lock (locker)
					{
						TagCurrentValues[tag.Info.TagId] = currentValue;
					}

					if (tag.Info.OnScan)
					{
						TrendingService.TrendingService.FireEvent(tag.Info.TagId, currentValue);
					}

				
					// samo ako je analogni
					if (tag.Info is AITag)
					{
						AITag ai = (AITag)tag.Info;
						// aktiviraj alarm i upisi u bazu 
						if (AlarmConfing.ContainsKey(ai.TagId))
						{
							if (AlarmConfing[ai.TagId].Time > DateTime.Now)
							{
                                if (!AlarmConfing[ai.TagId].Activated) {
									AlarmConfing[ai.TagId].Activated = true;

									alarmdb.Alarms.Add(AlarmConfing[ai.TagId]);

									alarmdb.SaveChanges();

									AlarmDisplayService.AlarmDisplayService.Notify(ai.TagId, AlarmConfing[ai.TagId].Priority, AlarmConfing[ai.TagId].Time);
									
									UpdateAlarmConfig();
								}
							}
						}
						
					}
				}

				Thread.Sleep(tag.Info.ScanTime);
			}
		}

        private static void UpdateAlarmConfig()
        {
			if (!(Directory.Exists(@"C:\Config\")))
				Directory.CreateDirectory(@"C:\Config\");

			XElement configuration =
				new XElement("alarms",
					new XElement("alarm",
						from a in AlarmConfing.Values
						where a is Alarm
						where a.Time > DateTime.Now						
						select new XElement("tag", a.TagId,
							new XAttribute("priority", a.Priority),
							new XAttribute("time", a.Time),
							new XAttribute("type", a.Type)
							)));

			using (System.IO.StreamWriter sw = new System.IO.StreamWriter(@"C:\Config\alarmConfig.txt"))
			{
				sw.WriteLine(configuration);
			}
		}

        public static void UpdateConfig()
		{
			if (!(Directory.Exists(@"C:\Config\")))
				Directory.CreateDirectory(@"C:\Config\");

			XElement configuration =
				new XElement("tags",
					new XElement("analogTags",
						from t in TagConfig.Values
						where t is AITag
						let at = t as AITag
						select new XElement("tag", t.TagId,
							new XAttribute("description", t.Description),
							new XAttribute("driver", t.Driver),
							new XAttribute("ioAddress", t.IOAddress),
							new XAttribute("scanTime", t.ScanTime),
							new XAttribute("onScan", t.OnScan),
							new XAttribute("lowLimit", at.LowLimit),
							new XAttribute("highLimit", at.HighLimit),
							new XAttribute("units", at.Units))
							),
					new XElement("digitalTags",
						from t in TagConfig.Values
						where t is DITag
						select new XElement("tag", t.TagId,
							new XAttribute("description", t.Description),
							new XAttribute("driver", t.Driver),
							new XAttribute("ioAddress", t.IOAddress),
							new XAttribute("scanTime", t.ScanTime),
							new XAttribute("onScan", t.OnScan)
							)
						)
					);

			using (System.IO.StreamWriter sw = new System.IO.StreamWriter(@"C:\Config\scadaConfig.txt"))
			{
				sw.WriteLine(configuration);
			}
		}
	}
}