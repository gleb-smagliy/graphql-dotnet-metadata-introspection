using System.Collections.Generic;
using GraphQL.MetadataIntrospection.Model;
using GraphQL.Resolvers;
using GraphQL.Types;

namespace GraphQL.MetadataIntrospection.Schema
{
    internal class MetadataFieldResolver : IFieldResolver<IEnumerable<Metadata>>
    {
        private readonly IEnumerable<Metadata> _metadata;

        public MetadataFieldResolver(IEnumerable<Metadata> metadata)
        {
            _metadata = metadata;
        }

        public IEnumerable<Metadata> Resolve(ResolveFieldContext context)
        {
            return _metadata;
        }

        object IFieldResolver.Resolve(ResolveFieldContext context)
        {
            return Resolve(context);
        }
    }
}