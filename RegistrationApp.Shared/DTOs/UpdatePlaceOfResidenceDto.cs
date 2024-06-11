using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RegistrationApp.Shared.DTOs
{
    public class UpdatePlaceOfResidenceDto
    {
        public string? City { get; set; }
        public string? Street { get; set; }
        public int? HouseNumber { get; set; }
        public int? AppartmentNumber { get; set; }
    }
}
