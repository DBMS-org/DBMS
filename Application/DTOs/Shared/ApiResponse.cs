using System;
using System.Collections.Generic;

namespace Application.DTOs.Shared
{
    public class ApiResponse
    {
        public int StatusCode { get; set; }
        public string Message { get; set; }
        public bool Success { get; set; }
        public DateTime Timestamp { get; set; } = DateTime.UtcNow;
        public string Version { get; set; } = "1.0.0"; 

        public ApiResponse(bool success, string message, int statusCode)
        {
            Success = success;
            Message = message;
            StatusCode = statusCode;
        }
    }

    public class ApiResponse<T> : ApiResponse
    {
        public T Data { get; set; }

        public ApiResponse(T data, bool success, string message, int statusCode)
            : base(success, message, statusCode)
        {
            Data = data;
        }
    }

    public class PagedApiResponse<T> : ApiResponse<IEnumerable<T>>
    {
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
        public int TotalPages { get; set; }
        public int TotalRecords { get; set; }

        public PagedApiResponse(IEnumerable<T> data, int pageNumber, int pageSize, int totalRecords, bool success, string message, int statusCode)
            : base(data, success, message, statusCode)
        {
            PageNumber = pageNumber;
            PageSize = pageSize;
            TotalRecords = totalRecords;
            TotalPages = (int)Math.Ceiling(totalRecords / (double)pageSize);
        }
    }
} 
