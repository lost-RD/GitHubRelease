# GitHubRelease

## A command line tool for using the GitHub Release API

This project uses packages from Nuget including CommandLine 2.1.1-beta, Newtonsoft.Json 9.0.1 and RestSharp 105.2.3.

### Usage

`GitHubRelease help` for a list of verbs. The output looks like this:

```
GitHubRelease 1.0.0.0
Copyright ©  2017

  list            Get a list of releases for this repo.

  release-id      Get release from an ID.

  release-tag     Get release from a tag.

  latest          Get the latest release for this repo.

  release         Make a new release for this repo.

  edit            Record changes to the repository.

  delete          Delete a release using an ID.

  assets          Get a list of assets for this release.

  asset-id        Get an asset from an ID.

  upload-asset    Upload an asset to a release.

  edit-asset      Change name/label of an asset.

  delete-asset    Delete an asset.

  dummy           Dummy option

  help            Display more information on a specific command.

  version         Display version information.
  ```
  
To get usage information for a verb, simply use it like this `GitHubRelease assets` and the output will look like this:

```
GitHubRelease 1.0.0.0
Copyright ©  2017

ERROR(S):
  Required option 'o, owner' is missing.
  Required option 'r, repo' is missing.

  --id           The ID of the release. Looks like this: 5100383.

  -o, --owner    Required. Name of the owner of the repository.

  -r, --repo     Required. Name of the repository.

  --help         Display this help screen.

  --version      Display version information.
  ```
  
A valid use of assets would look like this: `GitHubRelease assets -o lost-rd -r GitHubRelease --id 5918561` and the output currently looks like this:

```JSON
[
  {
    "url": "https://api.github.com/repos/lost-RD/GitHubRelease/releases/assets/3521911",
    "id": 3521911,
    "name": "GitHubRelease.exe",
    "label": null,
    "uploader": {
      "login": "lost-RD",
	  ...
    },
    "content_type": "application/x-msdownload",
    "state": "uploaded",
    "size": 24576,
    "download_count": 0,
    "created_at": "2017-03-30T06:54:05Z",
    "updated_at": "2017-03-30T06:54:08Z",
    "browser_download_url": "https://github.com/lost-RD/GitHubRelease/releases/download/0.1-pre/GitHubRelease.exe"
  }
]
```

### Access Token

For actions that read information, no access token is necessary, but for actions that make changes you must provide an access token. Tokens can be obtained at [this address](https://github.com/settings/tokens). Ensure you tick the repo checkbox. Copy the access token into a file called `access.token` which must be in the same directory as the executable file (`GitHubRelease.exe`).