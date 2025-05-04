<p align="center">
    <a href="https://github.com/Pulumock/Pulumock" title="Pulumock - A tool designed to address testing challenges in Pulumi .NET">
        <img src="pulumock-logo.png" width="150" alt="Project logo" />
    </a>
</p>

[![License](https://img.shields.io/github/license/Pulumock/Pulumock)](LICENSE)
[![Release](https://github.com/Pulumock/Pulumock/actions/workflows/release.yml/badge.svg)](https://github.com/Pulumock/Pulumock/actions/workflows/release.yml)
[![NuGet](https://img.shields.io/nuget/v/Pulumock?logo=nuget)](https://www.nuget.org/packages/Pulumock)
[![NuGet Pre-release](https://img.shields.io/nuget/vpre/Pulumock?logo=nuget)](https://www.nuget.org/packages/Pulumock)
[![NuGet downloads](https://img.shields.io/nuget/dt/Pulumock)](https://www.nuget.org/packages/Pulumock)
[![GitHub Discussions](https://img.shields.io/github/discussions/Pulumock/Pulumock)](https://github.com/orgs/Pulumock/discussions)
[![GitHub issues](https://img.shields.io/github/issues/Pulumock/Pulumock)](https://github.com/Pulumock/Pulumock/issues)

# Pulumock

Pulumock your stack before it bites back.

Pulumock is a testing tool for Pulumi .NET projects, designed to simplify and enhance unit testing. 
Built on top of the standard Pulumi .NET testing capabilities, it addresses common challenges reported by developers, 
based on real-world data from the [Pulumissues dataset](https://github.com/Pulumock/Pulumissues).

By using a fluent, strongly typed syntax, Pulumock abstracts away complexity and reduces boilerplate, letting you focus on test logic instead of setup. Pulumock also provides richer data for assertions, enabling more scenarios to be covered.

## Table of Contents

- [Getting Started](#getting-started)
- [Documentation](#documentation)
- [Contributing](#contributing)
- [License](#license)

## Getting Started

### Installation

Install the [Pulumock NuGet package](https://www.nuget.org/packages/Pulumock) in your testing project:
```shell
dotnet add package Pulumock
```

### Usage

Here’s a simple step-by-step example. For more in-depth instructions, see the [documentation](#documentation).

#### 1. Create a FixtureBuilder
Begin by instantiating a `FixtureBuilder` in your test method or wherever appropriate in your codebase:
```csharp
[Fact]
public async Task ExampleTest() 
{
    var fixtureBuilder = new FixtureBuilder();
}
```

#### 2. Apply Mocks
Use the builder pattern to add any required mocks to your fixture:
```csharp
[Fact]
public async Task ExampleTest() 
{
    var fixtureBuilder = new FixtureBuilder()
        .WithMockResource(new MockResourceBuilder<ResourceGroup>()
            .WithOutput(x => x.Location, "swedencentral")
            .Build()); 
}
```

#### 3. Build the Fixture & Write Assertions
Build the fixture to run your test setup, then use convenient extension methods to write your assertions:
```csharp
[Fact]
public async Task ExampleTest() 
{
    var fixtureBuilder = new FixtureBuilder()
        .WithMockResource(new MockResourceBuilder<ResourceGroup>()
            .WithOutput(x => x.Location, "swedencentral")
            .Build()); 
    
    var fixture = await fixtureBuilder
        .BuildAsync(async () => await CoreStack.DefineResourcesAsync()); // Your code that creates resources.
    
    var resourceGroup = fixture.StackResources.Require<ResourceGroup>();
    var location = await resourceGroup.Location.GetValueAsync();
    
    location.ShouldBe("swedencentral");
}
```

## Documentation

- Explore the [Wiki](https://github.com/Pulumock/Pulumock/wiki) for official documentation and straightforward usage guides.
- Check out the [Example project](./Source/Example/README.md) for a working demonstration of Pulumock in action across multiple scenarios.

## Contributing

See [CONTRIBUTING.md](CONTRIBUTING.md) for more information.

## License

Pulumock is licensed under the **MIT license**. Feel free to edit and distribute this project as you like.

See [LICENSE](LICENSE) for more information.
