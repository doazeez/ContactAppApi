using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DatabaseAndModel.DTO
{
    public class AddImageDTO
    {
        public IFormFile Image { get; set; }
    }
}
