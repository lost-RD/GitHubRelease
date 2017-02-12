using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using RestSharp;
using Newtonsoft.Json;
using CommandLine;
using CommandLine.Text;

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

		static void Main(string[] args)
		{
			// Obviously this isn't working code but I'm clearly misunderstanding it
			return Parser.Default.ParseArguments<ProgramArguments.GetReleaseFromIdSubOptions>(args)
			  .MapResult(
				(ProgramArguments.GetReleaseFromIdSubOptions opts) => GetLatestReleaseId(opts)
				errs => 1);
		}

		public class Application
		{
			static public void Run(ProgramArguments o)
			{
				string owner = o.Owner;
				string repo = o.Repo;
				Console.WriteLine(owner + ", " + repo);

				Console.WriteLine("Alpha: " + GetAlpha());
				Console.WriteLine("id of latest release: " + GetLatestReleaseId(owner, repo));
				//IRestResponse response = PostRelease("lost-RD", "FloorBeautyRebalance");
			}
		}

		static string GetAlpha()
		{
			string alpha = "Anull";
			alpha = System.IO.File.ReadAllText("alpha.txt");
			return alpha;
		}

		static IRestResponse ReleaseWithAsset(string owner, string repo, string comment)
		{
			IRestResponse response = PostRelease(owner, repo, null, null, null, comment);
			string id = GetLatestReleaseId(owner, repo);
			IRestResponse response2 = UploadReleaseAsset(owner, repo, id);
			return response2; 
		}

		static string GetLatestReleaseId(string owner, string repo)
		{
			IRestResponse latest = GetReleaseLatest(owner, repo);
			Console.WriteLine(latest.StatusCode);
			GitApiResponse json = JsonConvert.DeserializeObject<GitApiResponse>(latest.Content.ToString());
			string id = json.Id;
			return id;
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

		static IRestResponse GetReleaseFromId(string owner, string repo, string id)
		{
			// GET /repos/:owner/:repo/releases/:id
			string client_path = root + "/repos/" + owner + "/" + repo + "/releases" + id;
			var client = new RestClient(client_path);
			var request = new RestRequest(Method.GET);
			IRestResponse response = client.Execute(request);
			return response;
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

		static IRestResponse GetReleaseLatest(string owner, string repo)
		{
			// GET /repos/:owner/:repo/releases/latest
			string client_path = root + "/repos/" + owner + "/" + repo + "/releases/latest";
			var client = new RestClient(client_path);
			var request = new RestRequest(Method.GET);
			IRestResponse response = client.Execute(request);
			return response;
		}

		static IRestResponse PostRelease(string owner, string repo, string tag_name="text_tag", string target_commitish="master", string name="Release ?", string body="test body please ignore", bool draft=false, bool prerelease=false)
		{
			// POST /repos/:owner/:repo/releases
			/* string tag_name, target_commitish, name, body
			 * bool draft, prerelease */
			string client_path = root + "/repos/" + owner + "/" + repo + "/releases";
			var client = new RestClient(client_path);
			var request = new RestRequest(Method.POST);
			request.AddParameter("tag_name", tag_name, ParameterType.RequestBody);
			request.AddParameter("target_commitish", target_commitish, ParameterType.RequestBody);
			request.AddParameter("name", name, ParameterType.RequestBody);
			request.AddParameter("body", body, ParameterType.RequestBody);
			request.AddParameter("draft", draft, ParameterType.RequestBody);
			request.AddParameter("prerelease", prerelease, ParameterType.RequestBody);
			IRestResponse response = client.Execute(request);
			return response;
		}

		static IRestResponse EditRelease(string owner, string repo, string id)
		{
			// PATCH /repos/:owner/:repo/releases/:id
			/* string: tag_name, target_commitish, name, body
			 * bool draft, prerelease */
			string client_path = root + "/repos/" + owner + "/" + repo + "/releases/" + id;
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

		static IRestResponse DeleteRelease(string owner, string repo, string id)
		{
			// DELETE /repos/:owner/:repo/releases/:id
			string client_path = root + "/repos/" + owner + "/" + repo + "/releases/" + id;
			var client = new RestClient(client_path);
			var request = new RestRequest(Method.DELETE);
			IRestResponse response = client.Execute(request);
			return response;
		}

		static IRestResponse ListReleaseAssets(string owner, string repo, string id)
		{
			// GET /repos/:owner/:repo/releases/:id/assets
			string client_path = root + "/repos/" + owner + "/" + repo + "/releases/" + id + "/assets";
			var client = new RestClient(client_path);
			var request = new RestRequest(Method.GET);
			IRestResponse response = client.Execute(request);
			return response;
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

		static IRestResponse UploadReleaseAsset(string owner, string repo, string id)
		{
			// POST https://<upload_url>/repos/:owner/:repo/releases/:id/assets?name=foo.zip
			/* string: Content-Type, name, label
			 * Valid content types: https://www.iana.org/assignments/media-types/media-types.xhtml 
			 * We'll be going with application/zip here */
			string client_path = upload_url + "/repos/" + owner + "/" + repo + "/releases/" + id;
			var client = new RestClient(client_path);
			var request = new RestRequest(Method.PATCH);
			request.AddHeader("Content-Type", "application/zip");
			request.AddParameter("name", "Release X (Alpha Y)", ParameterType.RequestBody);
			request.AddParameter("label", "test label please ignore", ParameterType.RequestBody);
			IRestResponse response = client.Execute(request);
			return response;
		}

		static IRestResponse EditReleaseAsset(string owner, string repo, string id)
		{
			// PATCH /repos/:owner/:repo/releases/assets/:id
			/* string: name, label */
			string client_path = root + "/repos/" + owner + "/" + repo + "/releases/assets" + id;
			var client = new RestClient(client_path);
			var request = new RestRequest(Method.PATCH);
			request.AddHeader("Content-Type", "application/zip");
			request.AddParameter("name", "Release X (Alpha Y)", ParameterType.RequestBody);
			request.AddParameter("label", "test label please ignore", ParameterType.RequestBody);
			IRestResponse response = client.Execute(request);
			return response;
		}

		static IRestResponse DeleteReleaseAsset(string owner, string repo, string id)
		{
			// DELETE /repos/:owner/:repo/releases/assets/:id
			string client_path = root + "/repos/" + owner + "/" + repo + "/releases/assets" + id;
			var client = new RestClient(client_path);
			var request = new RestRequest(Method.DELETE);
			IRestResponse response = client.Execute(request);
			return response;
		}

	}

}
