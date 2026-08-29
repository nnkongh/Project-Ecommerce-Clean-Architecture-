// Please see documentation at https://learn.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.

(function () {
  function getCookie(name) {
    const value = `; ${document.cookie}`;
    const parts = value.split(`; ${name}=`);
    if (parts.length === 2) return parts.pop().split(';').shift();
    return null;
  }

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

  function initSignalR() {
    const bell = document.getElementById('notification-nav');
    if (!bell || typeof window.signalR === 'undefined') return;

    const connection = new signalR.HubConnectionBuilder()
      .withUrl(window.__API_BASE_URL__ + '/chatHub', {
        accessTokenFactory: () => getCookie('access_token') || ''
      })
      .withAutomaticReconnect()
      .build();

    connection.on('ReceiveNotification', function () {
      loadNotifications();
      try {
        const list = document.getElementById('notification-dropdown-list');
        if (list && list.scrollHeight) {
          // Trigger a reload of the dropdown content in case it is open
          loadRecent();
        }
      } catch (e) { }
    });

    connection.start().catch(() => { });
  }

  document.addEventListener('DOMContentLoaded', function () {
    loadNotifications();
    initSignalR();
  });
})();
