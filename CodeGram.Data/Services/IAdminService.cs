﻿using CodeGram.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodeGram.Data.Services
{
    public interface IAdminService
    {
        Task<List<Post>> GetReportedPostsAsync();
        Task ApproveReport(int postId);
        Task RejectReport(int postId);
    }
}
