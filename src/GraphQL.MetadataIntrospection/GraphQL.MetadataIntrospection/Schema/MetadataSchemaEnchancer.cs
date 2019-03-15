using System.Collections.Generic;
using GraphQL.MetadataIntrospection.Types;
using GraphQL.Resolvers;
using GraphQL.Types;

namespace GraphQL.MetadataIntrospection.Schema
{
    internal class MetadataSchemaEnchancer
    {
        private readonly IFieldResolver<IEnumerable<Model.Metadata>> _resolver;

        public MetadataSchemaEnchancer(IEnumerable<Model.Metadata> metadata)
        {
            this._resolver = new MetadataFieldResolver(metadata);
        }

        public ISchema MutateSchema(ISchema schema, string metadataQueryName)
        {
            var fieldType = new FieldType
            {
                Resolver = this._resolver,
                Name = metadataQueryName,
                Type = typeof(ListGraphType<Metadata>)
            };
            
            schema.Query.AddField(fieldType);

            return schema;
        }
    }
}