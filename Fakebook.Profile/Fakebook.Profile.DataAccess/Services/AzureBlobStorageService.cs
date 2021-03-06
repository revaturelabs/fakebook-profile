﻿using System;
using System.IO;
using System.Threading.Tasks;

using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;

using Fakebook.Profile.DataAccess.Services.Interfaces;

namespace Fakebook.Profile.DataAccess.Services
{
    /// <summary>
    /// Gets data from/to a NoSQL database hosted on Azure Blob.
    /// </summary>
    public class AzureBlobStorageService : IStorageService
    {
        private readonly BlobServiceClient _client;
        private readonly string _containerName;

        /// <summary>
        /// Construct a storage service to communicate with a NoSQL database on Azure Blob
        /// </summary>
        /// <param name="client"> an instance of BlobClient</param>
        /// <param name="containerName"> name of the storage container associated </param>
        /// <returns></returns>
        public AzureBlobStorageService(BlobServiceClient client, string containerName)
        {
            _client = client;
            _containerName = containerName;
        }

        /// <summary>
        /// an async method used to upload content to a NoSQL database on Azure Blob
        /// </summary>
        /// <param name="content"> content to be uploaded</param>
        /// <param name="contentType"> type of content </param>
        /// <param name="fileName"> name of the file received from frontend </param>
        /// <param name="containerName"> name of the storage container associated </param>
        /// <returns></returns>
        public async Task<Uri> UploadFileContentAsync(Stream content, string contentType, string fileName, string containerName = null)
        {
            if (containerName is not null)
            {
                containerName = _containerName;
            }

            BlobContainerClient client = GetClient(containerName);

            _ = await client.GetBlobClient(fileName).UploadAsync(content, new BlobHttpHeaders { ContentType = contentType });

            return client.Uri;
        }

        /// <summary>
        /// a helper method used to get a BlobClient
        /// </summary>
        private BlobContainerClient GetClient(string containerName)
        {
            BlobContainerClient containerClient = _client.GetBlobContainerClient(containerName);
            return containerClient;
        }
    }
}
