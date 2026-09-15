// Please see documentation at https://learn.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.

(function () {
  function updateBadge(count) {
    const badge = document.getElementById('notification-badge');
    if (!badge) return;
    if (count > 0) {
      badge.textContent = count > 99 ? '99+' : count;
      badge.style.display = '';
    } else {
      badge.style.display = 'none';
    }
  }

  function loadRecent() {
    const list = document.getElementById('notification-dropdown-list');
    if (!list) return;
    fetch('/Notification/Recent', { headers: { 'Accept': 'text/html' } })
      .then(r => r.text())
      .then(html => {
        if (html && html.trim().length > 0) {
          list.innerHTML = html;
        }
      })
      .catch(() => { });
  }

  function loadNotifications() {
    const badge = document.getElementById('notification-badge');
    if (!badge) return;

    fetch('/Notification/UnreadCount')
      .then(r => r.json())
      .then(data => { if (data && typeof data.count === 'number') updateBadge(data.count); })
      .catch(() => { });

    loadRecent();
  }

    async function getToken() {
        const res = await fetch('Auth/GetAccessToken', { credentials: 'include' });
        if (!res.ok) return null;
        const data = await res.json();
        return data.token;
    }
  function initSignalR() {
    const bell = document.getElementById('notification-nav');
    if (!bell || typeof window.signalR === 'undefined') return;

      const connection = new signalR.HubConnectionBuilder()
          .withUrl('https://localhost:7021/chatHub', { accessTokenFactory: () => getToken() })
      .withAutomaticReconnect()
      .build();

    connection.on('ReceiveNotification', function () {  
      loadNotifications();
    });

    connection.onreconnected(function () {
      loadNotifications();
    });

    connection.onclose(function (err) {
      if (err) console.warn('[notifications] SignalR closed', err);
    });

      connection.start()
          .then(() => console.log('[SignalR] CONNECTED', new Date().toISOString()))
          .catch(err => console.error('[SignalR] FAILED', err));

      connection.on('ReceiveNotification', () => {
          console.log('[SignalR] PUSH received', new Date().toISOString());
          loadNotifications();
      });
  }

  document.addEventListener('DOMContentLoaded', function () {
    loadNotifications();
    initSignalR();
    //setInterval(loadNotifications, 30000);
  });
})();
