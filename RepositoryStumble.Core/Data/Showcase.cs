using System;
using System.Collections.Generic;

namespace RepositoryStumble.Core.Data
{
    public class Showcase
    {
        public string Name { get; set; }
        public string Slug { get; set; }
        public string Description { get; set; }
    }

    public class ShowcaseRepositories
    {
        public string Name { get; set; }
        public string Slug { get; set; }
        public string Description { get; set; }
        public List<ShowcaseRepository> Repositories { get; set; } 
    }

    public class ShowcaseRepository
    {
        public string Url { get; set; }
        public string Owner { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int Stars { get; set; }
        public int Forks { get; set; }
    }

}

