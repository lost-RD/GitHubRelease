using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CommandLine;

namespace GitHubRelease
{
	public class ProgramArguments
	{
		public class ListReleasesSubOptions
		{
			[Option('o', "owner", Required = true, HelpText = "Name of the owner of the repository.")]
			public bool Owner { get; set; }

			[Option('r', "repo", Required = true, HelpText = "Name of the repository.")]
			public bool Repo { get; set; }
		}

		public class GetReleaseFromIdSubOptions
		{
			[Option('o', "owner", Required = true, HelpText = "Name of the owner of the repository.")]
			public bool Owner { get; set; }

			[Option('r', "repo", Required = true, HelpText = "Name of the repository.")]
			public bool Repo { get; set; }

			[Option("id", HelpText = "The ID of the release. Looks like this: 5100383.")]
			public bool Id { get; set; }
		}

		public class GetReleaseFromTagSubOptions
		{
			[Option('o', "owner", Required = true, HelpText = "Name of the owner of the repository.")]
			public bool Owner { get; set; }

			[Option('r', "repo", Required = true, HelpText = "Name of the repository.")]
			public bool Repo { get; set; }

			[Option("tag", HelpText = "The tag of the release.")]
			public bool Tag { get; set; }
		}

		public class GetReleaseLatestSubOptions
		{
			[Option('o', "owner", Required = true, HelpText = "Name of the owner of the repository.")]
			public bool Owner { get; set; }

			[Option('r', "repo", Required = true, HelpText = "Name of the repository.")]
			public bool Repo { get; set; }
		}

		public class PostReleaseSubOptions
		{
			[Option('o', "owner", Required = true, HelpText = "Name of the owner of the repository.")]
			public bool Owner { get; set; }

			[Option('r', "repo", Required = true, HelpText = "Name of the repository.")]
			public bool Repo { get; set; }
		}

		public class EditReleaseSubOptions
		{
			[Option('o', "owner", Required = true, HelpText = "Name of the owner of the repository.")]
			public bool Owner { get; set; }

			[Option('r', "repo", Required = true, HelpText = "Name of the repository.")]
			public bool Repo { get; set; }

			[Option("id", HelpText = "The ID of the release. Looks like this: 5100383.")]
			public bool Id { get; set; }
		}

		public class DeleteReleaseSubOptions
		{
			[Option('o', "owner", Required = true, HelpText = "Name of the owner of the repository.")]
			public bool Owner { get; set; }

			[Option('r', "repo", Required = true, HelpText = "Name of the repository.")]
			public bool Repo { get; set; }

			[Option("id", HelpText = "The ID of the release. Looks like this: 5100383.")]
			public bool Id { get; set; }
		}

		public class ListReleaseAssetsSubOptions
		{
			[Option('o', "owner", Required = true, HelpText = "Name of the owner of the repository.")]
			public bool Owner { get; set; }

			[Option('r', "repo", Required = true, HelpText = "Name of the repository.")]
			public bool Repo { get; set; }

			[Option("id", HelpText = "The ID of the release. Looks like this: 5100383.")]
			public bool Id { get; set; }
		}

		public class GetSingleReleaseAssetSubOptions
		{
			[Option('o', "owner", Required = true, HelpText = "Name of the owner of the repository.")]
			public bool Owner { get; set; }

			[Option('r', "repo", Required = true, HelpText = "Name of the repository.")]
			public bool Repo { get; set; }

			[Option("id", HelpText = "The ID of the release. Looks like this: 5100383.")]
			public bool Id { get; set; }
		}

		public class UploadReleaseAssetSubOptions
		{
			[Option('o', "owner", Required = true, HelpText = "Name of the owner of the repository.")]
			public bool Owner { get; set; }

			[Option('r', "repo", Required = true, HelpText = "Name of the repository.")]
			public bool Repo { get; set; }

			[Option("id", HelpText = "The ID of the release. Looks like this: 5100383.")]
			public bool Id { get; set; }
		}

		public class EditReleaseAssetSubOptions
		{
			[Option('o', "owner", Required = true, HelpText = "Name of the owner of the repository.")]
			public bool Owner { get; set; }

