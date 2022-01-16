using BabaFunkeInvoiceManager.Entities;
using BabaFunkeInvoiceManager.Models;
using BabaFunkeInvoiceManager.Repository;
using BabaFunkeInvoiceManager.Services;
using Moq;
using System;
using System.Threading.Tasks;
using Xunit;

namespace BabaFunkeInvoiceManager.Test
{
    public class InvoiceServiceFacts
    {
        private readonly InvoiceService _sut;
        private readonly Mock<IInvoiceRepository> _invoiceRepo;

        public InvoiceServiceFacts()
        {
            _invoiceRepo = new Mock<IInvoiceRepository>();
            _sut = new InvoiceService(_invoiceRepo.Object);
        }

        [Fact]
        public async Task AddInvoice_ShouldThrowNullArugumentExceptionIfInvoiceIsNull()
        {
            await Assert.ThrowsAsync<ArgumentNullException>(() => _sut.AddInvoice(null));
        }

        [Fact]
        public async Task AddInvoice_ShouldCallAddInvoiceTableEntityOnceIfInvoiceIsNotNull()
        {
            await _sut.AddInvoice(new Invoice());

            _invoiceRepo.Verify(i => i.AddInvoiceTableEntity(It.IsAny<InvoiceTableEntity>()), Times.Once);
        }
    }
}