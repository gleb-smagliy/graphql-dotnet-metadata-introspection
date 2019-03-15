using System.Collections.Generic;
using System.Linq;
using GraphQL.MetadataIntrospection.DirectivesVisitor;
using GraphQL.MetadataIntrospection.DirectivesVisitor.DirectivesUsages;
using GraphQL.MetadataIntrospection.Model;
using GraphQLParser.AST;

namespace GraphQL.MetadataIntrospection.Schema
{
    internal class MetadataExtractor : DirectivesUsagesVisitor
    {
        private readonly IDirectivesFilter _directivesFilter;

        private List<Metadata> _metadata;

        public MetadataExtractor(IDirectivesFilter directivesFilter)
        {
            this._directivesFilter = directivesFilter;
        }

        private static MetadataArgument MapArgument(GraphQLArgument argument)
        {
            return new MetadataArgument
            {
                TypeName = argument.Value.Kind.ToString(),
                Name = argument.Name.Value.ToString(),
                Value = argument.Value.ToString()
            };
        }

        protected override void ObjectFieldDirectiveUsageVisited(ObjectFieldDirectiveUsage usage)
        {
            var directiveName = usage.Directive.Name.Value;
            
            if (!this._directivesFilter.Include(directiveName))
            {
                return;
            }
            
            var metadata = new Metadata
            {
                Location = MetadataLocation.OBJECT_FIELD,
                Name = directiveName,
                Arguments = usage.Directive.Arguments.Select(MapArgument).ToArray()
            };

            this._metadata.Add(metadata);
        }

        protected override void ObjectTypeDirectiveUsageVisited(ObjectTypeDirectiveUsage usage)
        {
            var directiveName = usage.Directive.Name.Value;
            
            if (!this._directivesFilter.Include(directiveName))
            {
                return;
            }

            var metadata = new Metadata
            {
                Location = MetadataLocation.OBJECT_TYPE,
                Name = directiveName,
                Arguments = usage.Directive.Arguments.Select(MapArgument).ToArray()
            };

            this._metadata.Add(metadata);
        }

        public IEnumerable<Metadata> ExtractMetadata(GraphQLDocument document)
        {
            this._metadata = new List<Metadata>();

            this.Walk(document);

            return this._metadata;
        }
    }
}
