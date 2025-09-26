﻿using System.ComponentModel.DataAnnotations;

namespace Application.Models.Request
{
    public class OrderRequest
    {
        [Required]
        public List<Items> Items { get; set; }
        [Required]
        public Delivery Delivery { get; set; }
        public string Notes { get; set; }
    }
}
