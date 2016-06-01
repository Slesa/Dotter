using System;
using System.Diagnostics;
using System.IO;

namespace Dotter.Core.Helpers
{
    public class DotCaller
    {
        string InFileName
        {
            get { return Path.Combine(Path.GetTempPath(), "dotter.in.dot"); }
        }

        string OutFileName
        {
            get { return Path.Combine(Path.GetTempPath(), "dotter."+Guid.NewGuid()+".png"); }
        }

        public string Run(string input)
        {
            File.Delete(OutFileName);

            var tmpInput = InFileName;
            File.WriteAllText(tmpInput, input);

            var tmpOutput = OutFileName; 

            // Start the child process.
            var p = new Process();
            // Redirect the output stream of the child process.
            p.StartInfo.WindowStyle = ProcessWindowStyle.Hidden;
            p.StartInfo.UseShellExecute = false;
            p.StartInfo.RedirectStandardOutput = true;
            p.StartInfo.CreateNoWindow = true;
            p.StartInfo.WorkingDirectory = "files";
            p.StartInfo.FileName = "files\\dot.exe";
            p.StartInfo.Arguments = "-Tpng " + tmpInput + " -o " + tmpOutput; // + " -s=128";
            p.Start();
            // Do not wait for the child process to exit before
            // reading to the end of its redirected stream.
            // p.WaitForExit();
            // Read the output stream first and then wait.
            var output = p.StandardOutput.ReadToEnd();
            p.WaitForExit();

            return p.ExitCode != 0 ? string.Empty : tmpOutput;
            //return tmpOutput;
        }
    }
}