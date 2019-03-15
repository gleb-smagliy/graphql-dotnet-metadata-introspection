using GraphQLParser.AST;

namespace GraphQL.MetadataIntrospection.DirectivesVisitor.DirectivesUsages
{
    internal struct ObjectTypeDirectiveUsage
    {
        public GraphQLObjectTypeDefinition OnObject { get; private set; }

        public GraphQLDirective Directive { get; private set; }

        public ObjectTypeDirectiveUsage(GraphQLObjectTypeDefinition onObject, GraphQLDirective directive)
        {
            this.OnObject = onObject;
            this.Directive = directive;
        }
    }
}
