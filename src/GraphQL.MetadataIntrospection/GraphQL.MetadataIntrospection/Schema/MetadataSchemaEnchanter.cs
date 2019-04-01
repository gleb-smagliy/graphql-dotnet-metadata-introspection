using System.Collections.Generic;
using GraphQL.MetadataIntrospection.Types;
using GraphQL.Resolvers;
using GraphQL.Types;

namespace GraphQL.MetadataIntrospection.Schema
{
    internal class MetadataSchemaEnchanter
    {
        private readonly IFieldResolver<IEnumerable<Model.Metadata>> _resolver;

        public MetadataSchemaEnchanter(IEnumerable<Model.Metadata> metadata)
        {
            _resolver = new MetadataFieldResolver(metadata);
        }

        public ISchema MutateSchema(ISchema schema, string metadataQueryName)
        {
            var fieldType = new FieldType
            {
                Resolver = _resolver,
                Name = metadataQueryName,
                Type = typeof(ListGraphType<Metadata>)
            };
            
            schema.Query.AddField(fieldType);

            return schema;
        }
    }
}