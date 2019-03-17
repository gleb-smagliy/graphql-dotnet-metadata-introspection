using System;
using GraphQL.Utilities;
using Microsoft.Extensions.DependencyInjection;

namespace GraphQL.MetadataIntrospection
{
    /// <summary>
    /// Provides extensions to register schema with metadata using <see cref="IServiceCollection"/>
    /// </summary>
    public static class ServiceCollectionExtensions
    {
        /// <summary>
        /// Adds GraphQL schema from SDL string to IServiceCollection.
        /// Schema already has metadata query (by default on "_metadata" top-level field), which exposes all metadata found in schema.
        /// By default uses IServiceProvider.GetRequiredService as DependencyResolver for built schema. Can be overriden with configureBuilder action
        /// </summary>
        /// <param name="serviceCollection">Service collection to add schema to</param>
        /// <param name="typeDefinitions">Schema defined by SDL. Could contain metadata directives</param>
        /// <param name="configureBuilder">Graphql.Utilities.SchemaBuilder configuration</param>
        /// <param name="configureIntrospection">Allows you to customize some MetadataEnrichedSchema parameters</param>
        /// <returns>GraphQL schema object with built-in metadata query</returns>
        public static IServiceCollection AddSingletonSchemaWithMetadata(
            this IServiceCollection serviceCollection, 
            string typeDefinitions,
            Action<SchemaBuilder> configureBuilder = null,
            Action<IntrospectionConfiguration> configureIntrospection = null)
        {
            return serviceCollection.AddSingletonSchemaWithMetadata(new [] {typeDefinitions}, configureBuilder, configureIntrospection);
        }

        /// <summary>
        /// Adds GraphQL schema from multiple SDL strings to IServiceCollection.
        /// Schema already has metadata query (by default on "_metadata" top-level field), which exposes all metadata found in schema.
        /// By default uses IServiceProvider.GetRequiredService as DependencyResolver for built schema. Can be overriden with configureBuilder action
        /// </summary>
        /// <param name="serviceCollection">Service collection to add schema to</param>
        /// <param name="typeDefinitions">Schema defined by SDL. Could contain metadata directives</param>
        /// <param name="configureBuilder">Graphql.Utilities.SchemaBuilder configuration</param>
        /// <param name="configureIntrospection">Allows you to customize some enrichment parameters</param>
        /// <returns>GraphQL schema object with built-in metadata query</returns>
        public static IServiceCollection AddSingletonSchemaWithMetadata(
            this IServiceCollection serviceCollection, 
            string[] typeDefinitions,
            Action<SchemaBuilder> configureBuilder = null,
            Action<IntrospectionConfiguration> configureIntrospection = null)
        {
            AddServices(serviceCollection);

            serviceCollection.AddSingleton(s =>
                MetadataIntrospectableSchema.For(
                    typeDefinitions,
                    builder =>
                    {
                        builder.DependencyResolver = new FuncDependencyResolver(s.GetService);
                        configureBuilder?.Invoke(builder);
                    },
                    configureIntrospection
                ));
            
            return serviceCollection;
        }

        private static void AddServices(IServiceCollection serviceCollection)
        {
            serviceCollection.AddSingleton<Types.Metadata>();
            serviceCollection.AddSingleton<Types.MetadataArgument>();
            serviceCollection.AddSingleton<Types.MetadataLocation>();
        }
    }
}