using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.Application.BaseClass
{
    public class PagingResponse<T>
    {
        /// <summary>
        /// Dữ liệu trong trang hiện tại
        /// </summary>
        public List<T> Items { get; set; } = new List<T>();

        /// <summary>
        /// Trang hiện tại
        /// </summary>
        public int PageNumber { get; set; }

        /// <summary>
        /// Số bản ghi trên mỗi trang
        /// </summary>
        public int PageSize { get; set; }

        /// <summary>
        /// Tổng số bản ghi
        /// </summary>
        public int TotalItems { get; set; }

        /// <summary>
        /// Tổng số trang
        /// </summary>
        public int TotalPages
            => (int)Math.Ceiling((double)TotalItems / PageSize);
    }

    public class PagingSparePartResponse<T> : PagingResponse<T>
    {
        /// <summary>
        /// Sắp hết hàng
        /// </summary>
        public int? LowStockParts { get; set; }

        /// <summary>
        /// Hết hàng
        /// </summary>
        public int? OutOfStockParts { get; set; }

        /// <summary>
        /// Tổng giá trị
        /// </summary>
        public decimal? TotalValue { get; set; }

        /// <summary>
        /// Tổng phụ tùng (Nhập/Xuất)
        /// </summary>
        public int? TotalQuantity { get; set; }

        /// <summary>
        /// Phiếu đang chờ xử lý
        /// </summary>
        public int? PendingItems { get; set; }

        /// <summary>
        /// Phiếu đã hoàn thành
        /// </summary>
        public int? CompletedItems { get; set; }

        /// <summary>
        /// Tổng nhập
        /// </summary>
        public int? TotalImported { get; set; }
        
        /// <summary>
        /// Tổng xuất
        /// </summary>
        public int? TotalExported { get; set; }
    }

    public class PagingReportDistributionResponse<T> : PagingResponse<T>
    {
        public int? StatusGood { get; set; }
        public int? StatusAverage { get; set; }
        public int? StatusPoor { get; set; }
        public int? StatusBroken { get; set; }
        public int? OwnerVietGiang { get; set; }
        public int? OwnerBorrowed { get; set; }
    }
}
