using System;
using System.Linq;
using Mantle.Extensions;
using Mantle.Interfaces;
using Mantle.Messaging.Interfaces;
using Mantle.Messaging.Messages;

namespace Mantle.Messaging.Extensions
{
// ReSharper disable once InconsistentNaming
    public static class IPublisherChannelExtensions
    {
        public static void Publish<T>(this IPublisherChannel<MessageEnvelope> publisherChannel, T message,
                                      string correlationId = null)
            where T : class
        {
            publisherChannel.Require("publisherChannel");
            message.Require("message");

            var dependencyResolver = MantleContext.Current.DependencyResolver;
            var serializer = dependencyResolver.Get<ISerializer<T>>();
            var typeTokenProviders = dependencyResolver.GetAll<ITypeTokenProvider>().ToList();

            var messageEnvelope = new MessageEnvelope();

            messageEnvelope.Body = serializer.Serialize(message);
            messageEnvelope.CorrelationId = correlationId;
            messageEnvelope.Id = Guid.NewGuid().ToString();

            messageEnvelope.BodyTypeTokens =
                typeTokenProviders.Select(ttp => ttp.GetTypeToken<T>()).Where(tt => (tt != null)).ToList();

            publisherChannel.Publish(messageEnvelope);
        }
    }
}