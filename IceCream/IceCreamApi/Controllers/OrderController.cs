using IceCreamApi.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IceCreamApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderController : ControllerBase
    {
        projectContext db = new projectContext();

        [HttpGet("AllOrder")]
        public async Task<ActionResult> ViewAll()
        {
            var foundOrders = await db.BookOrders.OrderByDescending(c => c.Id).ToListAsync();

            if (foundOrders == null)
            {
                return NotFound();
            }

            return Ok(foundOrders);
        }
        [HttpGet("GetOrder/{id}")]
        public async Task<ActionResult> GetOrder(int id)
        {
            var foundOrder = await db.BookOrders.Where(o => o.Id == id).SingleOrDefaultAsync();

            if (foundOrder == null)
            {
                return NotFound();
            }

            return Ok(foundOrder);
        }

        [HttpGet("OrderDetail/{id}")]
        public async Task<ActionResult> ViewAllDetail(int id)
        {
            var foundDetails = await db.BookOrderDetails.Where(o => o.BookOrderId == id).ToListAsync();

            if (foundDetails == null)
            {
                return NotFound();
            }
            List<OrderDetailService> detailServices = new List<OrderDetailService>();
            foreach (BookOrderDetail detail in foundDetails)
            {
                BookIceCream book = await db.BookIceCreams.SingleOrDefaultAsync(c => c.BookId == detail.BookId);
                OrderDetailService orderDetailService = new OrderDetailService();
                orderDetailService.BookIceCream = book;
                orderDetailService.Id = detail.Id;
                orderDetailService.Quantity = detail.Quantity;
                orderDetailService.Price = detail.Price;
                orderDetailService.BookOrderId = detail.BookOrderId;
                orderDetailService.BookOrderId = detail.BookOrderId;
                detailServices.Add(orderDetailService);
            }
            return Ok(detailServices);
        }


        [HttpPost]
        public async Task<ActionResult> AddOrder(BookOrder order)
        {
            if (order == null)
            {
                return NotFound();
            }
            order.IsCompleted = false;
            db.BookOrders.Add(order);
            await db.SaveChangesAsync();
            var neworder = await db.BookOrders.SingleOrDefaultAsync(o => o == order);
            return Ok(neworder.Id);
        }

        [HttpPost("Detail")]
        public async Task<IActionResult> AddDetail(List<BookOrderDetail> details)
        {
            if (details == null)
            {
                return NotFound();
            }
            foreach (var detail in details)
            {
                db.BookOrderDetails.Add(detail);
                await db.SaveChangesAsync();
            }
            return Ok();
        }

        [HttpGet("Completed/{id}")]
        public async Task<ActionResult> Completed(int id)
        {
            var foundOrder = await db.BookOrders.SingleOrDefaultAsync(m => m.Id == id);
            if (foundOrder == null)
            {
                return NotFound();
            }

            foundOrder.IsCompleted = true;
            db.BookOrders.Update(foundOrder);
            await db.SaveChangesAsync();
            return Ok(foundOrder);
        }

        [HttpGet("Phone/{phone}")]
        public async Task<IActionResult> DetailByPHone(string phone)
        {
            var foundOrder = await db.BookOrders.Where(m => m.Phone == phone).OrderByDescending(c => c.Id).FirstOrDefaultAsync();
            if (foundOrder == null)
            {
                return NotFound();
            }
            return Ok(foundOrder);
        }
    }
}
