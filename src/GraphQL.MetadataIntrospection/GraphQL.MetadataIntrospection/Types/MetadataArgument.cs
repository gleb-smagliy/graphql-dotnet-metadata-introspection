using GraphQL.Types;

namespace GraphQL.MetadataIntrospection.Types
{
    internal class MetadataArgument : ObjectGraphType<Model.MetadataArgument>
    {
        public MetadataArgument()
        {
            Field(_ => _.Name).Description("Argument name");
            Field(_ => _.Value).Description("Argument value");
            Field("type", _ => _.TypeName).Description("Argument GraphQL type");
        }
    }
}
