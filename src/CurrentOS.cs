namespace Payabbhi
{
	public class CurrentOS
	{
		public string Name { get; private set; }

		public CurrentOS(char pathDirectorySeparatorChar, string osVersionString, bool is64bit, OSHelper osHelper)
		{
			bool IsWindows = pathDirectorySeparatorChar == '\\';
			if (IsWindows)
			{
				Name = osVersionString;

				Name = Name.Replace("Microsoft ", "");
				Name = Name.Replace("  ", " ");
				Name = Name.Replace(" )", ")");
				Name = Name.Trim();

				Name = Name.Replace("NT 10", "10 %bit 10");
				Name = Name.Replace("NT 6.2", "8 %bit 6.2");
				Name = Name.Replace("NT 6.1", "7 %bit 6.1");
				Name = Name.Replace("NT 6.0", "Vista %bit 6.0");
				Name = Name.Replace("NT 5.", "XP %bit 5.");
				Name = Name.Replace("%bit", (is64bit ? "64bit" : "32bit"));
			}
			else
			{
				string UnixName = osHelper.ReadProcessOutput("uname", null);
				if (UnixName.Contains("Darwin"))
				{
					Name = "Darwin MacOS X " + osHelper.ReadProcessOutput("sw_vers", "-productVersion");
					Name = Name.Trim();

					Name += (is64bit ? " 64bit" : " 32bit");
				}
				else if (UnixName != "")
				{
					Name = "Unix";
				}
				else
				{
					Name = "Unknown";
				}
			}
		}
	}
}
