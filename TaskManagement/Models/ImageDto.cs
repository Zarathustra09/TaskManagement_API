using System;

namespace TaskManagement.Models
{
    public class ImageDto
    {
        public int Id { get; set; }
        public int Task_Id { get; set; }
        public string Image_Path { get; set; }
        public DateTime Uploaded_At { get; set; }
    }
}
