using System;
using System.Collections.Generic;

namespace UsedCars.Models
{
    public class ErrorViewModel
    {
        public string RequestId { get; set; }

        public bool ShowRequestId => !string.IsNullOrEmpty(RequestId);

        public List<string> Error { get; set; }
    }
}
