#tool nuget:?package=NUnit.ConsoleRunner&version=3.4.0
#addin nuget:?package=Cake.Git&version=0.16.0
//////////////////////////////////////////////////////////////////////
// ARGUMENTS
//////////////////////////////////////////////////////////////////////

var target = Argument("target", "Default");
var configuration = Argument("configuration", "Release");

//////////////////////////////////////////////////////////////////////
// PREPARATION
//////////////////////////////////////////////////////////////////////

// Define directories.
var buildDir40 = Directory("./net40/bin") + Directory(configuration);
var buildDir45 = Directory("./net45/bin") + Directory(configuration);
var buildDirTest = Directory("./test/bin") + Directory(configuration);

var assemblyInfo40 = "./net40/Properties/AssemblyInfo.cs";
var assemblyInfo45 = "./net45/Properties/AssemblyInfo.cs";
var nuSpec = "./Payabbhi.nuspec";

//////////////////////////////////////////////////////////////////////
// TASKS
//////////////////////////////////////////////////////////////////////

Task("Clean")
    .Does(() =>
{
    CleanDirectory(buildDir40);
    CleanDirectory(buildDir45);
    CleanDirectory(buildDirTest);
});

Task("AssemblyInfo")
    .Does(() =>
{
    var product     = XmlPeek(File(nuSpec), "/package/metadata/id/text()");
    var version     = XmlPeek(File(nuSpec), "/package/metadata/version/text()");
    var description = XmlPeek(File(nuSpec), "/package/metadata/description/text()");
    var company     = XmlPeek(File(nuSpec), "/package/metadata/owners/text()");
    var copyright   = XmlPeek(File(nuSpec), "/package/metadata/copyright/text()");
    var title       = XmlPeek(File(nuSpec), "/package/metadata/id/text()");
    var gitCommit   = GitLogTip(".");

    var ais = new AssemblyInfoSettings {
      Product = product,
      Version = version,
      Description = description,
      Company   = company,
      Copyright = copyright,
      Title = title,
      Configuration = configuration,
      FileVersion = version,
      InformationalVersion = gitCommit.Sha
    };

    CreateAssemblyInfo(assemblyInfo40, ais);
    CreateAssemblyInfo(assemblyInfo45, ais);

});

Task("Restore-NuGet-Packages")
    .IsDependentOn("AssemblyInfo")
    .Does(() =>
{
    NuGetRestore("./PayabbhiNet40.sln");
    NuGetRestore("./PayabbhiNet45.sln");
});

Task("Build")
    .IsDependentOn("Restore-NuGet-Packages")
    .Does(() =>
{
      MSBuild("./PayabbhiNet40.sln", settings =>
        settings.SetConfiguration(configuration));
      MSBuild("./PayabbhiNet45.sln", settings =>
        settings.SetConfiguration(configuration));
});

Task("Run-Unit-Tests")
    .IsDependentOn("Build")
    .Does(() =>
{
    NUnit3("./test/bin/" + configuration + "/PayabbhiTest40.dll", new NUnit3Settings {
        NoResults = true
        });
    NUnit3("./test/bin/" + configuration + "/PayabbhiTest45.dll", new NUnit3Settings {
        NoResults = true
        });
});

//////////////////////////////////////////////////////////////////////
// TASK TARGETS
//////////////////////////////////////////////////////////////////////

Task("Default")
    .IsDependentOn("Run-Unit-Tests");

//////////////////////////////////////////////////////////////////////
// EXECUTION
//////////////////////////////////////////////////////////////////////

RunTarget(target);
