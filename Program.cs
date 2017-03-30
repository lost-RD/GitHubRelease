using System;
using System.Collections.Generic;
using RestSharp;
using Newtonsoft.Json;
using CommandLine;

namespace GitHubRelease
{
	class Program
	{
		public static string root = "https://api.github.com";
		public static string upload_url = "https://upload.github.com";
		public static string Alpha
		{
			get { return GetAlpha(); }
		}
		public static string access_token = System.IO.File.ReadAllText("access.token");

		static int Main(string[] args)
		{
			return Parser.Default.ParseArguments<ListReleasesOptions, GetReleaseFromIdOptions, GetReleaseFromTagOptions, GetReleaseLatestOptions, PostReleaseOptions, EditReleaseOptions, DeleteReleaseOptions, ListReleaseAssetsOptions, GetSingleReleaseAssetOptions, UploadReleaseAssetOptions, EditReleaseAssetOptions, DeleteReleaseAssetOptions, DummyOptions>(args)
			  .MapResult(
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
					(DummyOptions opts) => Dummy(opts.Owner, opts.Repo),
				errs => 1);
		}

		static string GetAlpha()
		{
			// Get the current alpha number
			string alpha = "Anull";
			alpha = System.IO.File.ReadAllText("alpha.txt");
			return alpha;
		}

		static IRestResponse ReleaseWithAsset(string owner, string repo, string comment)
		{
			// Make a release and upload an asset to it
			IRestResponse response = PostRelease(owner, repo, null, null, null, comment);
			int id = GetLatestReleaseId(owner, repo);
			IRestResponse response2 = UploadReleaseAsset(owner, repo, id.ToString());
			return response2;
		}

		static int GetLatestReleaseId(string owner, string repo)
		{
			// Get the ID of the latest release
			IRestResponse latest = GetReleaseLatest(owner, repo);
			Console.WriteLine(latest.StatusCode);
			ApiResponse json = JsonConvert.DeserializeObject<ApiResponse>(latest.Content.ToString());
			return json.Id;
		}

		static IRestResponse ListReleases(string owner, string repo)
		{
			// GET /repos/:owner/:repo/releases
			string client_path = root + "/repos/" + owner + "/" + repo + "/releases";
			var client = new RestClient(client_path);
			var request = new RestRequest(Method.GET);
			IRestResponse response = client.Execute(request);
			return response;
		}

		static int Verb_list(string owner, string repo)
		{
			//Get a list of releases for this repo
			IRestResponse response = ListReleases(owner, repo);
			List<ApiResponse> json = JsonConvert.DeserializeObject<List<ApiResponse>>(response.Content);
			foreach (ApiResponse release in json)
			{
				Console.WriteLine(String.Format("{0} | {1} | {2}", release.Id, release.Name, release.Body.Replace("\n", "")));
			}
			//Console.WriteLine(JsonConvert.SerializeObject(json, Formatting.Indented).ToString());
			return 1;
		}

		static IRestResponse GetReleaseFromId(string owner, string repo, string id)
		{
			// GET /repos/:owner/:repo/releases/:id
			string client_path = root + "/repos/" + owner + "/" + repo + "/releases/" + id;
			var client = new RestClient(client_path);
			var request = new RestRequest(Method.GET);
			IRestResponse response = client.Execute(request);
			return response;
		}

		static int Verb_release_id(string owner, string repo, string id)
		{
			//Get release from an ID
			IRestResponse response = GetReleaseFromId(owner, repo, id);
			ApiResponse json = JsonConvert.DeserializeObject<ApiResponse>(response.Content);
			Console.WriteLine(JsonConvert.SerializeObject(json, Formatting.Indented).ToString());
			return 1;
		}

		static IRestResponse GetReleaseFromTag(string owner, string repo, string tag)
		{
			// GET /repos/:owner/:repo/releases/tags/:tag
			string client_path = root + "/repos/" + owner + "/" + repo + "/releases/tags/" + tag;
			var client = new RestClient(client_path);
			var request = new RestRequest(Method.GET);
			IRestResponse response = client.Execute(request);
			return response;
		}

		static int Verb_release_tag(string owner, string repo, string tag)
		{
			//Get release from a tag
			IRestResponse response = GetReleaseFromTag(owner, repo, tag);
			ApiResponse json = JsonConvert.DeserializeObject<ApiResponse>(response.Content);
			Console.WriteLine(JsonConvert.SerializeObject(json, Formatting.Indented).ToString());
			return 1;
		}

