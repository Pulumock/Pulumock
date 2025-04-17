Create a GitHub Fine-grained access token with "Only select repositories (which includes public repos by default), and add Repository permissions with Issues set to read-only. And add to user secrets: `dotnet user-secrets set "GitHub:Token" "ghp_your_token_here"`


Run the program...

https://docs.github.com/en/rest/issues/issues?apiVersion=2022-11-28#list-repository-issues