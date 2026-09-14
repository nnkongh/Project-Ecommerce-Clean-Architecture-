using Ecommerce.Application.Interfaces;
using Ecommerce.Domain.Interfaces;
using Ecommerce.Domain.Interfaces.UnitOfWork;
using Ecommerce.Domain.Models;
using Ecommerce.Infrastructure.Interfaces;
using Google.Apis.Services;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Infrastructure.Services
{
    public class NotificationHubService : INotificationHubService
    {
        private readonly IHubContext<ChatHub> hubContext;
        public NotificationHubService(IHubContext<ChatHub> hubContext)
        {
            this.hubContext = hubContext;
        }
        public async Task SendNotificationToUserAsync(string userId, object message)
        {
            await hubContext.Clients.Groups($"user_{userId}").SendAsync("ReceiveNotification",message);
        }
    }
    public class ChatHub : Hub
    {
        private readonly ILogger<ChatHub> _logger;

        public ChatHub(ILogger<ChatHub> logger)
        {
            _logger = logger;
        }

        public override async Task OnConnectedAsync()
        {
            var userId = Context.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (!string.IsNullOrEmpty(userId))
            {
                await Groups.AddToGroupAsync(Context.ConnectionId, $"user_{userId}");
                _logger.LogInformation("User {UserId} connected to ChatHub", userId);
            }
            else
            {
                _logger.LogWarning("User connected to ChatHub without a valid identity. ConnectionId: {ConnectionId}", Context.ConnectionId);
            }

            await base.OnConnectedAsync();
        }

        public override async Task OnDisconnectedAsync(Exception? exception)
        {
            var userId = Context.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (!string.IsNullOrEmpty(userId))
            {
                await Groups.RemoveFromGroupAsync(Context.ConnectionId, $"user_{userId}");
                _logger.LogInformation("User {UserId} disconnected from ChatHub", userId);
            }
            else
            {
                _logger.LogWarning("Unauthenticated user disconnected from ChatHub. ConnectionId: {ConnectionId}", Context.ConnectionId);
            }

            if (exception != null)
            {
                _logger.LogError(exception, "User {UserId} disconnected with error", userId);
            }

            await base.OnDisconnectedAsync(exception);
        }
    }

    public class NotificationService : INotificationService
    {
        private readonly INotificationHubService _hubService;
        private readonly INotificationRepository _notificationRepository;
        private readonly IUnitOfWork _unitOfWork;

        public NotificationService(INotificationHubService hubService, INotificationRepository notificationRepository, IUnitOfWork unitOfWork)
        {
            _hubService = hubService;
            _notificationRepository = notificationRepository;
            _unitOfWork = unitOfWork;
        }
        public async Task SendNotificationAsync(Notification noti, CancellationToken token)
        {
            await _notificationRepository.AddAsync(noti);
            await _unitOfWork.SaveChangesAsync(token);
            await _hubService.SendNotificationToUserAsync(noti.UserId, new
            {
                noti.Id,
                noti.Title,
                noti.Description,
                noti.CreatedDate,
                noti.IsRead,
            });
        }

      
    }
}

