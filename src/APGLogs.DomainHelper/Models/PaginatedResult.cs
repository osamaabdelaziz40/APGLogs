using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace APGLogs.DomainHelper.Models
{
    public class PaginatedResult<T>
    {
        public List<T> Records { get; set; }
        public int Total { get; set; }
        public bool HasNext { get; set; }
    }

    public class PaginatedResultWithExport<T> : PaginatedResult<T>
    {
        [JsonIgnore]
        public byte[] PDFFile { get; set; }
    }
}
