using System;
using System.Net.WebSockets;
using System.Reactive.Linq;
using System.Threading;
using System.Threading.Tasks;
using StrawberryShake.Transport.WebSockets;

namespace GraphQL.ClientApp
{
    public class DeadSocketConnectionInterceptor : ISocketConnectionInterceptor
    {
        private readonly TimeSpan m_CheckInterval = TimeSpan.FromSeconds(10);
        private readonly Func<IWebSocketClient> m_GetSocket;
        private readonly Func<IWebSocketClient, Task> m_SocketDied;
        private bool m_IsDying;
        private readonly IDisposable m_TimerSubscription;

        public DeadSocketConnectionInterceptor(Func<IWebSocketClient> getSocket, Func<IWebSocketClient, Task> socketDied)
        {
            m_GetSocket = getSocket;
            m_SocketDied = socketDied;
            m_TimerSubscription = Observable.Timer(DateTimeOffset.Now, m_CheckInterval).Subscribe(CheckSocketState);
        }

        private void CheckSocketState(long obj)
        {
            var socket = m_GetSocket();
            if (socket == null)
                return;

            Console.WriteLine($"Socket IsClosed: {socket.IsClosed}, Inner State: {socket.Socket.State}");

            if (socket.Socket.State != WebSocketState.Aborted || m_IsDying)
                return;

            m_IsDying = true;
            m_TimerSubscription.Dispose();
            m_SocketDied(socket);
        }

        public ValueTask<object> CreateConnectionInitPayload(ISocketProtocol protocol, CancellationToken cancellationToken)
        {
            return new ValueTask<object>();
        }
    }
}