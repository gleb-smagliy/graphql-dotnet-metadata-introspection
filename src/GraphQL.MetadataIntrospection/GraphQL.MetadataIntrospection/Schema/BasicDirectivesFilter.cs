using System.Linq;

namespace GraphQL.MetadataIntrospection.Schema
{
    internal class BasicDirectivesFilter : IDirectivesFilter
    {
        private readonly string[] _includedDirectives;
        private readonly string[] _excludedDirectives;

        public BasicDirectivesFilter(string[] includedDirectives, string[] excludedDirectives)
        {
            _includedDirectives = includedDirectives;
            _excludedDirectives = excludedDirectives;
        }

        public bool Include(string directiveName)
        {
            return (_excludedDirectives.Length <= 0 || !_excludedDirectives.Contains(directiveName)) &&
                   (_includedDirectives.Length <= 0 || _includedDirectives.Contains(directiveName));
        }
    }
}