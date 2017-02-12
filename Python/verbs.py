import csv

data = []

with open("..\ProgramArguments.cs", mode="w") as outfile:
	outfile.write("""using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CommandLine;

namespace GitHubRelease
{
	public class ProgramArguments
	{
""")

	with open('Verbs.csv', newline='') as csvfile:
		reader = csv.DictReader(csvfile)
		for row in reader:
			verb = row['verb']
			method = row['method']
			helptext = row['helptext']
			arg = row['arg']
			data.append(row)
			print(verb, method, helptext, ":".join("{:02x}".format(ord(c)) for c in arg))
			outfile.write("""		public class {0}SubOptions
		<
			[Option('o', "owner", Required = true, HelpText = "Name of the owner of the repository.")]
			public bool Owner < get; set; >

			[Option('r', "repo", Required = true, HelpText = "Name of the repository.")]
			public bool Repo < get; set; >
""".format(method).replace("<", "{").replace(">", "}"))
			if arg:
				if arg=="Id":
					arghelp = "The ID of the release. Looks like this: 5100383."
				elif arg=="Tag":
					arghelp = "The tag of the release."
				else:
					arghelp = "Sample helptext (unknown arg)"
				outfile.write("""
			[Option("{1}", HelpText = "{2}")]
			public bool {3} < get; set; >
""".format(method, arg.lower(), arghelp, arg).replace("<", "{").replace(">", "}"))
			outfile.write("""		>

""".replace(">", "}"))
				
			
		outfile.write("""		public class Options
		{
			[ParserState]
			public IParserState LastParserState { get; set; }

			public Options()
			{
""")
		
		for row in data:
			verb = row['verb']
			method = row['method']
			helptext = row['helptext']
			arg = row['arg']
			#print(verb, method, helptext, ":".join("{:02x}".format(ord(c)) for c in arg))
			if arg:
				outfile.write("""				{0}Verb = new {0}SubOptions();
""".format(method))
				
		outfile.write("""			}

""")
		
		for row in data:
			verb = row['verb']
			method = row['method']
			helptext = row['helptext']
			arg = row['arg']
			outfile.write("""			[VerbOption("{verb}", HelpText = "{helptext}")]
			public {method}SubOptions {method}Verb < get; set; >

""".format(verb=verb, helptext=helptext, method=method).replace("<", "{").replace(">", "}"))

		outfile.write("""			[HelpVerbOption]
			public string GetUsage(string verb)
			<
				bool found;
				var instance = (CommandLineOptionsBase) CommandLineParser.GetVerbOptionsInstanceByName(verb, this, out found);
				var verbsIndex = verb == null || !found;
				var target = verbsIndex ? this : instance;
				return HelpText.AutoBuild(target, current =&gt; HelpText.DefaultParsingErrorsHandler(target, current), verbsIndex);
			>
		>
	>
>
""".replace("<", "{").replace(">", "}"))

		for row in data:
			
"""		static void Main(string[] args)
		{
			return Parser.Default.ParseArguments<ProgramArguments.GetReleaseFromIdSubOptions, >(args)
			  .MapResult(
				(AddOptions opts) => RunAddAndReturnExitCode(opts),
				(CommitOptions opts) => RunCommitAndReturnExitCode(opts),
				(CloneOptions opts) => RunCloneAndReturnExitCode(opts),
				errs => 1);
		}"""