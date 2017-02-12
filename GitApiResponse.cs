using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Newtonsoft.Json;

namespace GitHubRelease
{
	public class GitApiResponse
	{
		[JsonProperty("id")]
		public string Id { get; set; }

		[JsonProperty("tag_name")]
		public string Tag_Name { get; set; }

		[JsonProperty("name")]
		public string Name { get; set; }
	}
}
