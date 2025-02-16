﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodeGram.Data.Models
{
    public class Story
    {
        
        public int Id { get; set; }
        public string? ImageUrl { get; set; }
        public DateTime DateCreated { get; set; }
        public bool IsDeleted { get; set; }

        //Foreing Key

        public int UserId { get; set; }

        //Navigation properties

        public User User { get; set; }

    }
}
