using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;

namespace Tor.Controller
{
    /// <summary>
    /// A class containing the base methods and properties for a command which will be executed across a control connection,
    /// and will return a response corresponding to the response of the tor application.
    /// </summary>
    internal abstract class Command<T> where T : Response
    {
        /// <summary>
        /// Creates a new <typeparamref name="TCommand"/> object instance and dispatches the command to the specified client.
        /// </summary>
        /// <typeparam name="TCommand">The type of the command.</typeparam>
        /// <typeparam name="TResponse">The type of the response generated from the command.</typeparam>
        /// <param name="client">The client hosting the control connection port.</param>
        /// <returns><c>true</c> if the command was created and dispatched successfully; otherwise, <c>false</c>.</returns>
        public static bool DispatchAndReturn<TCommand>(Client client) where TCommand : Command<T>
        {
            try
            {
                TCommand command = Activator.CreateInstance<TCommand>();

                if (command == null)
                    return false;

                T response = command.Dispatch(client);
                return response.Success;
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// Dispatches the command to the client control port and produces a <typeparamref name="T"/> response result.
        /// </summary>
        /// <param name="client">The client hosting the control connection port.</param>
        /// <returns>A <typeparamref name="T"/> object instance containing the response data.</returns>
        public T Dispatch(Client client)
        {
            if (client == null)
                throw new ArgumentNullException("client");
            if (!client.IsRunning)
                throw new TorException("A command cannot be dispatched to a client which is no longer running");

            try
            {
                using (Connection connection = new Connection(client))
                {
                    if (!connection.Connect())
                        throw new TorException("A command could not be dispatched to a client because the command failed to connect to the control port");

                    if (!connection.Authenticate(client.GetControlPassword()))
                        throw new TorException("A command could not be dispatched to a client because the control could not be authenticated");

                    return Dispatch(connection);
                }
            }
            catch (Exception exception)
            {
                throw new TorException("A command could not be dispatched to a client because an error occurred", exception);
            }
        }

        /// <summary>
        /// Dispatches the command to the client control port and produces a <typeparamref name="T"/> response result.
        /// </summary>
        /// <param name="connection">The control connection where the command should be dispatched.</param>
        /// <returns>A <typeparamref name="T"/> object instance containing the response data.</returns>
        protected abstract T Dispatch(Connection connection);
    }
}
