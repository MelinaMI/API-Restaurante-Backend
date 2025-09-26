﻿using System.ComponentModel.DataAnnotations;

namespace Application.Models.Request
{
    public class Delivery
    {
        [Required]
        public int Id { get; set; }
        public string To { get; set; }
    }
}
