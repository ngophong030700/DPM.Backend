using System.ComponentModel.DataAnnotations;

namespace Shared.Application.BaseClass
{
    public class EntityResponse<T>
    {
        public T Data { get; set; } = default!;

        [Required]
        public string Message { get; set; } = string.Empty;

        [Required]
        public long Total { get; set; } = 0;

        public EntityResponse() { }

        public EntityResponse(T data, string message, long total = 1)
        {
            Data = data;
            Message = message;
            Total = total;
        }
    }

    public class EntitySparePartResponse<T> : EntityResponse<T>
    {
        /// <summary>
        /// Sắp hết hàng
        /// </summary>
        [Required]
        public int? LowStockParts { get; set; } = 0;

        /// <summary>
        /// Hết hàng
        /// </summary>
        [Required]
        public int? OutOfStockParts { get; set; } = 0;

        /// <summary>
        /// Tổng giá trị
        /// </summary>
        [Required]
        public decimal? TotalValue { get; set; } = 0;

        public EntitySparePartResponse() { }

        public EntitySparePartResponse(
            T data,
            string message,
            long total,
            int? lowStockParts,
            int? outOfStockParts,
            decimal? totalValue
        ) : base(data, message, total)
        {
            LowStockParts = lowStockParts;
            OutOfStockParts = outOfStockParts;
            TotalValue = totalValue;
        }
    }

    public class EntityStockReceiptResponse<T> : EntityResponse<T>
    {
        /// <summary>
        /// Tổng phụ tùng (Nhập/Xuất)
        /// </summary>
        [Required] 
        public int? TotalQuantity { get; set; } = 0;

        /// <summary>
        /// Phiếu đang chờ xử lý
        /// </summary>
        [Required] 
        public int? PendingItems { get; set; } = 0;

        /// <summary>
        /// Tổng giá trị
        /// </summary>
        [Required] 
        public decimal? TotalValue { get; set; } = 0;

        public EntityStockReceiptResponse() { }

        public EntityStockReceiptResponse(
            T data,
            string message,
            long total,
            int? totalQuantity,
            int? pendingItems,
            decimal? totalValue
        ) : base(data, message, total)
        {
            TotalQuantity = totalQuantity;
            PendingItems = pendingItems;
            TotalValue = totalValue;
        }
    }

    public class EntityStockIssueResponse<T> : EntityResponse<T>
    {
        /// <summary>
        /// Tổng phụ tùng (Nhập/Xuất)
        /// </summary>
        [Required] 
        public int? TotalQuantity { get; set; } = 0;

        /// <summary>
        /// Phiếu đang chờ xử lý
        /// </summary>
        [Required] 
        public int? PendingItems { get; set; } = 0;

        /// <summary>
        /// Phiếu đã hoàn thành
        /// </summary>
        [Required] 
        public int? CompletedItems { get; set; } = 0;

        public EntityStockIssueResponse() { }

        public EntityStockIssueResponse(
            T data,
            string message,
            long total,
            int? totalQuantity,
            int? pendingItems,
            int? completedItems
        ) : base(data, message, total)
        {
            TotalQuantity = totalQuantity;
            PendingItems = pendingItems;
            CompletedItems = completedItems;
        }
    }

    public class EntitySparePartStockResponse<T> : EntityResponse<T>
    {
        /// <summary>
        /// Tổng nhập
        /// </summary>
        [Required] 
        public int? TotalImported { get; set; } = 0;

        /// <summary>
        /// Tổng xuất
        /// </summary>
        [Required] 
        public int? TotalExported { get; set; } = 0;

        /// <summary>
        /// Tổng giá trị
        /// </summary>
        [Required] 
        public decimal? TotalValue { get; set; } = 0;

        public EntitySparePartStockResponse() { }

        public EntitySparePartStockResponse(
            T data,
            string message,
            long total,
            int? totalImported,
            int? totalExported,
            decimal? totalValue
        ) : base(data, message, total)
        {
            TotalImported = totalImported;
            TotalExported = totalExported;
            TotalValue = totalValue;
        }
    }

    public class EntityReportDistributionResponse<T> : EntityResponse<T>
    {
        public int? StatusGood { get; set; }
        public int? StatusAverage { get; set; }
        public int? StatusPoor { get; set; }
        public int? StatusBroken { get; set; }
        public int? OwnerVietGiang { get; set; }
        public int? OwnerBorrowed { get; set; }

        public EntityReportDistributionResponse() { }

        public EntityReportDistributionResponse(
            T data,
            string message,
            long total,
            int? statusGood,
            int? statusAverage,
            int? statusPoor,
            int? statusBroken,
            int? ownerVietGiang,
            int? ownerBorrowed
        ) : base(data, message, total)
        {
            StatusGood = statusGood;
            StatusAverage = statusAverage;
            StatusPoor = statusPoor;
            StatusBroken = statusBroken;
            OwnerVietGiang = ownerVietGiang;
            OwnerBorrowed = ownerBorrowed;
        }
    }

    public class ErrorResponse
    {
        [Required]
        public string CodeMessage { get; set; } = string.Empty;

        [Required]
        public ErrorDetail Message { get; set; } = new ErrorDetail();

        public ErrorResponse() { }

        public ErrorResponse(string codeMessage, string detail, string? exceptionMessage = null)
        {
            CodeMessage = codeMessage;
            Message = new ErrorDetail(detail, exceptionMessage);
        }

        public class ErrorDetail
        {
            [Required]
            public string Detail { get; set; } = string.Empty;

            // Log exception message để debug nhanh
            public string? ExceptionMessage { get; set; }

            public ErrorDetail() { }

            public ErrorDetail(string detail, string? exceptionMessage = null)
            {
                Detail = detail;
                ExceptionMessage = exceptionMessage;
            }
        }
    }
}
