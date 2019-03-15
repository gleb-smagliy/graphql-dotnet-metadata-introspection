using GraphQL.Types;

namespace GraphQL.MetadataIntrospection.Types
{
    internal class Metadata : ObjectGraphType<Model.Metadata>
    {
        public Metadata()
        {
            Field(_ => _.Name).Description("Metadata name");
            Field<MetadataLocation>("location", "Metadata location in the schema");
            Field<ListGraphType<MetadataArgument>>("arguments", resolve: ctx => ctx.Source.Arguments, description: "Metadata arguments");
        }
    }
}
