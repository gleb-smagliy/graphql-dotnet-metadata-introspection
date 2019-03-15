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
           
            if (this._includedDirectives.Length > 0 && !this._includedDirectives.Contains(directiveName))
            {
                return false;
            }

            if (this._excludedDirectives.Length > 0 && this._excludedDirectives.Contains(directiveName))
            {
                return false;
            }

            return true;
        }
    }
}