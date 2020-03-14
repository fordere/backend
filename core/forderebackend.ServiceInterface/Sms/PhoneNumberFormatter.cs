namespace forderebackend.ServiceInterface.Sms
{
    public class PhoneNumberFormatter
    {
        public static string Format(string input)
        {
            if (input == null)
            {
                return string.Empty;
            }

            var cleanInput = RemoveUnknownCharacters(input);

            if (cleanInput.StartsWith("0041"))
            {
                cleanInput = "+41" + cleanInput.Substring(4);
            }

            if (cleanInput.Length > 0 && cleanInput[0] == '0')
            {
                cleanInput = "+41" + cleanInput.Substring(1);
            }

            return cleanInput;
        }

        private static string RemoveUnknownCharacters(string input)
        {
            return input.Trim()
                .Replace(" ", string.Empty)
                .Replace("(0)", string.Empty)
                .Replace("'", string.Empty)
                .Replace("*", string.Empty)
                .Replace("/", string.Empty)
                .Replace(".", string.Empty);
        }
    }
}
