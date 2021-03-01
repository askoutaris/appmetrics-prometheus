using System.Text;
using System.Text.RegularExpressions;

namespace AppMetricsPrometheus
{
	public static class Constants
	{
		public static readonly UTF8Encoding UTF8Encoding = new UTF8Encoding(false);
		public static readonly Regex ValidPrometheusNameCharsRegex = new Regex(@"[^a-zA-Z0-9:_]");
	}
}
