# DENKApi.NET sample - basic workflow DML
Shows how to use *DENKApi.NET.DML* to prepare network and evaluate example image on GPU (via DML). This example is based on .NET8.

## Preparation
For general instructions on setting up a new solution/project using *DENKApi.NET* please see *README* on solution level.

1. Add `DENKApi.NET.DML` package to project.
2. Get access to GPU version of *DENKApi.NET* via `IDENKApi denkApi = DENKApiFactory.GetApi();` and ensure that the `DENKApi.NET.DML.DENKApiFactory` is used (the namespace is **important** here).
3. Now the *DENKApi.NET* can be used to analyse images on the GPU.

## *DENKApi.NET.WindowsHelper*
In this example the package *DENKApi.NET.WindowsHelper* is **not** used. Instead it shows how the conversion from images into byte arrays or from byte arrays into `Results` object can be accomplished.

Like the *DENKApi.NET.WindowsHelper* the project therefore uses windows specific NuGet packages (like `System.Drawing.Common`). This is only an example how the conversion can be done. There might be other ways that suite your specific needs better.
