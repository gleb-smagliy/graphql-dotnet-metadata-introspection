using System.Collections.Generic;
using System.Linq;
using GraphQL.MetadataIntrospection.Model;
using GraphQL.MetadataIntrospection.Schema;
using GraphQL.Types;
using Xunit;

namespace GraphQL.MetadataIntrospection.Tests.Schema
{
    public class MetadataSchemaEnchanterTests
    {
        [Fact]
        public void Mutate_Schema_Test()
        {            
            var qName = "_meta";           
            var meta = new List<Metadata>();           
            var initSchema = new GraphQL.Types.Schema {Query = new ObjectGraphType()};

            var schema = new MetadataSchemaEnchanter(meta).MutateSchema(initSchema, qName);
            Assert.Single(schema.Query.Fields);

            var field = schema.Query.Fields.First();
            Assert.Equal(qName, field.Name);
            Assert.Equal(typeof(ListGraphType<GraphQL.MetadataIntrospection.Types.Metadata>), field.Type);
            var ou = field.Resolver.Resolve(null);
            Assert.Equal(meta, ou);
        }
    }
}