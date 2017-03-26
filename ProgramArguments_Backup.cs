using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CommandLine;
using CommandLine.Text;

namespace GitHubRelease
{
	public class CommonSubOptions
	{
		[Option('o', "owner", Required = true, HelpText = "Name of the owner of the repository.")]
		public string Owner { get; set; }

		[Option('r', "repo", Required = true, HelpText = "Name of the repository.")]
		public string Repo { get; set; }
	}

	[Verb("list", HelpText = "Get a list of releases for this repo")]
	public class ListReleasesOptions : CommonSubOptions
	{
		
	}
}
