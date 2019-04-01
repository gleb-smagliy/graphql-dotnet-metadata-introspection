using GraphQL.MetadataIntrospection.DirectivesVisitor.DirectivesUsages;
using GraphQLParser.AST;

namespace GraphQL.MetadataIntrospection.DirectivesVisitor
{
    internal abstract class DirectivesUsagesVisitor
    {
        protected void Walk(GraphQLDocument document)
        {
            foreach (var astNode in document.Definitions)
            {
                switch (astNode.Kind)
                {
                    case ASTNodeKind.ObjectTypeDefinition:
                        VisitObjectType((GraphQLObjectTypeDefinition)astNode);
                        break;
                    case ASTNodeKind.TypeExtensionDefinition:
                        var extension = (GraphQLTypeExtensionDefinition)astNode;
                        VisitObjectType(extension.Definition);
                        break;
                    case ASTNodeKind.InterfaceTypeDefinition:
                        VisitInterfaceType((GraphQLInterfaceTypeDefinition)astNode);
                        break;
                }
            }
        }

        protected virtual void InterfaceFieldDirectiveUsageVisited(InterfaceFieldDirectiveUsage usage) { }
        protected virtual void InterfaceTypeDirectiveUsageVisited(InterfaceTypeDirectiveUsage usage) { }
        protected virtual void ObjectFieldDirectiveUsageVisited(ObjectFieldDirectiveUsage usage) { }
        protected virtual void ObjectTypeDirectiveUsageVisited(ObjectTypeDirectiveUsage usage) { }

        private void VisitObjectType(GraphQLObjectTypeDefinition objectType)
        {
            foreach (var directive in objectType.Directives)
            {
                var usage = new ObjectTypeDirectiveUsage(objectType, directive);
                ObjectTypeDirectiveUsageVisited(usage);
            }

            foreach (var field in objectType.Fields)
            {
                VisitObjectTypeField(objectType, field);
            }
        }

        private void VisitObjectTypeField(GraphQLObjectTypeDefinition objectType, GraphQLFieldDefinition field)
        {
            foreach (var directive in field.Directives)
            {
                var usage = new ObjectFieldDirectiveUsage(objectType, field, directive);
                ObjectFieldDirectiveUsageVisited(usage);
            }
        }

        private void VisitInterfaceType(GraphQLInterfaceTypeDefinition interfaceType)
        {
            foreach (var directive in interfaceType.Directives)
            {
                var usage = new InterfaceTypeDirectiveUsage(interfaceType, directive);
                InterfaceTypeDirectiveUsageVisited(usage);
            }

            foreach (var field in interfaceType.Fields)
            {
                VisitInterfaceTypeField(interfaceType, field);
            }
        }

        private void VisitInterfaceTypeField(GraphQLInterfaceTypeDefinition interfaceType, GraphQLFieldDefinition field)
        {
            foreach (var directive in field.Directives)
            {
                var usage = new InterfaceFieldDirectiveUsage(interfaceType, field, directive);
                InterfaceFieldDirectiveUsageVisited(usage);
            }
        }
    }
}
