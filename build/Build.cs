using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Nuke.Common;
using Nuke.Common.CI;
using Nuke.Common.Execution;
using Nuke.Common.IO;
using Nuke.Common.ProjectModel;
using Nuke.Common.Tooling;
using Nuke.Common.Tools.DotNet;
using Nuke.Common.Tools.MSBuild;
using Nuke.Common.Utilities.Collections;
using static Nuke.Common.EnvironmentInfo;
using static Nuke.Common.IO.FileSystemTasks;
using static Nuke.Common.IO.PathConstruction;
using static Nuke.Common.Tools.DotNet.DotNetTasks;

class Build : NukeBuild
{
    /// Support plugins are available for:
    ///   - JetBrains ReSharper        https://nuke.build/resharper
    ///   - JetBrains Rider            https://nuke.build/rider
    ///   - Microsoft VisualStudio     https://nuke.build/visualstudio
    ///   - Microsoft VSCode           https://nuke.build/vscode

    public static int Main () => Execute<Build>(x => x.Compile);

    [Solution] readonly Solution Solution;

    [Parameter("Configuration to build - Default is 'Debug' (local) or 'Release' (server)")]
    readonly Configuration Configuration = IsLocalBuild ? Configuration.Debug : Configuration.Release;
    
    [Parameter("Directory of the PlateUp root folder")]
    readonly string PlateUpDir;

    readonly AbsolutePath GameAssembliesDir = RootDirectory / ".GameAssemblies";

    IEnumerable<Project> GetProjects()
    {
        return Solution.GetProjects("*")
            .Where(project => project.Directory != BuildProjectDirectory);
    }

    Target Clean => _ => _
        .Before(Restore, GetGameAssemblies)
        .Executes(() =>
        {
            GetProjects().ForEach(project =>
            {
                DotNetClean(new DotNetCleanSettings().SetProject(project));
            });
            
            if (GameAssembliesDir.DirectoryExists())
                Directory.Delete(GameAssembliesDir, true);
        });

    Target GetGameAssemblies => _ => _
        .Requires(() => PlateUpDir)
        .Executes(() =>
        {
            AbsolutePath managedDir = (AbsolutePath)PlateUpDir / "PlateUp" / "PlateUp_Data" / "Managed";
            
            Assert.DirectoryExists(PlateUpDir);
            Assert.DirectoryExists(managedDir);
            
            var assembliesToCopy = managedDir.GlobFiles("*.dll")
                .Where(file => !file.Name.Contains("netstandard.dll") && !file.Name.StartsWith("System."));

            if (GameAssembliesDir.DirectoryExists())
            {
                var existingAssemblies = GameAssembliesDir.GlobFiles("*.dll").Select(assembly => assembly.Name);
                assembliesToCopy = assembliesToCopy.Where(assembly => !existingAssemblies.Contains(assembly.Name));
            }
            else
            {
                Directory.CreateDirectory(GameAssembliesDir);
            }
            
            assembliesToCopy.ForEach(assembly =>
            {
                AbsolutePath dest = GameAssembliesDir / assembly.Name;
                File.Copy(assembly, dest);
            });
        });

    Target Restore => _ => _
        .Executes(() =>
        {
            GetProjects().ForEach(project =>
            {
                DotNetRestore(new DotNetRestoreSettings().SetProjectFile(project.Path));
            });
        });

    Target Compile => _ => _
        .DependsOn(Restore, GetGameAssemblies)
        .Executes(() =>
        {
            GetProjects().ForEach(project =>
            {
                DotNetBuild(new DotNetBuildSettings().SetProjectFile(project.Path));
            });
        });

    Target ExportModsToGame => _ => _
        .DependsOn(Compile)
        .Requires(() => PlateUpDir)
        .Executes(() =>
        {
            AbsolutePath modsDir = (AbsolutePath)PlateUpDir / "PlateUp" / "Mods";
            
            GetProjects().ForEach(project =>
            {
                var msBuildProject = project.GetMSBuildProject();

                string targetDir = msBuildProject.GetPropertyValue("TargetDir");
                string targetName = msBuildProject.GetPropertyValue("TargetName");

                AbsolutePath dll = (AbsolutePath)targetDir / $"{targetName}.dll";
                AbsolutePath pdb = (AbsolutePath)targetDir / $"{targetName}.pdb";
                
                Assert.FileExists(dll);
                Assert.FileExists(pdb);

                AbsolutePath destFolder = modsDir / targetName;
                if (destFolder.DirectoryExists())
                    Directory.Delete(destFolder, true);
                Directory.CreateDirectory(destFolder);

                AbsolutePath destDll = destFolder / dll.Name;
                AbsolutePath destPdb = destFolder / pdb.Name;
                
                File.Copy(dll, destDll);
                File.Copy(pdb, destPdb);
            });
        });
}
