﻿namespace Forum_System.Models.QueryParameters
{
    public class UserQueryParameters
    {
        public string? Username { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public bool? IsBlocked { get; set; }
        public bool? IsAdmin { get; set; }
        public string? SortBy { get; set; }
        public string? SortOrder { get; set; }
        public string? Email { get; set; }
        public string? Role { get; set; }
        public int PageNumber { get; set; } = 1;
        public int PageSize { get; set; } = 15;
    }
}
