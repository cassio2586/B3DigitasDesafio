using CashFlow.Architecture.Core.CashAggregate;
using CashFlow.Architecture.Core.Services;
using CashFlow.Architecture.SharedKernel.Interfaces;
using Castle.Core.Logging;
using Clean.Architecture.Core.ContributorAggregate;
using Clean.Architecture.Core.Services;
using Clean.Architecture.SharedKernel.Interfaces;
using MediatR;
using Moq;
using Xunit;

namespace Clean.Architecture.UnitTests.Core.Services
{
    public class DeleteContributorService_DeleteContributor
    {
        private readonly Mock<IRepository<Cash>> _mockRepo = new Mock<IRepository<Cash>>();
        private readonly Mock<IMediator> _mockMediator = new Mock<IMediator>();
        private readonly Mock<ILogger> _mockIlogger = new Mock<ILogger>();
        private readonly CreateCashService _service;

        public DeleteContributorService_DeleteContributor()
        {
            _service = new CreateCashService(_mockRepo.Object, _mockMediator.Object,_mockIlogger);
        }

        [Fact]
        public async Task ReturnsNotFoundGivenCantFindContributor()
        {
            var result = await _service.DeleteContributor(0);

            Assert.Equal(Ardalis.Result.ResultStatus.NotFound, result.Status);
        }
    }
}
