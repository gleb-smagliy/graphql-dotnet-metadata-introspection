using System;
using System.Collections.Generic;
using GraphQL.MetadataIntrospection.Model;
using GraphQL.MetadataIntrospection.Schema;
using GraphQL.Types;
using GraphQL.Utilities;

namespace GraphQL.MetadataIntrospection
{
    /// <summary>
    /// Contains methods similar to Graphql.Schema class.
    /// Allows you to create and enrich schema defined with SDL with metadata  
    /// </summary>
    public static class MetadataIntrospectableSchema
    {
        private static IEnumerable<Metadata> ExtractFrom(string typeDefinitions, IntrospectionConfiguration configuration)
        {
            var filter = new BasicDirectivesFilter(configuration.IncludedDirectives.ToArray(), configuration.ExcludedDirectives.ToArray());
            var extractor = new MetadataExtractor(filter);

            var ast = SchemaParser.ParseSchema(typeDefinitions);
            var metadata = extractor.ExtractMetadata(ast);

            return metadata;
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


        /// <summary>
        /// Builds GraphQL schema from SDL string. Already with metadata query, which exposes all metadata found in schema.
        /// </summary>
        /// <param name="typeDefinitions">Schema defined by SDL. Could contain metadata directives</param>
        /// <param name="configure">Graphql.Utilities.SchemaBuilder configuration</param>
        /// <param name="metadataEnrichmentConfiguration">Allows you to customize some MetadataEnrichedSchema parameters</param>
        /// <returns>GraphQL schema object with built-in metadata query</returns>
        public static ISchema For(string typeDefinitions, Action<SchemaBuilder> configure = null, Action<IntrospectionConfiguration> configureEnrichment = null)
        {
            return For(new [] {typeDefinitions}, configure);
        }

        /// <summary>
        /// Builds GraphQL schema from multiple SDL strings. Already with metadata query, which exposes all metadata found in schema.
        /// </summary>
        /// <param name="typeDefinitions">Schema defined by SDL. Could contain metadata directives</param>
        /// <param name="configureBuild">Graphql.Utilities.SchemaBuilder configuration</param>
        /// <param name="configureEnrichment">Allows you to customize some enrichment parameters</param>
        /// <returns>GraphQL schema object with built-in metadata query</returns>
        public static ISchema For(string[] typeDefinitions, Action<SchemaBuilder> configureBuild = null, Action<IntrospectionConfiguration> configureEnrichment = null)
        {
            var schemaDefinition = string.Join("\n", typeDefinitions);
            var schema = GraphQL.Types.Schema.For(schemaDefinition, configureBuild);
            var enrichment = CreateEnrichment(configureEnrichment);

            var metadata = ExtractFrom(schemaDefinition, enrichment);
            var metadataSchemaEnchancer = new MetadataSchemaEnchancer(metadata);

            return metadataSchemaEnchancer.MutateSchema(schema, enrichment.FieldName);
        }
    }
}
