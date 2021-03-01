namespace AppMetricsPrometheus
{
	public enum NewLineFormat
	{
		//
		// Summary:
		//     Use Unix style new line character by default
		Default,
		//
		// Summary:
		//     Use Environement.NewLine as new line character
		Auto,
		//
		// Summary:
		//     Use '\r\n' as new line character
		Windows,
		//
		// Summary:
		//     Use '\r' as new line character
		Unix
	}
}
