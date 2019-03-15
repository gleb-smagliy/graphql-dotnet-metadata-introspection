namespace GraphQL.MetadataIntrospection.Schema
{
    internal interface IDirectivesFilter
    {
        bool Include(string directiveName);
    }
}