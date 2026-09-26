using Castle.Components.DictionaryAdapter.Xml;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
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

        [Fact]
        public async Task GetById_WhenWorkOrderExists_ReturnsOkWithWorkOrder()
        {
            var workOrder = new WorkOrder
            {
                Id = 1,
                Title = "Fix pump",
                Description = "Pump is leaking",
                Status = "Open",
                Priority = "High",
                AssetId = 2
            };

            var serviceMock = new Mock<IWorkOrderService>();

            serviceMock
                .Setup(s => s.GetByIdAsync(1))
                .ReturnsAsync(workOrder);

            var controller = new WorkOrdersController(serviceMock.Object);

            var result = await controller.GetById(1);

            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var model = Assert.IsType<WorkOrderResponse>(okResult.Value);
            Assert.Equal(workOrder.Id, model.Id);
            Assert.Equal(workOrder.Title, model.Title);
        }

    }
}
