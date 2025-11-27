using AutoMapper;
using Store.G02.Domain.Contracts;
using Store.G02.Domain.Entities.Order;
using Store.G02.Domain.Entities.Products;
using Store.G02.Domain.Exceptionsn.BadRequest;
using Store.G02.Domain.Exceptionsn.NotFound;
using Store.G02.Domain.Exceptionsn.Order;
using Store.G02.Services.Abstractions.Orders;
using Store.G02.Services.Specifications.Orders;
using Store.G02.Shared.Dtos.Orders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Store.G02.Services.Orders
{
    public class OrderService(IUnitOfWork _unitOfWork, IMapper _mapper, IBasketRepository _basketRepository) : IOrderService
    {
        public async Task<OrderResponse?> CreateOrderAsync(OrderRequest request, string userEmail)
        {
            #region Small summary
            // 1. Get Order Address
            // 2. Get DeliveryMethod By Id
            // 3. Get Order Items
            // 3.1 Get basket by id
            // 3.2 Convert every basket item to order item

            // 4. Calculate 
            // 5. Creater order in Database 
            // 6. Add order in Database 
            // 7. Store Db context > Generic Repository > UnitOfWork
            #endregion

            var orderAddress = _mapper.Map<OrderAddress>(request.ShipToAddress);

            var deliveryMethod = await _unitOfWork.GetRepository<int, DeliveryMethod>().GetAsync(request.DeliveryMethodId);
            if(deliveryMethod is null) throw new DeliveryMethodNotFoundException(request.DeliveryMethodId);

            var basket = await _basketRepository.GetBasketAsync(request.BasketId);
            if (basket is null) throw new BasketNotFoundException(request.BasketId);


            var orderItems = new List<OrderItem>();

            foreach (var item in basket.Items)
            {
                // Check price
                // get product from db

                var product = await _unitOfWork.GetRepository<int, Product>().GetAsync(item.Id);
                if (product is null) throw new ProductNotFoundException(item.Id);

                if (product.Price != item.Price) item.Price = product.Price;

                var productInOrderItem = new ProductInOrderItem(item.Id, item.ProductName, item.PictureUrl);
                var orderItem = new OrderItem(productInOrderItem, item.Price, item.Quantity);
                orderItems.Add(orderItem);
            }

            var subTotal =  orderItems.Sum(OI => OI.Price * OI.Quantity);

            var order = new Order(userEmail, orderAddress, deliveryMethod, orderItems, subTotal,basket.PaymentIntentId);

            await _unitOfWork.GetRepository<Guid, Order>().AddAsync(order);

            var count = await _unitOfWork.SaveChangesAsync();

            return _mapper.Map<OrderResponse>(order);
        }

        public async Task<IEnumerable<DeliveryMethodResponse>> GetAllDeliveryMethodAsync()
        {
            var deliveryMethod = await _unitOfWork.GetRepository<int, DeliveryMethod>().GetAllAsync();
            return _mapper.Map<IEnumerable<DeliveryMethodResponse>>(deliveryMethod);
            
        }

        public async Task<OrderResponse?> GetOrderByIdForSpecificUserAsync(Guid id, string userEmail)
        {
            var spec = new OrderSpecification(id, userEmail);
            var order = await _unitOfWork.GetRepository<Guid, Order>().GetAsync(spec);
            if (order is null) throw new OrderNotFoundException(id);
            return _mapper.Map<OrderResponse>(order);
        }

        public async Task<IEnumerable<OrderResponse>> GetOrdersForSpecificUserAsync(string userEmail)
        {
            var spec = new OrderSpecification(userEmail);
            var order = await _unitOfWork.GetRepository<Guid, Order>().GetAllAsync(spec);
            return _mapper.Map<IEnumerable<OrderResponse>> (order);
        }
    }
}
