using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using API.Dtos;
using API.Errors;
using Core.Entities;
using Core.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Stripe;
using Order = Core.Entities.OrderAggregate.Order;

namespace API.Controllers
{
    public class PaymentsController : BaseApiController
    {
        private readonly IPaymentService paymentService;
        private readonly ILogger<PaymentsController> logger;

        public PaymentsController(IPaymentService paymentService, ILogger<PaymentsController> logger)
        {
            this.paymentService = paymentService;
            this.logger = logger;
        }

        [Authorize]
        [HttpPost("{basketId}")]
        public async Task<ActionResult<CustomerBasket>> CreateOrUpdatePaymentIntent(string basketId)
        {
            var basket = await paymentService.CreateOrUpdatePaymentIntent(basketId);

            if(basket == null) return BadRequest(new ApiResponse(400, "Problems with your basket"));

            return Ok(basket);
        }

        [HttpPost("webhook")]
        public async Task<ActionResult> StripeWebhook()
        {
            var json = await new StreamReader(HttpContext.Request.Body).ReadToEndAsync();
            const string endpointSecret = "";

            var stripeEvent = EventUtility.ParseEvent(json);
            var signatureHeader = Request.Headers["Stripe-Signature"];

            stripeEvent = EventUtility.ConstructEvent(json, signatureHeader, endpointSecret);

            PaymentIntent intent;
            Order order;

            if(stripeEvent.Type == Events.PaymentIntentSucceeded)
            {
                intent = (PaymentIntent) stripeEvent.Data.Object;
                logger.LogInformation("Payment succeeded", intent.Id);
                order = await paymentService.UpdateOrderPaymentSucceeded(intent.Id);
                logger.LogInformation("Order updated to payment received ", order.Id); 

            }
            else if(stripeEvent.Type == Events.PaymentIntentPaymentFailed)
            {
                intent = (PaymentIntent) stripeEvent.Data.Object;
                logger.LogInformation("Payment Failed", intent.Id);
                order = await paymentService.UpdateOrderPaymentFailed(intent.Id);
                logger.LogInformation("Payment failed ", order.Id);
            }

            return Ok();
        }
    }
}