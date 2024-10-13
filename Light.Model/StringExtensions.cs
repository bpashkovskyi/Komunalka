namespace Light.Model;

public static class StringExtensions
{
    public static string ToSentenceCase(this string input)
    {
        if (string.IsNullOrEmpty(input))
        {
            return input;
        }

        input = input.ToLower();


        if (input.Length == 1)
        {
            return input.ToUpper();
        }
            
        return input.Remove(1).ToUpper() + input.Substring(1);
    }
}