﻿namespace TransportManagement.Models.User
{
    public class RegisterViewModel
    {
        public string Email { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string PhoneNumber { get; set; }
        public string Address { get; set; }
        public int Experience { get; set; }
        public DateTime DateOfBirth { get; set; }
    }
}
