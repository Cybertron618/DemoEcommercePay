using Elasticsearch.Net;
using Microsoft.Extensions.Configuration;
using Nest;
using System;

namespace DemoEcommercePay.Api.src.Infrastructure.Elasticsearch
{
    public class ElasticSearchHelper : IDisposable
    {
        private readonly ElasticClient _client;

        public ElasticSearchHelper(IConfiguration configuration)
        {
            var uriString = configuration["Elasticsearch:Uri"];
            var indexName = configuration["Elasticsearch:IndexName"];

            if (string.IsNullOrEmpty(uriString) || string.IsNullOrEmpty(indexName))
            {
                throw new ArgumentNullException(nameof(configuration), "Elasticsearch URI or index name is missing in configuration.");
            }

            var settings = new ConnectionSettings(new Uri(uriString)).DefaultIndex(indexName);
            _client = new ElasticClient(settings);
        }

        public async Task<bool> IndexDocumentAsync<TDocument>(TDocument document) where TDocument : class
        {
            var response = await _client.IndexDocumentAsync(document);
            return response.IsValid;
        }

        public async Task<bool> UpdateDocumentAsync<TDocument>(TDocument document, DocumentPath<TDocument> documentPath) where TDocument : class
        {
            var response = await _client.UpdateAsync(documentPath, u => u.Doc(document));
            return response.IsValid;
        }

        public async Task<bool> DeleteDocumentAsync<TDocument>(DocumentPath<TDocument> documentPath) where TDocument : class
        {
            var response = await _client.DeleteAsync(documentPath);
            return response.IsValid;
        }

        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }
    }
}
