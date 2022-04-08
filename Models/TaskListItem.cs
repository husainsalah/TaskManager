using System;
using System.ComponentModel.DataAnnotations;

namespace TaskManager.Models{
    public class TaskListItem{
        
        public int Id {get; set; }
        [Required(ErrorMessage = "A short description is required")]
        [MinLength(2, ErrorMessage = "Title must contain at least two characters!")]
        [MaxLength(1000, ErrorMessage = "Title must contain a maximum of 1000 characters!")]  
        public string Description { get; set; }
        public DateTime DueDate { get; set; }

        [Required(ErrorMessage = "Date is required")]  
        [DataType(DataType.DateTime)]
        public bool Completed {get; set; }
    }
}