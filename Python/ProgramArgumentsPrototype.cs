using CommandLine;

namespace GitHubRelease
{
	public class CommonOptions
	{
		[Option('o', "owner", Required = true, HelpText = "Name of the owner of the repository.")]
		public string Owner { get; set; }

		[Option('r', "repo", Required = true, HelpText = "Name of the repository.")]
		public string Repo { get; set; }
	}

	[Verb("list", HelpText = "Get a list of releases for this repo.")]
	public class ListReleasesOptions : CommonOptions
	{
	}

	[Verb("release-id", HelpText = "Get release from an ID.")]
	public class GetReleaseFromIdOptions : CommonOptions
	{
		[Option("id", HelpText = "The ID of the release. Looks like this: 5100383.")]
		public string Id { get; set; }
	}

	[Verb("release-tag", HelpText = "Get release from a tag.")]
	public class GetReleaseFromTagOptions : CommonOptions
	{
		[Option("tag", HelpText = "The tag of the release.")]
		public string Tag { get; set; }
	}

	[Verb("latest", HelpText = "Get the latest release for this repo.")]
	public class GetReleaseLatestOptions : CommonOptions
	{
	}

	[Verb("release", HelpText = "Make a new release for this repo.")]
	public class PostReleaseOptions : CommonOptions
	{
	}

	[Verb("edit", HelpText = "Record changes to the repository.")]
	public class EditReleaseOptions : CommonOptions
	{
		[Option("id", HelpText = "The ID of the release. Looks like this: 5100383.")]
		public string Id { get; set; }
	}

	[Verb("delete", HelpText = "Delete a release using an ID.")]
	public class DeleteReleaseOptions : CommonOptions
	{
		[Option("id", HelpText = "The ID of the release. Looks like this: 5100383.")]
		public string Id { get; set; }
	}

	[Verb("assets", HelpText = "Get a list of assets for this release.")]
	public class ListReleaseAssetsOptions : CommonOptions
	{
		[Option("id", HelpText = "The ID of the release. Looks like this: 5100383.")]
		public string Id { get; set; }
	}

	[Verb("asset-id", HelpText = "Get an asset from an ID.")]
	public class GetSingleReleaseAssetOptions : CommonOptions
	{
		[Option("id", HelpText = "The ID of the release. Looks like this: 5100383.")]
		public string Id { get; set; }
	}

	[Verb("upload-asset", HelpText = "Upload an asset to a release.")]
	public class UploadReleaseAssetOptions : CommonOptions
	{
		[Option("id", HelpText = "The ID of the release. Looks like this: 5100383.")]
		public string Id { get; set; }
	}

	[Verb("edit-asset", HelpText = "Change name/label of an asset. ")]
	public class EditReleaseAssetOptions : CommonOptions
	{
		[Option("id", HelpText = "The ID of the release. Looks like this: 5100383.")]
		public string Id { get; set; }
	}

	[Verb("delete-asset", HelpText = "Delete an asset.")]
	public class DeleteReleaseAssetOptions : CommonOptions
	{
		[Option("id", HelpText = "The ID of the release. Looks like this: 5100383.")]
		public string Id { get; set; }
	}

	[Verb("dummy", HelpText = "Dummy option")]
	public class DummyOptions : CommonOptions
	{
	}

}
		int Main(string[] args)
		{
			return Parser.Default.ParseArguments<ListReleasesOptions, GetReleaseFromIdOptions, GetReleaseFromTagOptions, GetReleaseLatestOptions, PostReleaseOptions, EditReleaseOptions, DeleteReleaseOptions, ListReleaseAssetsOptions, GetSingleReleaseAssetOptions, UploadReleaseAssetOptions, EditReleaseAssetOptions, DeleteReleaseAssetOptions, DummyOptions>(args)
			  .MapResult<ListReleasesOptions, GetReleaseFromIdOptions, GetReleaseFromTagOptions, GetReleaseLatestOptions, PostReleaseOptions, EditReleaseOptions, DeleteReleaseOptions, ListReleaseAssetsOptions, GetSingleReleaseAssetOptions, UploadReleaseAssetOptions, EditReleaseAssetOptions, DeleteReleaseAssetOptions, DummyOptions, int>(
					(ListReleasesOptions opts) => Verb_list(opts.Owner, opts.Repo),
					(GetReleaseFromIdOptions opts) => Verb_release_id(opts.Owner, opts.Repo, opts.Id),
					(GetReleaseFromTagOptions opts) => Verb_release_tag(opts.Owner, opts.Repo, opts.Tag),
					(GetReleaseLatestOptions opts) => Verb_latest(opts.Owner, opts.Repo),
					(PostReleaseOptions opts) => Verb_release(opts.Owner, opts.Repo),
					(EditReleaseOptions opts) => Verb_edit(opts.Owner, opts.Repo, opts.Id),
					(DeleteReleaseOptions opts) => Verb_delete(opts.Owner, opts.Repo, opts.Id),
					(ListReleaseAssetsOptions opts) => Verb_assets(opts.Owner, opts.Repo, opts.Id),
					(GetSingleReleaseAssetOptions opts) => Verb_asset_id(opts.Owner, opts.Repo, opts.Id),
					(UploadReleaseAssetOptions opts) => Verb_upload_asset(opts.Owner, opts.Repo, opts.Id),
					(EditReleaseAssetOptions opts) => Verb_edit_asset(opts.Owner, opts.Repo, opts.Id),
					(DeleteReleaseAssetOptions opts) => Verb_delete_asset(opts.Owner, opts.Repo, opts.Id),
					(DummyOptions opts) => Verb_dummy(opts.Owner, opts.Repo),
				errs => 1);
		}