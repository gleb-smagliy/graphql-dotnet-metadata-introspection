using System.Text.RegularExpressions;

namespace GraphQL.MetadataIntrospection.Schema
{
    internal static class MetadataFieldNameValidator
    {
        private const string FieldRegexPattern = "^[_A-Za-z][_0-9A-Za-z]*$";
        
        private static readonly Regex FieldRegexp = new Regex(FieldRegexPattern, RegexOptions.Compiled);
        
        public static FieldValidationResult Validate(string fieldName)
        {
            if (!FieldRegexp.IsMatch(fieldName))
            {
                return new FieldValidationResult
                {
                    Success = false,
                    Error = $"Field name <{fieldName}> should satisfy the regex <{FieldRegexPattern}>"
                };
            }

            return new FieldValidationResult
            {
                Success = true
            };
        }
    }
}