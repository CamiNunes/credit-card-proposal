using Marraia.Queue.Interfaces;
using Microsoft.AspNetCore.Mvc;
using ProposalCreditCard.API.DTOs;
using ProposalCreditCard.Application.UseCase.RegisterCustomer.Event;

namespace ProposalCreditCard.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CustomersController : ControllerBase
    {
        private readonly IEventBus _eventBus;
        private readonly ILogger<CustomersController> _logger;

        public CustomersController(IEventBus eventBus, ILogger<CustomersController> logger)
        {
            _eventBus = eventBus;
            _logger = logger;
        }

        [HttpPost("register")]
        public async Task<IActionResult> RegisterCustomer([FromBody] CustomerDto customerDto)
        {
            try
            {
                var customerEvent = new CustomerEvent
                {
                    Name = customerDto.Name,
                    Salary = customerDto.Salary,
                    CreditValue = customerDto.CreditValue,
                    Decision = "PENDENTE", // Defina o estado inicial
                    Simulation = null // Pode ser inicializado com null, dependendo da lógica de negócio
                };

                _logger.LogInformation("Publicando evento de registro de cliente.");

                // Publica o evento para o Consumer.Worker
                //await _eventBus.PublishAsync(customerEvent, "messagebus.customer.eventhandler").ConfigureAwait(false);
                await _eventBus.PublishAsync(customerEvent, "messagebus.proposalCreditCard").ConfigureAwait(false);

                _logger.LogInformation("Evento publicado com sucesso.");

                return Ok("Cliente registrado com sucesso.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao registrar cliente.");
                return StatusCode(500, $"Erro ao registrar cliente: {ex.Message}");
            }
        }
    }
}
