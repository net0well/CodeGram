﻿using CodeGram.Data.Helpers.Enums;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodeGram.Data.Services
{
    public interface IFilesService
    {
        Task<String> UploadImageAsync(IFormFile file, ImageFileType imageFileType);
    }
}
