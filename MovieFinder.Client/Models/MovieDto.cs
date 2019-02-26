﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MovieFinder.Client.Models
{
    public class MovieDto
    {
        public string ID { get; set; }
        public string Title { get; set; }
        public int Year { get; set; }
        public string Type { get; set; }
        public string Poster { get; set; }
    }
}