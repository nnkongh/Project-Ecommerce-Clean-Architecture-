using AutoMapper;
using Ecommerce.Web.Interface;
using Ecommerce.Web.ViewModels;
using Ecommerce.Web.ViewModels.ApiResponse;

namespace Ecommerce.Web.Services
{
    public class OrderClient : IOrderClient
    {
        private readonly HttpClient _httpClient;
        private readonly IMapper _mapper;

        public OrderClient(IHttpClientFactory httpClient, IMapper mapper)
        {
            _mapper = mapper;
            _httpClient = httpClient.CreateClient("ApiClient");
        }

        public Task<ApiResponse<OrderViewModel>> CreatOrderAsync()
        {
            throw new NotImplementedException();
        }
    }
}
