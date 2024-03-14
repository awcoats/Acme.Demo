using System;
using System.CodeDom;
using System.Linq;
using Nuke.Common;
using Nuke.Common.CI;
using Nuke.Common.Execution;
using Nuke.Common.IO;
using Nuke.Common.ProjectModel;
using Nuke.Common.Tooling;
using Nuke.Common.Tools.DotNet;
using Nuke.Common.Utilities.Collections;
using Serilog;
using static Nuke.Common.EnvironmentInfo;
using static Nuke.Common.IO.FileSystemTasks;
using static Nuke.Common.IO.PathConstruction;

class Build : NukeBuild
{
    /// Support plugins are available for:
    ///   - JetBrains ReSharper        https://nuke.build/resharper
    ///   - JetBrains Rider            https://nuke.build/rider
    ///   - Microsoft VisualStudio     https://nuke.build/visualstudio
    ///   - Microsoft VSCode           https://nuke.build/vscode

    public static int Main () => Execute<Build>(x => x.UnitTest);

    [Parameter("Configuration to build - Default is 'Debug' (local) or 'Release' (server)")]
    readonly Configuration Configuration = IsLocalBuild ? Configuration.Debug : Configuration.Release;

    AbsolutePath SourceDirectory => RootDirectory / "src";
    AbsolutePath TestsDirectory => RootDirectory / "tests";
    AbsolutePath ArtifactsDirectory => RootDirectory / "artifacts";

    
     
    Target Clean => _ => _
        .Before(Restore)
        .Executes(() =>
        {
            Log.Information("Host:{Host}",Host);
            Log.Information("IsLocalBuild:{IsLocalBuild}",IsLocalBuild);
            Log.Information("IsServerBuild:{IsServerBuild}",IsServerBuild);
            //Log.Information("Profile:{Profile}",Configuration);
            //Log.Information("Release:{Release}",Configuration.Release);
            //Log.Information("Debug:{Debug}",Configuration.Debug);
           
            DotNetTasks.DotNetClean();
            //SourceDirectory.GlobDirectories("**/bin", "**/obj").DeleteDirectories();
            //TestsDirectory.GlobDirectories("**/bin", "**/obj").DeleteDirectories();
            ArtifactsDirectory.CreateOrCleanDirectory();
        });

    Target Restore => _ => _
        .DependsOn(Clean)
        .Executes(() =>
        {
          DotNetTasks.DotNetRestore();
             
        });

    Target Compile => _ => _
        .DependsOn(Restore)
        .Executes(() =>
        {
            Log.Information("Compile!!!");
            DotNetTasks.DotNetBuild();
        });

    Target UnitTest => _ => _
        .DependsOn(Compile)
        .Executes(() =>
        {
            DotNetTasks.DotNetTest();
        });

     Target Publish => _ => _
        .Executes(() =>
        {
            var publishSettings = new DotNetPublishSettings()
                .SetOutput(ArtifactsDirectory / "Acme.Demo.Web")
                .SetProject("./src/Acme.Demo.Web/Acme.Demo.Web.csproj")
                .SetConfiguration(Configuration);
            DotNetTasks.DotNetPublish(publishSettings);
        });
}
