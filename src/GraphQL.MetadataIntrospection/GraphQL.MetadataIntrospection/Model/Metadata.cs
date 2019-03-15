namespace GraphQL.MetadataIntrospection.Model
{
    internal class Metadata
    {
        public MetadataLocation Location { get; set; }
        public string Name { get; set; }
        public MetadataArgument[] Arguments { get; set; }
    }
}
