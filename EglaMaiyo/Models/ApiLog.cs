using System;

namespace EglaMaiyo.Models
{
    public class ApiLog
    {
        public DateTime Timestamp { get; set; }
        public string Endpoint { get; set; } = string.Empty;
        public string Status { get; set; } = string.Empty;
        public string Message { get; set; } = string.Empty;
        public string Response { get; set; } = string.Empty;
        public string Source { get; set; } = string.Empty;
    }
}
