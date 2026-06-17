using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.DTOs.BookDTOs
{
    public class BookCreateUpdateDTO
    {
        [Required(ErrorMessage = "Book title is required.")]
        [StringLength(100, ErrorMessage = "Title cannot exceed 100 characters.")]
        public string Title { get; set; } = string.Empty;

        [Required(ErrorMessage = "Author name is required.")]
        
        public string Author { get; set; } = string.Empty;

        [Required(ErrorMessage = "Price is required.")]
        [Range(0.01, 10000.00, ErrorMessage = "Price must be between 0.01 and 10,000.00.")]
        public decimal Price { get; set; }

        [Required(ErrorMessage = "Stock quantity is required.")]
       
        public int StockQuantity { get; set; }

        [StringLength(599, ErrorMessage = "Description cannot exceed 500 characters.")]
        public string Description { get; set; } = string.Empty;

        public string CoverImageUrl { get; set; } = string.Empty;

        [Required(ErrorMessage = "Category ID is required.")]
        public int CategoryId { get; set; }
    }
}
