﻿using System;
using Mantle.Azure;
using Microsoft.ServiceBus;
using Microsoft.ServiceBus.Messaging;

namespace Mantle.Messaging.Azure
{
    public class AzureServiceBusTopicPublisherClient : AzureServiceBusClient, IPublisherClient
    {
        private readonly TopicClient topicClient;

        public AzureServiceBusTopicPublisherClient(AzureServiceBusTopicPublisherEndpoint endpoint,
                                                   IAzureServiceBusConfiguration sbConfiguration)
            : base(sbConfiguration)
        {
            if (endpoint == null)
                throw new ArgumentNullException("endpoint");

            endpoint.Validate();

            try
            {
                if (NsManager.TopicExists(endpoint.TopicName) == false)
                    NsManager.CreateTopic(endpoint.TopicName);

                topicClient = TopicClient.CreateFromConnectionString(sbConfiguration.ConnectionString,
                                                                     endpoint.TopicName);

                topicClient.RetryPolicy = RetryExponential.Default;
            }
            catch (Exception ex)
            {
                throw new MessagingException(
                    String.Format(
                        "An error occurred while attempting to access the specified Azure service bus topic [{0}]. See inner exception for more details.",
                        endpoint.TopicName),
                    ex);
            }
        }

        public void Publish<T>(T message)
        {
            try
            {
                var sbMessage = new BrokeredMessage(message);

                sbMessage.Properties["MantleType"] = typeof (T).GetMessagingTypeString();
                topicClient.Send(sbMessage);
            }
            catch (Exception ex)
            {
                throw new MessagingException("Unable to send message. See inner exception for more details.", ex);
            }
        }
    }
}