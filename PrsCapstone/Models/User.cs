﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace PrsCapstone.Models {
    public class User {

        public int Id { get; set; }
        [StringLength(30)]
        [Required]
        public string FirstName { get; set;}
        [StringLength(30)]
        [Required]
        public string LastName { get; set;}
        [StringLength(32)]
        [Required]
        public string Username { get; set;}
        [StringLength(64)]
        [Required]
        public string Password { get; set;}
        [StringLength(10)]
        public string Phone { get; set;}
        [StringLength(100)]
        public string Email { get; set;}
        public bool IsReviewer { get; set; } = false;
        public bool IsAdmin { get; set; } = false;
        [StringLength(30)]
        public string SecurityAnswer { get; set; }

    }
}
