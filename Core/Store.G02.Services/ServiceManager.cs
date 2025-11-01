using AutoMapper;
using Store.G02.Domain.Contracts;
using Store.G02.Services.Abstractions;
using Store.G02.Services.Abstractions.Baskets;
using Store.G02.Services.Abstractions.Products;
using Store.G02.Services.Baskets;
using Store.G02.Services.Products;


namespace Store.G02.Services
{
    public class ServiceManager(IUnitOfWork _unitOfWork, IMapper _mapper, IBasketRepository _basketRepository) : IServiceManager
    {
        public IProductService ProductService { get; } = new ProductService(_unitOfWork, _mapper);

        public IBasketService BasketService { get; } = new BasketService(_basketRepository, _mapper);
    }
}
