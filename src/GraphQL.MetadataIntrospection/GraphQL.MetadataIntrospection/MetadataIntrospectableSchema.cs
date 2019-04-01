using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using GraphQL.MetadataIntrospection.Model;
using GraphQL.MetadataIntrospection.Schema;
using GraphQL.Types;
using GraphQL.Utilities;

[assembly:InternalsVisibleTo("GraphQL.MetadataIntrospection.Tests")]
namespace GraphQL.MetadataIntrospection
{
    /// <summary>
    /// Contains methods similar to Graphql.Schema class.
    /// Allows you to create and enrich schema defined with SDL with metadata  
    /// </summary>
    public static class MetadataIntrospectableSchema
    {
        /// <summary>
        /// Builds GraphQL schema from multiple SDL strings. Already with metadata query, which exposes all metadata found in schema.
        /// </summary>
        /// <param name="typeDefinitions">Schema defined by SDL. Could contain metadata directives</param>
        /// <param name="configureBuild">Graphql.Utilities.SchemaBuilder configuration</param>
        /// <param name="configureEnrichment">Allows you to customize some enrichment parameters</param>
        /// <returns>GraphQL schema object with built-in metadata query</returns>
        public static ISchema For(string[] typeDefinitions, Action<SchemaBuilder> configureBuild = null,
            Action<IntrospectionConfiguration> configureEnrichment = null)
        {
            return For(string.Join("\n", typeDefinitions), configureBuild, configureEnrichment);
        }


        /// <summary>
        /// Builds GraphQL schema from SDL string. Already with metadata query, which exposes all metadata found in schema.
        /// </summary>
        /// <param name="typeDefinitions">Schema defined by SDL. Could contain metadata directives</param>
        /// <param name="configureBuild">Graphql.Utilities.SchemaBuilder configuration</param>
        /// <param name="configureEnrichment">Allows you to customize some MetadataEnrichedSchema parameters</param>
        /// <returns>GraphQL schema object with built-in metadata query</returns>
        public static ISchema For(string typeDefinitions, Action<SchemaBuilder> configureBuild = null,
            Action<IntrospectionConfiguration> configureEnrichment = null)
        {
            var schema = GraphQL.Types.Schema.For(typeDefinitions, configureBuild);

            var enrichment = CreateEnrichment(configureEnrichment);
            var metadataSchemaEnchanter = new MetadataSchemaEnchanter(ExtractFrom(typeDefinitions, enrichment));

            return metadataSchemaEnchanter.MutateSchema(schema, enrichment.FieldName);
        }

        private static IEnumerable<Metadata> ExtractFrom(string typeDefinitions,IntrospectionConfiguration configuration)
        {
            var filter = new BasicDirectivesFilter(configuration.IncludedDirectives.ToArray(),configuration.ExcludedDirectives.ToArray());
            var ast = SchemaParser.ParseSchema(typeDefinitions);

            return new MetadataExtractor(filter).ExtractMetadata(ast);
        }

        private static IntrospectionConfiguration CreateEnrichment(Action<IntrospectionConfiguration> configure = null)
        {
            var enrichment = new IntrospectionConfiguration
            {
                FieldName = "_metadata"
            };

            configure?.Invoke(enrichment);

            return enrichment;
        }
    }
}