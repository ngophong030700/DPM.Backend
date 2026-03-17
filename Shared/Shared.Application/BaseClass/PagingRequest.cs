using Microsoft.AspNetCore.Mvc;
using Shared.Domain.Enum;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.Design.Serialization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.Application.BaseClass
{
    public class PagingRequest
    {
        /// <summary>
        /// Trang hiện tại (1-based)
        /// </summary>
        public int? PageNumber { get; set; } = 1;

        /// <summary>
        /// Số bản ghi trên mỗi trang
        /// </summary>
        public int? PageSize { get; set; } = 20;

        /// <summary>
        /// Tùy chọn sắp xếp theo property, ví dụ "name" hoặc "created_at"
        /// </summary>
        public string? SortBy { get; set; }

        /// <summary>
        /// Sắp xếp tăng/giảm, mặc định tăng
        /// </summary>
        public SortDirectionEnum? SortDirection { get; set; }

        /// <summary>
        /// Từ khóa
        /// </summary>
        public string? Keyword { get; set; }

        /// <summary>
        /// Từ ngày
        /// </summary>
        public DateTime? FromDate { get; set; }

        /// <summary>
        /// Đến ngày
        /// </summary>
        public DateTime? ToDate { get; set; }
    }
}
