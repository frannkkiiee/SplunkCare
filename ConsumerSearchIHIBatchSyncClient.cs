﻿/*
 * Copyright 2011 NEHTA
 *
 * Licensed under the NEHTA Open Source (Apache) License; you may not use this
 * file except in compliance with the License. A copy of the License is in the
 * 'license.txt' file, which should be provided with this work.
 *
 * Unless required by applicable law or agreed to in writing, software
 * distributed under the License is distributed on an "AS IS" BASIS, WITHOUT
 * WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied. See the
 * License for the specific language governing permissions and limitations
 * under the License.
 */

using System;
using System.Collections.Generic;
using System.ServiceModel.Channels;
using System.Security.Cryptography.X509Certificates;
using System.ServiceModel;

using Nehta.VendorLibrary.Common;
using nehta.mcaR3.ConsumerSearchIHIBatchSync;
using Nehta.VendorLibrary.HI.Common;
using AutoMapper;

namespace Nehta.VendorLibrary.HI
{
    /// <summary>
    /// An implementation of a client for the Medicare Healthcare Identifiers service. This class may be used to 
    /// connect to Medicare's service to perform IHI batch search operations.
    /// </summary>
    public class ConsumerSearchIHIBatchSyncClient : IDisposable
    {
        internal ConsumerSearchIHIBatchSyncPortType ihiBatchClient;

        /// <summary>
        /// SOAP messages for the last client call.
        /// </summary>
        public HIEndpointProcessor.SoapMessages SoapMessages { get; set; }

        /// <summary>
        /// The ProductType to be used in all IHI searches.
        /// </summary>
        ProductType product;

        /// <summary>
        /// The User to be used in all IHI searches.
        /// </summary>
        QualifiedId user;

        /// <summary>
        /// The hpio of the organisation.
        /// </summary>
        QualifiedId hpio;

        /// <summary>
        /// Gets the timestamp for the soap request.
        /// </summary>
        public TimestampType LastSoapRequestTimestamp { get; private set; }

        /// <summary>
        /// HI service name.
        /// </summary>
        public const string HIServiceOperation = "ConsumerSearchIHIBatchSync";

        /// <summary>
        /// HI service version.
        /// </summary>
        public const string HIServiceVersion = "3.0";

        #region Constructors

        /// <summary>
        /// Initializes an instance of the ConsumerSearchIHIBatchSyncClient.
        /// </summary>
        /// <param name="endpointUri">Web service endpoint for Medicare's consumer IHI batch search service.</param>
        /// <param name="product">PCIN (generated by Medicare) and platform name values.</param>
        /// <param name="user">Identifier for the application that is calling the service.</param>
        /// <param name="signingCert">Certificate to sign the soap message with.</param>
        /// <param name="tlsCert">Certificate for establishing TLS connection to the HI service.</param>
        public ConsumerSearchIHIBatchSyncClient(Uri endpointUri, ProductType product, QualifiedId user, QualifiedId hpio, X509Certificate2 signingCert, X509Certificate2 tlsCert)
        {
            Validation.ValidateArgumentRequired("endpointUri", endpointUri);

            InitializeClient(endpointUri.ToString(), null, signingCert, tlsCert, product, user, hpio);
        }

        /// <summary>
        /// Initializes an instance of the ConsumerSearchIHIBatchSyncClient.
        /// </summary>
        /// <param name="endpointConfigurationName">Endpoint configuration name for the ConsumerSearchIHI endpoint.</param>
        /// <param name="product">PCIN (generated by Medicare) and platform name values.</param>
        /// <param name="user">Identifier for the application that is calling the service.</param>
        /// <param name="signingCert">Certificate to sign the soap message with.</param>
        /// <param name="tlsCert">Certificate for establishing TLS connection to the HI service.</param>
        public ConsumerSearchIHIBatchSyncClient(string endpointConfigurationName, ProductType product, QualifiedId user, QualifiedId hpio, X509Certificate2 signingCert, X509Certificate2 tlsCert)
        {
            Validation.ValidateArgumentRequired("endpointConfigurationName", endpointConfigurationName);

            InitializeClient(null, endpointConfigurationName, signingCert, tlsCert, product, user, hpio);
        }

        #endregion

        /// <summary>
        /// Perform a consumer batch ihi search on the Medicare IHI service.
        /// </summary>
        /// <param name="searches">List of SearchIHIRequestType containing the request identifier and the search parameters.</param>
        /// <returns>Results matching the search parameters. Different fields are returned for diffent search types. For full details please refer to:
        /// <list type="bullet">
        /// <item><description><see cref="Nehta.VendorLibrary.HI.ConsumerSearchIHIClient.BasicSearch"/></description></item>
        /// <item><description><see cref="Nehta.VendorLibrary.HI.ConsumerSearchIHIClient.BasicMedicareSearch"/></description></item>
        /// <item><description><see cref="Nehta.VendorLibrary.HI.ConsumerSearchIHIClient.BasicDvaSearch"/></description></item>
        /// <item><description><see cref="Nehta.VendorLibrary.HI.ConsumerSearchIHIClient.DetailedSearch"/></description></item>
        /// <item><description><see cref="Nehta.VendorLibrary.HI.ConsumerSearchIHIClient.AustralianPostalAddressSearch"/></description></item>
        /// <item><description><see cref="Nehta.VendorLibrary.HI.ConsumerSearchIHIClient.AustralianStreetAddressSearch"/></description></item>
        /// <item><description><see cref="Nehta.VendorLibrary.HI.ConsumerSearchIHIClient.InternationalAddressSearch"/></description></item>
        /// </list>
        /// </returns>
        public searchIHIBatchResponse SearchIHIBatchSync(List<CommonSearchIHIRequestType> searches)
        {
            Validation.ValidateArgumentRequired("searches", searches);

            searchIHIBatchSyncRequest envelope = new searchIHIBatchSyncRequest();

            var mappedSearches = Mapper.Map<List<CommonSearchIHIRequestType>, List<SearchIHIRequestType>>(searches);

            envelope.searchIHIBatchSync = mappedSearches.ToArray();
            envelope.product = product;
            envelope.user = user;
            envelope.hpio = hpio;
            envelope.signature = new SignatureContainerType();

            envelope.timestamp = new TimestampType()
            {
                created = DateTime.Now,
                expires = DateTime.Now.AddDays(30),
                expiresSpecified = true
            };

            // Set LastSoapRequestTimestamp
            LastSoapRequestTimestamp = envelope.timestamp;                            
            
            searchIHIBatchSyncResponse response = null;

            try
            {
                response = ihiBatchClient.searchIHIBatchSync(envelope);
            }
            catch (Exception ex)
            {
                // Catch generic FaultException and call helper to throw a more specific fault
                // (FaultException<ServiceMessagesType>
                FaultHelper.ProcessAndThrowFault<ServiceMessagesType>(ex);
            }

            if (response != null && response.searchIHIBatchResponse != null)
                return response.searchIHIBatchResponse;
            else
                throw new ApplicationException(Properties.Resources.UnexpectedServiceResponse);
        }

