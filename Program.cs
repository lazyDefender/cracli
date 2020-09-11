using MatthiWare.CommandLine;
using System;
using System.Reflection;
using System.Diagnostics;
using System.Net.Http;
using CraCli.Options;
using System.Threading.Tasks;
using System.IO;
using System.Dynamic;

namespace CraCli
{
    class Program
    {
        static string location = Environment.CurrentDirectory;
        static string projectName;
        
        static string GetGitUsername()
        {
            Process process = new Process();
            ProcessStartInfo startInfo = new ProcessStartInfo
            {
                WindowStyle = ProcessWindowStyle.Hidden,
                FileName = "cmd.exe",
                Arguments = $@"/C git config --get user.name",
                RedirectStandardOutput = true
            };
            process.StartInfo = startInfo;
            process.Start();
            process.WaitForExit();
            return process.StandardOutput.ReadLine();
        }
        static void CloneRepo(string url)
        {
            Process process = new Process();
            ProcessStartInfo startInfo = new ProcessStartInfo
            {
                WindowStyle = ProcessWindowStyle.Hidden,
                FileName = "cmd.exe",
                Arguments = $@"/C git clone {url}",
                RedirectStandardOutput = true
            };
            process.StartInfo = startInfo;
            process.Start();
            Console.WriteLine(process.StandardOutput.ReadToEnd());
            process.WaitForExit();
        }

        static void RepoRemoveRemote(string remoteUrl)
        {
            Process process = new Process();
            ProcessStartInfo startInfo = new ProcessStartInfo
            {
                WindowStyle = ProcessWindowStyle.Hidden,
                FileName = "cmd.exe",
                Arguments = $@"/C git remote rm {remoteUrl}",
                RedirectStandardOutput = true,
                WorkingDirectory = $"{location}/{projectName}"
            };
            process.StartInfo = startInfo;
            process.Start();
            process.WaitForExit();
        }

        static void RepoSetRemote(string remote)
        {
            Process process = new Process();
            ProcessStartInfo startInfo = new ProcessStartInfo {
                WindowStyle = ProcessWindowStyle.Hidden,
                FileName = "cmd.exe",
                Arguments = $@"/C git remote set-url origin {remote}",
                RedirectStandardOutput = true,
                WorkingDirectory = $"{location}/{projectName}"
            };
            process.StartInfo = startInfo;
            process.Start();
            process.WaitForExit();
        }

        static void RepoPush()
        {
            Process process = new Process();
            ProcessStartInfo startInfo = new ProcessStartInfo
            {
                WindowStyle = ProcessWindowStyle.Hidden,
                FileName = "cmd.exe",
                Arguments = $@"/C git push -u origin master",
                RedirectStandardOutput = true,
                WorkingDirectory = $"{location}/{projectName}"
            };
            process.StartInfo = startInfo;
            process.Start();
            process.WaitForExit();
        }

        static void CreateRepo(string name)
        {
            Process process = new Process();
            ProcessStartInfo startInfo = new ProcessStartInfo
            {
                WindowStyle = ProcessWindowStyle.Hidden,
                FileName = "cmd.exe",
                Arguments = $@"/C gh repo create",
                RedirectStandardOutput = true,
                WorkingDirectory = $"{location}/{projectName}"
            };
            process.StartInfo = startInfo;
            process.Start();
            process.WaitForExit();
        }
        static async Task Main(string[] args)
        {
            var options = new CommandLineParserOptions
            {
                AppName = "cra"
            };
            var parser = new CommandLineParser<ProgramOptions>();
            var result = parser.Parse(args);
            var programOptions = result.Result;
            projectName = programOptions.Name;
            Console.WriteLine(projectName);

            
            Console.WriteLine("Location:" + location);
            string repoURL = @"https://github.com/lazyDefender/cra";
            CloneRepo($"{repoURL}.git");
            Directory.Move($@"{location}/cra", $@"{location}/{projectName}");
            RepoRemoveRemote($"origin");
            CreateRepo(projectName);
            string gitUsername = GetGitUsername();
            string newUrl = $@"https://github.com/{gitUsername}/{projectName}.git";
            RepoSetRemote(newUrl);
            RepoPush();
        }
    }
}
