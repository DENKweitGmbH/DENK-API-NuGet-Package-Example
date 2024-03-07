# DENKApi.NET sample - basic workflow with *WindowsHelper* on CPU
Shows how to use *DENKApi.NET.CPU* togehter with *DENKApi.NET.WindowsHelper* to prepare network and evaluate example image. This example is based on .NET Framework 4.7.2.

## Preparation
For general instructions on setting up a new solution/project using *DENKApi.NET* please see *README* on solution level.

1. Add `DENKApi.NET.CPU` and `DENKApi.NET.WindowsHelper` packages to project.
2. Get access to CPU version of *DENKApi.NET* via `IDENKApi denkApi = DENKApiFactory.GetApi();` and ensure that the `DENKApi.NET.CPU.DENKApiFactory` is used (the namespace is **important** here).
3. Now the *DENKApi.NET* can be used to analyse images on the CPU.

**IMPORTANT:** In case you use this example as blueprint for your own project based on .NET Framework 4.7.2 (or lower), keep in mind that `packages.config` can cause problems when using the *DENKApi.NET* NuGet packages. Please see *README* on solution level for further details.
