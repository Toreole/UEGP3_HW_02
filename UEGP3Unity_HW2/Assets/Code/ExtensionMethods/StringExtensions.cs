namespace UEGP3.ExtensionMethods
{
	public static class StringExtensions
	{
		public static bool IsNullOrEmpty(this string s)
		{
			return (s == null) || s.Equals("") || s.Equals(" ");
		}
	}
}