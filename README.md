# Using this Template

1. Create a new Repository in GitHub based on this template
    - The name should be in the form `dbup-nameofthenewprovider`
    - It should be public
1. Create PR to add the name of the repo to the `AllProviders.txt` file in the `DbUp` repository
1. Clone it
1. Open it in VSCode or other light weight editor that doesn't have strong opinions about solution/project structure (i.e. not Rider/VS)
1. Rename the following:
    - `dbup-db2.sln`
    - `dbup-db2.sln.DotSettings`
    - `dbup-db2\dbup-db2.csproj`
    - `dbup-db2` directory
1. Search for `Db2` and replace with the new provider's name, **turning on the preserve case option**
1. Run `dotnet build` to ensure it builds
1. Uncomment the `push` and `pull_request` lines in `.github\workflows\main.yml`
1. Delete these instructions up to and including the next line, then check in

[![GitHub Workflow Status (branch)](https://img.shields.io/github/actions/workflow/status/DbUp/dbup-db2/main.yml?branch=main)](https://github.com/DbUp/dbup-db2/actions/workflows/main.yml?query=branch%3Amain)
[![NuGet](https://img.shields.io/nuget/dt/dbup-db2.svg)](https://www.nuget.org/packages/dbup-db2)
[![NuGet](https://img.shields.io/nuget/v/dbup-db2.svg)](https://www.nuget.org/packages/dbup-db2)
[![Prerelease](https://img.shields.io/nuget/vpre/dbup-db2?color=orange&label=prerelease)](https://www.nuget.org/packages/dbup-db2)

# DbUp Db2 support
DbUp is a .NET library that helps you to deploy changes to SQL Server databases. It tracks which SQL scripts have been run already, and runs the change scripts that are needed to get your database up to date.

## Getting Help
To learn more about DbUp check out the [documentation](https://dbup.readthedocs.io/en/latest/)

Please only log issue related to Db2 support in this repo. For cross cutting issues, please use our [main issue list](https://github.com/DbUp/DbUp/issues).

# Contributing

See the [readme in our main repo](https://github.com/DbUp/DbUp/blob/master/README.md) for how to get started and contribute.