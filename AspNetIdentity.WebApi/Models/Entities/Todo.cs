using System;
using AspNetIdentity.WebApi.Infrastructure;

namespace AspNetIdentity.WebApi.Models.Entities
{
    public class Todo
    {
        public int Id { get; set; }

        public string Title { get; set; }

        public string Description { get; set; }

        public int Priority { get; set; }

        public string Status { get; set; }

        public string Category { get; set; }

        public DateTime CreatedAt { get; set; }

        public DateTime UpdatedAt { get; set; }

        public ApplicationUser User { get; set; }   
    }
}