		static IRestResponse GetReleaseLatest(string owner, string repo)
		{
			// GET /repos/:owner/:repo/releases/latest
			string client_path = root + "/repos/" + owner + "/" + repo + "/releases/latest";
			var client = new RestClient(client_path);
			var request = new RestRequest(Method.GET);
			IRestResponse response = client.Execute(request);
			return response;
		}

		static int Verb_latest(string owner, string repo)
		{
			//Get the latest release for this repo
			IRestResponse response = GetReleaseLatest(owner, repo);
			ApiResponse json = JsonConvert.DeserializeObject<ApiResponse>(response.Content);
			Console.WriteLine(JsonConvert.SerializeObject(json, Formatting.Indented).ToString());
			return 1;
		}

		static IRestResponse PostRelease(string owner, string repo, string tag_name="text_tag", string target_commitish="master", string name="Release ?", string body="test body please ignore", bool draft=false, bool prerelease=false)
		{
			// POST /repos/:owner/:repo/releases
			/* string tag_name, target_commitish, name, body
			 * bool draft, prerelease */
			string client_path = root + "/repos/" + owner + "/" + repo + "/releases?access_token="+access_token;
			var client = new RestClient(client_path);
			var request = new RestRequest(Method.POST);
			request.AddParameter("tag_name", tag_name, "application/json", ParameterType.RequestBody);
			request.AddParameter("target_commitish", target_commitish, "application/json", ParameterType.RequestBody);
			request.AddParameter("name", name, "application/json", ParameterType.RequestBody);
			request.AddParameter("body", body, "application/json", ParameterType.RequestBody);
			request.AddParameter("draft", draft, "application/json", ParameterType.RequestBody);
			request.AddParameter("prerelease", prerelease, "application/json", ParameterType.RequestBody);
			//Console.WriteLine(client_path);
			Console.WriteLine(JsonConvert.SerializeObject(request.Parameters, Formatting.Indented).ToString());
			IRestResponse response = client.Execute(request);
			return response;
		}

		static int Verb_release(string owner, string repo)
		{
			//Make a new release for this repo
			IRestResponse response = PostRelease(owner, repo);
			ApiResponse json = JsonConvert.DeserializeObject<ApiResponse>(response.Content);
			Console.WriteLine(JsonConvert.SerializeObject(json, Formatting.Indented).ToString());
			Console.WriteLine(response.StatusDescription+response.Content);
			return 1;
		}

		static IRestResponse EditRelease(string owner, string repo, string id)
		{
			// PATCH /repos/:owner/:repo/releases/:id
			/* string: tag_name, target_commitish, name, body
			 * bool draft, prerelease */
			string client_path = root + "/repos/" + owner + "/" + repo + "/releases/" + id + "?access_token=" + access_token;
			var client = new RestClient(client_path);
			var request = new RestRequest(Method.PATCH);
			request.AddParameter("tag_name", "test_tag", ParameterType.RequestBody);
			request.AddParameter("target_commitish", "master", ParameterType.RequestBody);
			request.AddParameter("name", "Release X (Alpha Y)", ParameterType.RequestBody);
			request.AddParameter("body", "test body please ignore", ParameterType.RequestBody);
			request.AddParameter("draft", false, ParameterType.RequestBody);
			request.AddParameter("prerelease", false, ParameterType.RequestBody);
			IRestResponse response = client.Execute(request);
			return response;
		}

		static int Verb_edit(string owner, string repo, string id)
		{
			//Record changes to the repository
			IRestResponse response = EditRelease(owner, repo, id);
			ApiResponse json = JsonConvert.DeserializeObject<ApiResponse>(response.Content);
			Console.WriteLine(JsonConvert.SerializeObject(json, Formatting.Indented).ToString());
			return 1;
		}

		static IRestResponse DeleteRelease(string owner, string repo, string id)
		{
			// DELETE /repos/:owner/:repo/releases/:id
			string client_path = root + "/repos/" + owner + "/" + repo + "/releases/" + id + "?access_token=" + access_token;
			var client = new RestClient(client_path);
			var request = new RestRequest(Method.DELETE);
			IRestResponse response = client.Execute(request);
			return response;
		}

		static int Verb_delete(string owner, string repo, string id)
		{
			//Record changes to the repository
			IRestResponse response = DeleteRelease(owner, repo, id);
			ApiResponse json = JsonConvert.DeserializeObject<ApiResponse>(response.Content);
			Console.WriteLine(JsonConvert.SerializeObject(json, Formatting.Indented).ToString());
			return 1;
		}

		static IRestResponse ListReleaseAssets(string owner, string repo, string id)
		{
			// GET /repos/:owner/:repo/releases/:id/assets
			string client_path = root + "/repos/" + owner + "/" + repo + "/releases/" + id + "/assets";
			Console.WriteLine(client_path);
			var client = new RestClient(client_path);
			var request = new RestRequest(Method.GET);
			IRestResponse response = client.Execute(request);
			return response;
		}