        #region Private and internal methods

        /// <summary>
        /// Initializes an instance of the ConsumerSearchIHIBatchSyncClient.
        /// </summary>
        /// <param name="endpointUrl">Web service endpoint for Medicare's consumer IHI batch search service.</param>
        /// <param name="endpointConfigurationName">Endpoint configuration name for the ConsumerSearchIHIBatchSync endpoint.</param>
        /// <param name="product">PCIN (generated by Medicare) and platform name values.</param>
        /// <param name="user">Identifier for the application that is calling the service.</param>
        /// <param name="signingCert">Certificate to sign the soap message with.</param>
        /// <param name="tlsCert">Certificate for establishing TLS connection to the HI service.</param>
        /// <param name="hpio">Identifer for the organisation</param>
        private void InitializeClient(string endpointUrl, string endpointConfigurationName, X509Certificate2 signingCert, X509Certificate2 tlsCert, ProductType product, QualifiedId user, QualifiedId hpio)
        {
            Utility.SetUpMapping();

            Validation.ValidateArgumentRequired("product", product);
            Validation.ValidateArgumentRequired("user", user);
            Validation.ValidateArgumentRequired("signingCert", signingCert);
            Validation.ValidateArgumentRequired("tlsCert", tlsCert);

            this.product = product;
            this.user = user;
            this.hpio = hpio;

            SoapMessages = new HIEndpointProcessor.SoapMessages();

            ConsumerSearchIHIBatchSyncPortTypeClient client = null;

            if (!string.IsNullOrEmpty(endpointUrl))
            {
                EndpointAddress address = new EndpointAddress(endpointUrl);
                CustomBinding tlsBinding = GetBinding();

                client = new ConsumerSearchIHIBatchSyncPortTypeClient(tlsBinding, address);
            }
            else if (!string.IsNullOrEmpty(endpointConfigurationName))
            {
                client = new ConsumerSearchIHIBatchSyncPortTypeClient(endpointConfigurationName);
            }

            if (client != null)
            {
                HIEndpointProcessor.ProcessEndpoint(client.Endpoint, signingCert, SoapMessages);

                if (tlsCert != null)
                {
                    client.ClientCredentials.ClientCertificate.Certificate = tlsCert;
                }

                ihiBatchClient = client;
            }
        }

        /// <summary>
        /// Gets the binding configuration for the client.
        /// </summary>
        /// <returns>Configured CustomBinding instance.</returns>
        internal CustomBinding GetBinding()
        {
            // Set up binding
            CustomBinding tlsBinding = new CustomBinding();
            
            // Extend default timeouts
            tlsBinding.CloseTimeout = new TimeSpan(0, 3, 0);
            tlsBinding.OpenTimeout = new TimeSpan(0, 3, 0);
            tlsBinding.ReceiveTimeout = new TimeSpan(0, 10, 0);
            tlsBinding.SendTimeout = new TimeSpan(0, 3, 0);

            TextMessageEncodingBindingElement tlsEncoding = new TextMessageEncodingBindingElement();
            tlsEncoding.ReaderQuotas.MaxDepth = 2147483647;
            tlsEncoding.ReaderQuotas.MaxStringContentLength = 2147483647;
            tlsEncoding.ReaderQuotas.MaxArrayLength = 2147483647;
            tlsEncoding.ReaderQuotas.MaxBytesPerRead = 2147483647;
            tlsEncoding.ReaderQuotas.MaxNameTableCharCount = 2147483647;

            HttpsTransportBindingElement httpsTransport = new HttpsTransportBindingElement();
            httpsTransport.RequireClientCertificate = true;
            httpsTransport.MaxReceivedMessageSize = 2147483647;
            httpsTransport.MaxBufferSize = 2147483647;

            tlsBinding.Elements.Add(tlsEncoding);
            tlsBinding.Elements.Add(httpsTransport);

            return tlsBinding;
        }

        #endregion

        #region IDisposable Members

        /// <summary>
        /// Closes and disposes the client.
        /// </summary>
        public void Dispose()
        {
            ClientBase<ConsumerSearchIHIBatchSyncPortType> searchClient;

            if (ihiBatchClient is ClientBase<ConsumerSearchIHIBatchSyncPortType>)
            {
                searchClient = (ClientBase<ConsumerSearchIHIBatchSyncPortType>)ihiBatchClient;
                if (searchClient.State != CommunicationState.Closed)
                    searchClient.Close();
            }
        }

        #endregion
    }
}