			[Option('r', "repo", Required = true, HelpText = "Name of the repository.")]
			public bool Repo { get; set; }

			[Option("id", HelpText = "The ID of the release. Looks like this: 5100383.")]
			public bool Id { get; set; }
		}

		public class DeleteReleaseAssetSubOptions
		{
			[Option('o', "owner", Required = true, HelpText = "Name of the owner of the repository.")]
			public bool Owner { get; set; }

			[Option('r', "repo", Required = true, HelpText = "Name of the repository.")]
			public bool Repo { get; set; }

			[Option("id", HelpText = "The ID of the release. Looks like this: 5100383.")]
			public bool Id { get; set; }
		}

		public class Options
		{
			[ParserState]
			public IParserState LastParserState { get; set; }

			public Options()
			{
				GetReleaseFromIdVerb = new GetReleaseFromIdSubOptions();
				GetReleaseFromTagVerb = new GetReleaseFromTagSubOptions();
				EditReleaseVerb = new EditReleaseSubOptions();
				DeleteReleaseVerb = new DeleteReleaseSubOptions();
				ListReleaseAssetsVerb = new ListReleaseAssetsSubOptions();
				GetSingleReleaseAssetVerb = new GetSingleReleaseAssetSubOptions();
				UploadReleaseAssetVerb = new UploadReleaseAssetSubOptions();
				EditReleaseAssetVerb = new EditReleaseAssetSubOptions();
				DeleteReleaseAssetVerb = new DeleteReleaseAssetSubOptions();
			}

			[VerbOption("list", HelpText = "Get a list of releases for this repo.")]
			public ListReleasesSubOptions ListReleasesVerb { get; set; }

			[VerbOption("release-id", HelpText = "Get release from an ID.")]
			public GetReleaseFromIdSubOptions GetReleaseFromIdVerb { get; set; }

			[VerbOption("release-tag", HelpText = "Get release from a tag.")]
			public GetReleaseFromTagSubOptions GetReleaseFromTagVerb { get; set; }

			[VerbOption("latest", HelpText = "Get the latest release for this repo.")]
			public GetReleaseLatestSubOptions GetReleaseLatestVerb { get; set; }

			[VerbOption("release", HelpText = "Make a new release for this repo.")]
			public PostReleaseSubOptions PostReleaseVerb { get; set; }

			[VerbOption("edit", HelpText = "Record changes to the repository.")]
			public EditReleaseSubOptions EditReleaseVerb { get; set; }

			[VerbOption("delete", HelpText = "Delete a release using an ID.")]
			public DeleteReleaseSubOptions DeleteReleaseVerb { get; set; }

			[VerbOption("assets", HelpText = "Get a list of assets for this release.")]
			public ListReleaseAssetsSubOptions ListReleaseAssetsVerb { get; set; }

			[VerbOption("asset-id", HelpText = "Get an asset from an ID.")]
			public GetSingleReleaseAssetSubOptions GetSingleReleaseAssetVerb { get; set; }

			[VerbOption("upload-asset", HelpText = "Upload an asset to a release.")]
			public UploadReleaseAssetSubOptions UploadReleaseAssetVerb { get; set; }

			[VerbOption("edit-asset", HelpText = "Change name/label of an asset. ")]
			public EditReleaseAssetSubOptions EditReleaseAssetVerb { get; set; }

			[VerbOption("delete-asset", HelpText = "Delete an asset.")]
			public DeleteReleaseAssetSubOptions DeleteReleaseAssetVerb { get; set; }

			[HelpVerbOption]
			public string GetUsage(string verb)
			{
				bool found;
				var instance = (CommandLineOptionsBase) CommandLineParser.GetVerbOptionsInstanceByName(verb, this, out found);
				var verbsIndex = verb == null || !found;
				var target = verbsIndex ? this : instance;
				return HelpText.AutoBuild(target, current =&gt; HelpText.DefaultParsingErrorsHandler(target, current), verbsIndex);
			}
		}
	}
}