		static int Verb_assets(string owner, string repo, string id)
		{
			//Record changes to the repository
			IRestResponse response = ListReleaseAssets(owner, repo, id);
			ApiResponse json = JsonConvert.DeserializeObject<ApiResponse>(response.Content);
			Console.WriteLine(JsonConvert.SerializeObject(json, Formatting.Indented).ToString());
			return 1;
		}

		static IRestResponse GetSingleReleaseAsset(string owner, string repo, string id)
		{
			// GET /repos/:owner/:repo/releases/assets/:id
			string client_path = root + "/repos/" + owner + "/" + repo + "/releases/assets" + id;
			var client = new RestClient(client_path);
			var request = new RestRequest(Method.GET);
			IRestResponse response = client.Execute(request);
			return response;
		}

		static int Verb_asset_id(string owner, string repo, string id)
		{
			//Record changes to the repository
			IRestResponse response = GetSingleReleaseAsset(owner, repo, id);
			ApiResponse json = JsonConvert.DeserializeObject<ApiResponse>(response.Content);
			Console.WriteLine(JsonConvert.SerializeObject(json, Formatting.Indented).ToString());
			return 1;
		}

		static IRestResponse UploadReleaseAsset(string owner, string repo, string id)
		{
			// POST https://<upload_url>/repos/:owner/:repo/releases/:id/assets?name=foo.zip
			/* string: Content-Type, name, label
			 * Valid content types: https://www.iana.org/assignments/media-types/media-types.xhtml 
			 * We'll be going with application/zip here */
			string client_path = upload_url + "/repos/" + owner + "/" + repo + "/releases/" + id + "?access_token=" + access_token;
			var client = new RestClient(client_path);
			var request = new RestRequest(Method.PATCH);
			request.AddHeader("Content-Type", "application/zip");
			request.AddParameter("name", "Release X (Alpha Y)", ParameterType.RequestBody);
			request.AddParameter("label", "test label please ignore", ParameterType.RequestBody);
			IRestResponse response = client.Execute(request);
			return response;
		}

		static int Verb_upload_asset(string owner, string repo, string id)
		{
			//Record changes to the repository
			IRestResponse response = UploadReleaseAsset(owner, repo, id);
			ApiResponse json = JsonConvert.DeserializeObject<ApiResponse>(response.Content);
			Console.WriteLine(JsonConvert.SerializeObject(json, Formatting.Indented).ToString());
			return 1;
		}

		static IRestResponse EditReleaseAsset(string owner, string repo, string id)
		{
			// PATCH /repos/:owner/:repo/releases/assets/:id
			/* string: name, label */
			string client_path = root + "/repos/" + owner + "/" + repo + "/releases/assets" + id + "?access_token=" + access_token;
			var client = new RestClient(client_path);
			var request = new RestRequest(Method.PATCH);
			request.AddHeader("Content-Type", "application/zip");
			request.AddParameter("name", "Release X (Alpha Y)", ParameterType.RequestBody);
			request.AddParameter("label", "test label please ignore", ParameterType.RequestBody);
			IRestResponse response = client.Execute(request);
			return response;
		}

		static int Verb_edit_asset(string owner, string repo, string id)
		{
			//Record changes to the repository
			IRestResponse response = EditReleaseAsset(owner, repo, id);
			ApiResponse json = JsonConvert.DeserializeObject<ApiResponse>(response.Content);
			Console.WriteLine(JsonConvert.SerializeObject(json, Formatting.Indented).ToString());
			return 1;
		}

		static IRestResponse DeleteReleaseAsset(string owner, string repo, string id)
		{
			// DELETE /repos/:owner/:repo/releases/assets/:id
			string client_path = root + "/repos/" + owner + "/" + repo + "/releases/assets" + id + "?access_token=" + access_token;
			var client = new RestClient(client_path);
			var request = new RestRequest(Method.DELETE);
			IRestResponse response = client.Execute(request);
			return response;
		}

		static int Verb_delete_asset(string owner, string repo, string id)
		{
			//Record changes to the repository
			IRestResponse response = DeleteReleaseAsset(owner, repo, id);
			ApiResponse json = JsonConvert.DeserializeObject<ApiResponse>(response.Content);
			Console.WriteLine(JsonConvert.SerializeObject(json, Formatting.Indented).ToString());
			return 1;
		}

		static int Dummy(string owner, string repo)
		{
			Console.WriteLine(String.Format("{0}, {1}", owner, repo));
			return 1;
		}

	}

}
