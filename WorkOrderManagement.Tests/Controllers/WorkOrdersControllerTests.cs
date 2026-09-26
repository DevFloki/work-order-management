using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualBasic;
using Moq;
using WorkOrderManagement.Api.Controllers;
using WorkOrderManagement.Api.Models;
using WorkOrderManagement.Api.Services;

namespace WorkOrderManagement.Tests.Controllers
{
    public class WorkOrdersControllerTests
    {
        [Fact]
        public async Task GetById_WhenWorkOrderDoesNotExist_ReturnsNotFound()
        {
            // Arrange
            var serviceMock = new Mock<IWorkOrderService>();

            serviceMock
                .Setup(s => s.GetByIdAsync(999))
                .ReturnsAsync((WorkOrder?)null);

            var controller = new WorkOrdersController(serviceMock.Object);

            // Act
            var result = await controller.GetById(999);

            // Assert
            Assert.IsType<NotFoundResult>(result.Result);

        }

    }
}
