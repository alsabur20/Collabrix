namespace Collabrix.Utils
{
    public static class Utils
    {
        public static string GetInitials(string fullName)
        {
            if (string.IsNullOrWhiteSpace(fullName))
                return string.Empty;

            var words = fullName.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
            string initials = string.Concat(words.Select(word => char.ToUpper(word[0])));

            return initials;
        }
    }
}
