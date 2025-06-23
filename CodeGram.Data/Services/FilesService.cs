﻿using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using CodeGram.Data.Helpers.Enums;
using CodeGram.Data.Models;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;

namespace CodeGram.Data.Services
{
    public class FilesService : IFilesService
    {
        private readonly BlobServiceClient _blobServiceClient;
        public FilesService(string connectionString)
        {
            _blobServiceClient = new BlobServiceClient(connectionString);
        }
        public async Task<string> UploadImageAsync(IFormFile file, ImageFileType imageFileType)
        {
            string containerName = imageFileType switch
            {
                ImageFileType.PostImage => "posts",
                ImageFileType.StoryImage => "stories",
                ImageFileType.ProfilePicture => "profiles",
                ImageFileType.CoverImage => "covers",
                _ => throw new ArgumentException("Invalid file type")
            };

            if(file == null || file.Length == 0)
            {
                return "";
            }

            //ensure the container exists

            var containerClient = _blobServiceClient.GetBlobContainerClient(containerName);
            await containerClient.CreateIfNotExistsAsync();

            //Generate a unique file name
            string fileName = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);
            var blobClient = containerClient.GetBlobClient(fileName);

            //upload the file

            using(var stream = file.OpenReadStream())
            {
                await blobClient.UploadAsync(stream, new BlobHttpHeaders
                {
                    ContentType = file.ContentType
                });
            }

            return blobClient.Uri.ToString();
        }
    }
}