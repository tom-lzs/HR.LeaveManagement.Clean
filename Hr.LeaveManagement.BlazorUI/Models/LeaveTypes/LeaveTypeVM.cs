﻿using System.ComponentModel.DataAnnotations;

namespace Hr.LeaveManagement.BlazorUI.Models.LeaveTypes
{
    public class LeaveTypeVM
    {
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        [Display(Name = "Default number of Days")]
        public int DefaultDays { get; set; }

    }
}
