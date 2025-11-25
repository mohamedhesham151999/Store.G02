using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using Store.G02.Domain.Contracts;
using Store.G02.Domain.Entities.Identity;
using Store.G02.Services.Abstractions;
using Store.G02.Services.Abstractions.Auth;
using Store.G02.Services.Abstractions.Baskets;
using Store.G02.Services.Abstractions.Cashe;
using Store.G02.Services.Abstractions.Orders;
using Store.G02.Services.Abstractions.Products;
using Store.G02.Services.Auth;
using Store.G02.Services.Baskets;
using Store.G02.Services.Cashe;
using Store.G02.Services.Orders;
using Store.G02.Services.Products;
using Store.G02.Shared;


namespace Store.G02.Services
{
    public class ServiceManager(IUnitOfWork _unitOfWork,
                                IMapper _mapper,
                                IBasketRepository _basketRepository,
                                ICasheRepository _cashRepository,
                                UserManager<AppUser> _userManager,
                                IOptions<JwtOptions> options) : IServiceManager
    {
        public IProductService ProductService { get; } = new ProductService(_unitOfWork, _mapper);

        public IBasketService BasketService { get; } = new BasketService(_basketRepository, _mapper);

        public ICasheService CasheService { get; } = new CasheService(_cashRepository);

        public IAuthService AuthService { get; } = new AuthService(_userManager, options, _mapper);

        public IOrderService orderService { get; } = new OrderService(_unitOfWork, _mapper , _basketRepository);
    }
}
