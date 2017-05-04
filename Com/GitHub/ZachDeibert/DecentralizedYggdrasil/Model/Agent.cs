﻿using System;
using System.Xml.Serialization;
using Newtonsoft.Json;

namespace Com.GitHub.ZachDeibert.DecentralizedYggdrasil.Model {
	public class Agent {
		public static readonly Agent Minecraft = new Agent("Minecraft", 1);
		public static readonly Agent Scrolls = new Agent("Scrolls", 1);

		[JsonProperty("name"), XmlElement("name")]
		public string Name;
		[JsonProperty("version"), XmlElement("version")]
		public int Version;

		public Agent() {
		}

		public Agent(string name, int version) {
			Name = name;
			Version = version;
		}
	}
}

