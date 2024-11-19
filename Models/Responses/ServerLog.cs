using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;

namespace InventorySystem.Models.Responses
{
    /// <summary>
    /// Represents a server log that contains information about the status and messages related to server operations.
    /// </summary>
    public class ServerLog : ApiMessageListResponse
    {
        /// <summary>
        /// Gets or sets the user action that triggered the server log entry.
        /// </summary>
        public string? UserAction { get; set; }

        /// <summary>
        /// Gets or sets the server message associated with the server log entry.
        /// </summary>
        public string? ServerMessage { get; set; }

        /// <summary>
        /// Gets or sets the status code returned by the server during the operation.
        /// </summary>
        public int? StatusCode { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="ServerLog"/> class with the provided values.
        /// This method is used to set all the properties of the <see cref="ServerLog"/>.
        /// </summary>
        /// <param name="isSuccess">Indicates whether the operation was successful.</param>
        /// <param name="messages">A list of messages associated with the server log entry.</param>
        /// <param name="model">The model object related to the log entry.</param>
        /// <param name="redirectUrl">The URL to which the user should be redirected after the operation.</param>
        /// <param name="action">The user action that triggered the log entry.</param>
        /// <param name="serverMessage">A message from the server regarding the operation.</param>
        /// <param name="statuscode">The HTTP status code returned by the server.</param>
        public void Construct(bool isSuccess, List<string> messages, object? model, string? redirectUrl,
             string? action, string? serverMessage, int? statuscode)
        {
            IsValid = isSuccess;
            Message = messages;
            Model = model;
            RedirectUrl = redirectUrl;
            UserAction = action;
            ServerMessage = serverMessage;
            StatusCode = statuscode;
        }

        /// <summary>
        /// Deconstructs the <see cref="ServerLog"/> into individual properties.
        /// This method allows for easy extraction of the values contained within the <see cref="ServerLog"/>.
        /// </summary>
        /// <param name="isSuccess">Outputs the success status of the operation.</param>
        /// <param name="messages">Outputs the list of messages associated with the server log entry.</param>
        /// <param name="model">Outputs the model object related to the log entry.</param>
        /// <param name="redirectUrl">Outputs the URL to which the user should be redirected after the operation.</param>
        /// <param name="action">Outputs the user action that triggered the log entry.</param>
        /// <param name="serverMessage">Outputs the message from the server regarding the operation.</param>
        /// <param name="statuscode">Outputs the HTTP status code returned by the server.</param>
        public void Deconstruct(out bool isSuccess, out List<string> messages, out object? model, out string? redirectUrl,
            out string? action, out string? serverMessage, out int? statuscode)
        {
            isSuccess = IsValid;
            messages = Message ?? new List<string>();  // Initialize with an empty list if Message is null
            model = Model;
            redirectUrl = RedirectUrl;
            action = UserAction;
            serverMessage = ServerMessage;
            statuscode = StatusCode;
        }
    }
}
