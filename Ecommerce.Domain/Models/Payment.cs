using Ecommerce.Domain.Enum;
using Ecommerce.Domain.Exceptions;
using System;

namespace Ecommerce.Domain.Models
{
    public class Payment
    {
        public int Id { get; private set; }
        public int SubOrderId { get; private set; }
        public SubOrder? SubOrder { get; private set; }
        public decimal Amount { get; private set; }
        public PaymentMethod Method { get; private set; }
        public PaymentStatus Status { get; private set; }
        public string? TransactionId { get; private set; }
        public DateTime? PaidAt { get; private set; }
        public string? Note { get; private set; }

        private Payment() { }

        public static Payment Create(int subOrderId, decimal amount, PaymentMethod method, string? note = null)
        {
            if (amount <= 0) throw new DomainException("Số tiền thanh toán phải lớn hơn 0");

            return new Payment
            {
                SubOrderId = subOrderId,
                Amount = amount,
                Method = method,
                Status = PaymentStatus.Pending,
                Note = note
            };
        }

        public void MarkAsCompleted(string? transactionId = null)
        {
            Status = PaymentStatus.Completed;
            TransactionId = transactionId;
            PaidAt = DateTime.Now;
        }

        public void MarkAsFailed(string? note = null)
        {
            Status = PaymentStatus.Failed;
            Note = note;
        }

        public void MarkAsRefunded(string? note = null)
        {
            Status = PaymentStatus.Refunded;
            Note = note;
        }
    }
}