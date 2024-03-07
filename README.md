# DENKApi.NET.Sample
Includes examples and instructions on using *DENKApi.NET*.

## Introduction
*DENKApi.NET* provides a wrapper around the native *DENKApi*. Based on the architecture of the *DENKApi*, the *DENKApi.NET* ships as several NuGet packages:

- *DENKApi.NET.Core* - A base package including types common to all flavors.
- *DENKApi.NET.CPU* - The *DENKApi* in CPU-flavor. This allows evaluation of images using DENKweit neural networks on CPU
- *DENKApi.NET.GPU* - The *DENKApi* in GPU-flavor (CUDA). This allows evaluation of images using DENKweit neural networks on GPU
- *DENKApi.NET.DML* - The *DENKApi* in DML-flavor. This allows evaluation of images using DENKweit neural networks on GPU via DML

All the packages above are independent of the operating system and .NET version. They are realised using .NET Standard 2.0 and thus can be used with .NET and .NET Framework (version 4.6.1 and higher).

The *DENKApi.NET.Core* package is used by the CPU/GPU-specific packages and cannot be used standalone. Each CPU/GPU-specific package depends on *DENKApi.NET.Core* and thus it is installed automatically with the CPU/GPU-specific packages.

Each CPU/GPU-specific package provides a specific `DENKApiFactory`. This factory provides access to an CPU/GPU-specific object implementing the `IDENKApi` interface. Thus a program can be easily switched from say *DENKApi.NET.CPU* to *DENKApi.NET.GPU*. The only change required for such a switch is to change the NuGet packages and update the `using` statement.

There is an additional NuGet-package called *DENKApi.NET.WindowsHelper*. This packages has dependencies to other NuGet-Packages that are only available on Windows. When using this package, your project should target windows. Otherwise VisualStudio will give you a warning about this. While .NET Framework projects are only available for windows, .NET projects can be marked as windows specific by adding `windows` to the `TargetFramework` property in the proejct file (e.g. `<TargetFramework>net8.0-windows</TargetFramework>`).
The *DENKApi.NET.WindowsHelper* package provides a few methods to support you with converting .NET types to byte arrays for usage with the native API.

## Preparing Solution / Project
The example applications in this solution are already properly configured. So you can run the examples immediately. Here are some general instructions to follow when setting up a new solution for usage with *DENKApi.NET*:

1. Create solution and/or project in VisualStudio (.NET as well as .NET Framework is supported)
2. Add downloaded NuGet packages to a local folder (`nuget add` will accomplish this - see [Local Feeds on learn.microsoft.com](https://learn.microsoft.com/en-us/nuget/hosting-packages/local-feeds))
3. Add package source to local nuget feed (see [Package sources on learn.micorosft.com](https://learn.microsoft.com/en-us/nuget/consume-packages/install-use-packages-visual-studio#package-sources))
4. Install NuGet packages via VisualStudio or on command line (ensure that the proper package source is selected, when using UI within VisualStudio)
5. Ensure that `x64` configuration is available and selected in *Configuration Manager* (*DENKApi.NET* requires `x64` and will not work on `AnyCPU`)

### Important note on `packages.config`
The provided NuGet-Packages are created with .NET8 and especially SDK-style projects in mind. Generally .NET Framework is supported. However, it might happen that projects using `packages.config` instead of `PackageReference` throw exceptions indicating that certain *DLLs* were not found. This will happen at runtime and **not** while installing the NuGet package.

The problem here is that some *DLLs* will not be copied from the NuGet package into your project. This issue can be solved by migrating your project from `packages.config` to `PackageReference`. Microsoft documents this process [here](https://learn.microsoft.com/en-us/nuget/consume-packages/migrate-packages-config-to-package-reference).

In many cases it is as simple as executing *Migrate packages.config to PackageReference....* from the context menu on the `packages.config` file in VisualStudio.