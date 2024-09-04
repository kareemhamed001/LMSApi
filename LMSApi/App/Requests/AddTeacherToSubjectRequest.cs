﻿using System.ComponentModel.DataAnnotations;

namespace LMSApi.App.Requests
{
    public class AddTeacherToSubjectRequest
    {
        [Required]
        [Range(1, int.MaxValue)]
        public int TeacherId { get; set; }
        [Required]
        [Range(1, int.MaxValue)]
        public int SubjectId { get; set; }
    }
}