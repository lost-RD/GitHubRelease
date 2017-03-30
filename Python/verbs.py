import csv

data = []

with open("ProgramArgumentsPrototype.cs", mode="w") as outfile:
	outfile.write("""using CommandLine;

namespace GitHubRelease
{
	public class CommonOptions
	{
		[Option('o', "owner", Required = true, HelpText = "Name of the owner of the repository.")]
		public string Owner { get; set; }

		[Option('r', "repo", Required = true, HelpText = "Name of the repository.")]
		public string Repo { get; set; }
	}

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
			outfile.write("""	[Verb("{verb}", HelpText = "{helptext}")]
	public class {method}Options : CommonOptions
	<
""".format(verb=verb, helptext=helptext, method=method).replace("<", "{").replace(">", "}"))
			if arg:
				if arg=="Id":
					arghelp = "The ID of the release. Looks like this: 5100383."
				elif arg=="Tag":
					arghelp = "The tag of the release."
				else:
					arghelp = "Sample helptext (unknown arg)"
				outfile.write("""		[Option("{1}", HelpText = "{2}")]
		public string {3} < get; set; >
""".format(method, arg.lower(), arghelp, arg).replace("<", "{").replace(">", "}"))
			outfile.write("""	>

""".replace(">", "}"))
		outfile.write("""}""".replace(">", "}"))

		outfile.write("""
		static int Main(string[] args)
		{
			return Parser.Default.ParseArguments<""")
		options = ""
		for row in data:
			verb = row['verb']
			method = row['method']
			helptext = row['helptext']
			arg = row['arg']
			options = options+"{method}Options, ".format(method=method)

		outfile.write(options[:-2])
		
		outfile.write(""">(args)
			  .MapResult(
""".format(options=options))

		for row in data:
			verb = row['verb']
			method = row['method']
			helptext = row['helptext']
			arg = row['arg']
			outfile.write("""					({method}Options opts) => Verb_{verb}(opts.Owner, opts.Repo""".format(method=method, verb=verb).replace("-", "_"))
			if arg:
				if arg=="Id":
					outfile.write(", opts.Id")
				elif arg=="Tag":
					outfile.write(", opts.Tag")
			outfile.write("""),
""")

		outfile.write("""				errs => 1);
		}""")