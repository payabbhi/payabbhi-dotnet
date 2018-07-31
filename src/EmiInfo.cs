using System;
using System.Collections.Generic;
using System.Net;
using Newtonsoft.Json;

namespace Payabbhi
{
	public class EmiInfo : PayabbhiEntity
	{
		[JsonProperty("tenure")]
		public int Tenure { get; set; }

		[JsonProperty("interest_rate")]
		public int InterestRate { get; set; }

		[JsonProperty("provider")]
		public string Provider { get; set; }

		[JsonProperty("subvention")]
		public string Subvention { get; set; }
	}
}
