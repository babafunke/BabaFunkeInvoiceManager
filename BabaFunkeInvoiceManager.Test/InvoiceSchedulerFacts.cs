using BabaFunkeInvoiceManager.Functions;
using BabaFunkeInvoiceManager.Models;
using BabaFunkeInvoiceManager.Services.Interfaces;
using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;
using Moq;
using System.IO;
using System.Threading.Tasks;
using Xunit;

namespace BabaFunkeInvoiceManager.Test
{
    public class InvoiceSchedulerFacts
    {
        private readonly InvoiceScheduler _sut;
        private readonly Mock<IBlobManager> _blobManager;
        private readonly Mock<IEmailService> _emailService;
        private readonly Mock<IInvoiceService> _invoiceService;
        private readonly Mock<TimerInfo> _timer;
        private readonly Mock<ILogger> _logger;
        private readonly BlobDetail blobDetail;

        public InvoiceSchedulerFacts()
        {
            _blobManager = new Mock<IBlobManager>();
            _emailService = new Mock<IEmailService>();
            _invoiceService = new Mock<IInvoiceService>();
            _timer = new Mock<TimerInfo>(null, null, false);
            _logger = new Mock<ILogger>();
            _sut = new InvoiceScheduler(_blobManager.Object, _emailService.Object, _invoiceService.Object);

            blobDetail = new BlobDetail(new MemoryStream(), "", "", "");
        }


        [Fact]
        public async Task GivenARequestToScheduleInvoice_ShouldCallTheEmailServiceOnceIfBlobDetailIsNotNull()
        {
            _blobManager.Setup(b => b.GetBlob(It.IsAny<string>())).ReturnsAsync(blobDetail);

            await _sut.Run(_timer.Object, _logger.Object);

            _emailService.Verify(e => e.SendEmail(blobDetail), Times.Once);
        }

        [Fact]
        public async Task GivenARequestToScheduleInvoice_ShouldNotCallTheEmailServiceIfBlobDetailIsNull()
        {
            _blobManager.Setup(b => b.GetBlob(It.IsAny<string>())).ReturnsAsync(() => null);

            await _sut.Run(_timer.Object, _logger.Object);

            _emailService.Verify(e => e.SendEmail(null), Times.Never);
        }

        [Fact]
        public async Task GivenARequestToScheduleInvoice_ShouldCallTheInvoiceServiceOnceIfBlobDetailIsNotNull()
        {
            _blobManager.Setup(b => b.GetBlob(It.IsAny<string>())).ReturnsAsync(blobDetail);

            _emailService.Setup(b => b.SendEmail(blobDetail)).ReturnsAsync(true);

            await _sut.Run(_timer.Object, _logger.Object);

            _invoiceService.Verify(i => i.AddInvoice(It.IsAny<Invoice>()), Times.Once);
        }

        [Fact]
        public async Task GivenARequestToScheduleInvoice_ShouldNotCallTheInvoiceServiceIfEmailServiceReturnsFalse()
        {
            _blobManager.Setup(b => b.GetBlob(It.IsAny<string>())).ReturnsAsync(blobDetail);

            _emailService.Setup(b => b.SendEmail(blobDetail)).ReturnsAsync(false);

            await _sut.Run(_timer.Object, _logger.Object);

            _invoiceService.Verify(i => i.AddInvoice(It.IsAny<Invoice>()), Times.Never);
        }
    }
}