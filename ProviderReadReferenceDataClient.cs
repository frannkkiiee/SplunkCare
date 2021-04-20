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
using System.ServiceModel.Channels;
using System.Security.Cryptography.X509Certificates;
using System.ServiceModel;

using Nehta.VendorLibrary.Common;
using nehta.mcaR32.ProviderReadReferenceData;


namespace Nehta.VendorLibrary.HI
{
    /// <summary>
    /// An implementation of a client for the Medicare ReadReferenceData web service. This client may be 
    /// used to read current reference data values held within the HI Service.
    /// </summary>
    public class ProviderReadReferenceDataClient : IDisposable
    {
        internal ProviderReadReferenceDataPortType providerReadReferenceDataClient;

        /// <summary>
        /// SOAP messages for the last client call.
        /// </summary>
        public HIEndpointProcessor.SoapMessages SoapMessages { get; set; }

        /// <summary>
        /// The ProductType to be used in all searches.
        /// </summary>
        ProductType product;

        /// <summary>
        /// The User to be used in all searches.
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
        public const string HIServiceOperation = "ProviderReadReferenceData";

        /// <summary>
        /// HI service version.
        /// </summary>
        public const string HIServiceVersion = "3.2.0";

        #region Constructors

        /// <summary>
        /// Initializes an instance of the ProviderReadReferenceDataClient.
        /// </summary>
        /// <param name="endpointUri">Web service endpoint for Medicare's ProviderReadReferenceData Service.</param>
        /// <param name="product">PCIN (generated by Medicare) and platform name values.</param>
        /// <param name="user">Identifier for the application that is calling the service.</param>
        /// <param name="signingCert">Certificate to sign the soap message with.</param>
        /// <param name="tlsCert">Certificate for establishing TLS connection to the HI service.</param>
        public ProviderReadReferenceDataClient(Uri endpointUri, ProductType product, QualifiedId user, QualifiedId hpio, X509Certificate2 signingCert, X509Certificate2 tlsCert)
        {
            Validation.ValidateArgumentRequired("endpointUri", endpointUri);

            InitializeClient(endpointUri.ToString(), null, signingCert, tlsCert, product, user, hpio);
        }

        /// <summary>
        /// Initializes an instance of the ProviderReadReferenceDataClient.
        /// </summary>
        /// <param name="endpointConfigurationName">Endpoint configuration name for the ProviderReadReferenceData endpoint.</param>
        /// <param name="product">PCIN (generated by Medicare) and platform name values.</param>
        /// <param name="user">Identifier for the application that is calling the service.</param>
        /// <param name="signingCert">Certificate to sign the soap message with.</param>
        /// <param name="tlsCert">Certificate for establishing TLS connection to the HI service.</param>
        public ProviderReadReferenceDataClient(string endpointConfigurationName, ProductType product, QualifiedId user, QualifiedId hpio, X509Certificate2 signingCert, X509Certificate2 tlsCert)
        {
            Validation.ValidateArgumentRequired("endpointConfigurationName", endpointConfigurationName);

            InitializeClient(null, endpointConfigurationName, signingCert, tlsCert, product, user, hpio);
        }

        #endregion

        /// <summary>
        /// Obtain the current acceptable values for a list of web service request elements.
        /// Elements include (but may not be limited to):
        /// <list type="bullet">
        /// <item>providerTypeCode</item>
        /// <item>providerSpecialty</item>
        /// <item>providerSpecialisation</item>
        /// <item>organisationTypeCode</item>
        /// <item>organisationService</item>
        /// <item>organisationServiceUnit</item>
        /// <item>operatingSystem</item>
        /// <item>token</item>
        /// </list>
        /// </summary>
        /// <param name="referenceList">A string array of request elements to look up.</param>
        /// <returns>The current reference data values for the specified elements.</returns>
        public readReferenceDataResponse ReadReferenceData(string[] referenceList)
        {
            Validation.ValidateArgumentRequired("referenceList", referenceList);

            readReferenceDataRequest envelope = new readReferenceDataRequest();

            envelope.readReferenceData = referenceList;
            envelope.product = product;
            envelope.user = user;
            envelope.hpio = hpio;
            envelope.signature = new SignatureContainerType();

            envelope.timestamp = new TimestampType()
            {
                created = DateTime.Now.ToUniversalTime(),
                expires = DateTime.Now.AddDays(30).ToUniversalTime(),
                expiresSpecified = true
            };

            // Set LastSoapRequestTimestamp
            LastSoapRequestTimestamp = envelope.timestamp;

            readReferenceDataResponse1 response1 = null;

            try
            {
                response1 = providerReadReferenceDataClient.readReferenceData(envelope);
            }
            catch (Exception ex)
            {
                // Catch generic FaultException and call helper to throw a more specific fault
                // (FaultException<ServiceMessagesType>
                FaultHelper.ProcessAndThrowFault<ServiceMessagesType>(ex);
            }

            if (response1 != null && response1.readReferenceDataResponse != null)
                return response1.readReferenceDataResponse;
            else
                throw new ApplicationException(Properties.Resources.UnexpectedServiceResponse);
        }

        #region Private and internal methods

        /// <summary>
        /// Initializes an instance of the ProviderReadReferenceDataClient.
        /// </summary>
        /// <param name="endpointUrl">Web service endpoint for Medicare's ProviderReadReferenceData Service.</param>
        /// <param name="endpointConfigurationName">Endpoint configuration name for the ProviderReadReferenceData endpoint.</param>
        /// <param name="signingCert">Certificate to sign the soap message with.</param>
        /// <param name="tlsCert">Certificate for establishing TLS connection to the HI service.</param>
        /// <param name="product">PCIN (generated by Medicare) and platform name values.</param>
        /// <param name="user">Identifier for the application that is calling the service.</param>
        /// <param name="hpio">Identifier for the organisation</param>
        private void InitializeClient(string endpointUrl, string endpointConfigurationName, X509Certificate2 signingCert, X509Certificate2 tlsCert, ProductType product, QualifiedId user, QualifiedId hpio)
        {
            Validation.ValidateArgumentRequired("product", product);
            Validation.ValidateArgumentRequired("user", user);
            Validation.ValidateArgumentRequired("signingCert", signingCert);
            Validation.ValidateArgumentRequired("tlsCert", tlsCert);

            this.product = product;
            this.user = user;
            this.hpio = hpio;

            SoapMessages = new HIEndpointProcessor.SoapMessages();

            ProviderReadReferenceDataPortTypeClient client = null;

            if (!string.IsNullOrEmpty(endpointUrl))
            {
                EndpointAddress address = new EndpointAddress(endpointUrl);
                CustomBinding tlsBinding = GetBinding();

                client = new ProviderReadReferenceDataPortTypeClient(tlsBinding, address);
            }
            else if (!string.IsNullOrEmpty(endpointConfigurationName))
            {
                client = new ProviderReadReferenceDataPortTypeClient(endpointConfigurationName);
            }

            if (client != null)
            {
                HIEndpointProcessor.ProcessEndpoint(client.Endpoint, signingCert, SoapMessages);

                if (tlsCert != null)
                {
                    client.ClientCredentials.ClientCertificate.Certificate = tlsCert;
                }

                providerReadReferenceDataClient = client;
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
            ClientBase<ProviderReadReferenceDataPortType> searchClient;

            if (providerReadReferenceDataClient is ClientBase<ProviderReadReferenceDataPortType>)
            {
                searchClient = (ClientBase<ProviderReadReferenceDataPortType>)providerReadReferenceDataClient;
                if (searchClient.State != CommunicationState.Closed)
                    searchClient.Close();
            }
        }

        #endregion
    }
}
