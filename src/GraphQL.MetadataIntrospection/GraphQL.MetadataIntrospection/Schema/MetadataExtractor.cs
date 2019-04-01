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
            _directivesFilter = directivesFilter;
        }

        private static MetadataArgument MapArgument(GraphQLArgument argument)
        {
            return new MetadataArgument
            {
                Name = argument.Name.Value,
                Value = argument.Value.ToString()

            };
        }

        protected override void ObjectFieldDirectiveUsageVisited(ObjectFieldDirectiveUsage usage)
        {
            var directiveName = usage.Directive.Name.Value;
            
            if (!_directivesFilter.Include(directiveName))
            {
                return;
            }
            
            var metadata = new Metadata
            {
                Location = MetadataLocation.OBJECT_FIELD,
                Name = directiveName,
                Arguments = usage.Directive.Arguments.Select(MapArgument).ToArray(),
                FieldName = usage.OnField.Name.Value,
                TypeName = usage.OnObject.Name.Value
            };

            _metadata.Add(metadata);
        }

        protected override void ObjectTypeDirectiveUsageVisited(ObjectTypeDirectiveUsage usage)
        {
            var directiveName = usage.Directive.Name.Value;
            
            if (!_directivesFilter.Include(directiveName))
            {
                return;
            }

            var metadata = new Metadata
            {
                Location = MetadataLocation.OBJECT_TYPE,
                Name = directiveName,
                Arguments = usage.Directive.Arguments.Select(MapArgument).ToArray(),
                TypeName = usage.OnObject.Name.Value
            };

            _metadata.Add(metadata);
        }

        public IEnumerable<Metadata> ExtractMetadata(GraphQLDocument document)
        {
            _metadata = new List<Metadata>();

            Walk(document);

            return _metadata;
        }
    }
}
