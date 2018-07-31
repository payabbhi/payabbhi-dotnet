using System.Diagnostics;

namespace Payabbhi
{
	public class OSHelper
	{
		public virtual string ReadProcessOutput(string name, string args)
		{
			try
			{
				Process p = new Process();
				p.StartInfo.UseShellExecute = false;
				p.StartInfo.RedirectStandardOutput = true;
				if (!string.IsNullOrEmpty(args)) p.StartInfo.Arguments = " " + args;
				p.StartInfo.FileName = name;
				p.Start();
				string output = p.StandardOutput.ReadToEnd();
				p.WaitForExit();
				if (output == null) output = "";
				output = output.Trim();
				return output;
			}
			catch
			{
				return "";
			}
		}
	}
}
