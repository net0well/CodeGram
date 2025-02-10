﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodeGram.Data.Models
{
    public class Comment
    {

        [Key]
        public int Id { get; set; }
        public string Content { get; set; }
        public DateTime DateCreated { get; set; }
        public DateTime DateUpdated { get; set; }

        //foreing keys
        public int PostId { get; set; }
        public int UserId { get; set; }

        //navigation properties

        public Post Post { get; set; }
        public User User { get; set; }
    }
}
