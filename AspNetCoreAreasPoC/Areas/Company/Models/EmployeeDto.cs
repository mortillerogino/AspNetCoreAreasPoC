using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AspNetCoreAreasPoC.Areas.Company.Models
{
    public class EmployeeDto
    {
        public EmployeeDto()
        {

        }

        public EmployeeDto(Employee employee)
        {
            Id = employee.Id;
            Name = employee.Name;
            ImageBytes = employee.Image;
            
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public byte[] ImageBytes { get; set; }
        public IFormFile Image { get; set; }
        public string ImagePath
        {
            get
            {
                if (ImageBytes != null)
                {
                    var imgString = Convert.ToBase64String(ImageBytes);
                    return string.Format($"data:image/png;base64,{imgString}");
                }

                return "";
            }
        }

    }
}
