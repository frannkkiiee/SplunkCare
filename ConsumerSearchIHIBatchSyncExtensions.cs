using System.Collections.Generic;

using nehta.mcaR3.ConsumerSearchIHIBatchSync;
using Nehta.VendorLibrary.Common;

namespace Nehta.VendorLibrary.HI
{
    /// <summary>
    /// Extension methods to assist with ConsumerSearchIHIBatchSync.
    /// </summary>
    public static class ConsumerSearchIHIBatchSyncExtensions
    {
        /// <summary>
        /// Adds a basic search to the batch search.
        /// </summary>
        /// <param name="searches">The current list of searches for the batch search.</param>
        /// <param name="identifier">A 36 character GUID which identifies this search.</param>
        /// <param name="search">
        /// The search criteria. The following fields are expected:
        /// <list type="bullet">
        /// <item><description>ihiNumber (Mandatory)</description></item>
        /// <item><description>familyName (Mandatory)</description></item>
        /// <item><description>givenName (Optional)</description></item>
        /// <item><description>dateOfBirth (Mandatory)</description></item>
        /// <item><description>sex (Mandatory)</description></item>
        /// </list>
        /// All other fields are to be null. For the fields returned by this search, see return value of <see cref="Nehta.VendorLibrary.HI.ConsumerSearchIHIClient.BasicSearch"/>.
        /// </param>
        public static void AddBasicSearch(this List<SearchIHIRequestType> searches, string identifier, searchIHI search)
        {
            Validation.ValidateStringLength("identifier", identifier, 36, true);

            Validation.ValidateArgumentRequired("search", search);
            Validation.ValidateArgumentRequired("search.ihiNumber", search.ihiNumber);
            Validation.ValidateArgumentRequired("search.familyName", search.familyName);
            Validation.ValidateDateTime("search.dateOfBirth", search.dateOfBirth);

            Validation.ValidateArgumentNotAllowed("search.australianPostalAddress", search.australianPostalAddress);
            Validation.ValidateArgumentNotAllowed("search.australianStreetAddress", search.australianStreetAddress);
            Validation.ValidateArgumentNotAllowed("search.dvaFileNumber", search.dvaFileNumber);
            Validation.ValidateArgumentNotAllowed("search.history", search.historySpecified);
            Validation.ValidateArgumentNotAllowed("search.internationalAddress", search.internationalAddress);
            Validation.ValidateArgumentNotAllowed("search.medicareCardNumber", search.medicareCardNumber);
            Validation.ValidateArgumentNotAllowed("search.medicareIRN", search.medicareIRN);

            searches.Add(new SearchIHIRequestType() { 
                searchIHI = search,
                requestIdentifier = identifier
            });
        }

        /// <summary>
        /// Adds a basic medicare search to the batch search.
        /// </summary>
        /// <param name="searches">The current list of searches for the batch search.</param>
        /// <param name="identifier">A 36 character GUID which identifies this search.</param>
        /// <param name="search">
        /// The search criteria. The following fields are expected:
        /// <list type="bullet">
        /// <item><description>medicareCardNumber (Mandatory)</description></item>
        /// <item><description>medicareIRN (Optional)</description></item>
        /// <item><description>familyName (Mandatory)</description></item>
        /// <item><description>givenName (Optional)</description></item>
        /// <item><description>dateOfBirth (Mandatory)</description></item>
        /// <item><description>sex (Mandatory)</description></item>
        /// </list>
        /// All other fields are to be null. For the fields returned by this search, see return value of <see cref="Nehta.VendorLibrary.HI.ConsumerSearchIHIClient.BasicMedicareSearch"/>.
        /// </param>
        public static void AddBasicMedicareSearch(this List<SearchIHIRequestType> searches, string identifier, searchIHI search)
        {
            Validation.ValidateStringLength("identifier", identifier, 36, true);

            Validation.ValidateArgumentRequired("search.medicareCardNumber", search.medicareCardNumber);
            Validation.ValidateArgumentRequired("search.familyName", search.familyName);
            Validation.ValidateDateTime("search.dateOfBirth", search.dateOfBirth);

            Validation.ValidateArgumentNotAllowed("search.ihiNumber", search.ihiNumber);
            Validation.ValidateArgumentNotAllowed("search.dvaFileNumber", search.dvaFileNumber);
            Validation.ValidateArgumentNotAllowed("search.australianPostalAddress", search.australianPostalAddress);
            Validation.ValidateArgumentNotAllowed("search.australianStreetAddress", search.australianStreetAddress);
            Validation.ValidateArgumentNotAllowed("search.history", search.historySpecified);
            Validation.ValidateArgumentNotAllowed("search.internationalAddress", search.internationalAddress);

            searches.Add(new SearchIHIRequestType()
            {
                searchIHI = search,
                requestIdentifier = identifier
            });
        }

        /// <summary>
        /// Adds a basic DVA search to the batch search.
        /// </summary>
        /// <param name="searches">The current list of searches for the batch search.</param>
        /// <param name="identifier">A 36 character GUID which identifies this search.</param>
        /// <param name="search">
        /// The search criteria. The following fields are expected:
        /// <list type="bullet">
        /// <item><description>dvaFileNumber (Mandatory)</description></item>
        /// <item><description>familyName (Mandatory)</description></item>
        /// <item><description>givenName (Optional)</description></item>
        /// <item><description>dateOfBirth (Mandatory)</description></item>
        /// <item><description>sex (Mandatory)</description></item>
        /// </list>
        /// All other fields are to be null. For the fields returned by this search, see return value of <see cref="Nehta.VendorLibrary.HI.ConsumerSearchIHIClient.BasicDvaSearch"/>.
        /// </param>
        public static void AddBasicDvaSearch(this List<SearchIHIRequestType> searches, string identifier, searchIHI search)
        {
            Validation.ValidateStringLength("identifier", identifier, 36, true);

            Validation.ValidateArgumentRequired("search", search);
            Validation.ValidateArgumentRequired("search.dvaFileNumber", search.dvaFileNumber);
            Validation.ValidateArgumentRequired("search.familyName", search.familyName);
            Validation.ValidateDateTime("search.dateOfBirth", search.dateOfBirth);

            Validation.ValidateArgumentNotAllowed("search.medicareCardNumber", search.medicareCardNumber);
            Validation.ValidateArgumentNotAllowed("search.medicareIRN", search.medicareIRN);
            Validation.ValidateArgumentNotAllowed("search.ihiNumber", search.ihiNumber);
            Validation.ValidateArgumentNotAllowed("search.australianPostalAddress", search.australianPostalAddress);
            Validation.ValidateArgumentNotAllowed("search.australianStreetAddress", search.australianStreetAddress);
            Validation.ValidateArgumentNotAllowed("search.history", search.historySpecified);
            Validation.ValidateArgumentNotAllowed("search.internationalAddress", search.internationalAddress);        

            searches.Add(new SearchIHIRequestType()
            {
                searchIHI = search,
                requestIdentifier = identifier
            });
        }
        
        /// <summary>
        /// Adds a detailed search to the batch search.
        /// </summary>
        /// <param name="searches">The current list of searches for the batch search.</param>
        /// <param name="identifier">A 36 character GUID which identifies this search.</param>
        /// <param name="search">
        /// The search criteria. The following fields are expected:
        /// <list type="bullet">
        /// <item><description>familyName (Mandatory)</description></item>
        /// <item><description>givenName (Optional)</description></item>
        /// <item><description>dateOfBirth (Mandatory)</description></item>
        /// <item><description>sex (Mandatory)</description></item>
        /// </list>
        /// All other fields are to be null. For the fields returned by this search, see return value of <see cref="Nehta.VendorLibrary.HI.ConsumerSearchIHIClient.DetailedSearch"/>.
        /// </param>
        public static void AddDetailedSearch(this List<SearchIHIRequestType> searches, string identifier, searchIHI search)
        {
            Validation.ValidateStringLength("identifier", identifier, 36, true);

            Validation.ValidateArgumentRequired("search", search);
            Validation.ValidateArgumentRequired("search.familyName", search.familyName);
            Validation.ValidateDateTime("search.dateOfBirth", search.dateOfBirth);

            Validation.ValidateArgumentNotAllowed("search.medicareCardNumber", search.medicareCardNumber);
            Validation.ValidateArgumentNotAllowed("search.medicareIRN", search.medicareIRN);
            Validation.ValidateArgumentNotAllowed("search.ihiNumber", search.ihiNumber);
            Validation.ValidateArgumentNotAllowed("search.australianPostalAddress", search.australianPostalAddress);
            Validation.ValidateArgumentNotAllowed("search.australianStreetAddress", search.australianStreetAddress);
            Validation.ValidateArgumentNotAllowed("search.dvaFileNumber", search.dvaFileNumber);
            Validation.ValidateArgumentNotAllowed("search.history", search.historySpecified);
            Validation.ValidateArgumentNotAllowed("search.internationalAddress", search.internationalAddress);        

            searches.Add(new SearchIHIRequestType()
            {
                searchIHI = search,
                requestIdentifier = identifier
            });
        }

        /// <summary>
        /// Adds an australian postal address search to the batch search.
        /// </summary>
        /// <param name="searches">The current list of searches for the batch search.</param>
        /// <param name="identifier">A 36 character GUID which identifies this search.</param>
        /// <param name="search">
        /// The search criteria. The following fields are expected:
        /// <list type="bullet">
        /// <item><description>familyName (Mandatory)</description></item>
        /// <item><description>givenName (Optional)</description></item>
        /// <item><description>dateOfBirth (Mandatory)</description></item>
        /// <item><description>sex (Mandatory)</description></item>
        /// <item>
        ///     <description><b>australianPostalAddress</b> (Mandatory)
        ///     <list type="bullet">
        ///         <item><description>suburb (Mandatory)</description></item>
        ///         <item><description>state (Mandatory)</description></item>
        ///         <item><description>postcode (Mandatory)</description></item>
        ///         <item>
        ///             <description><b>postalDeliveryGroup</b> (Mandatory)
        ///             <list type="bullet">
        ///                 <item><description>postalDeliveryType (Mandatory)</description></item>
        ///                 <item><description>postalDeliveryNumber (Optional)</description></item>
        ///             </list>
        ///             </description>
        ///         </item>
        ///     </list>
        ///     </description>
        /// </item>
        /// </list>
        /// All other fields are to be null. For the fields returned by this search, see return value of <see cref="Nehta.VendorLibrary.HI.ConsumerSearchIHIClient.AustralianPostalAddressSearch"/>.
        /// </param>
        public static void AddAustralianPostalAddressSearch(this List<SearchIHIRequestType> searches, string identifier, searchIHI search)
        {
            Validation.ValidateStringLength("identifier", identifier, 36, true);

            Validation.ValidateArgumentRequired("search", search);
            Validation.ValidateArgumentRequired("search.familyName", search.familyName);
            Validation.ValidateDateTime("search.dateOfBirth", search.dateOfBirth);

            // Check australian postal address
            Validation.ValidateArgumentRequired("search.australianPostalAddress", search.australianPostalAddress);
            Validation.ValidateArgumentRequired("search.australianPostalAddress.postalDeliveryGroup", search.australianPostalAddress.postalDeliveryGroup);
            Validation.ValidateArgumentRequired("search.australianPostalAddress.suburb", search.australianPostalAddress.suburb);
            Validation.ValidateArgumentRequired("search.australianPostalAddress.postcode", search.australianPostalAddress.postcode);

            Validation.ValidateArgumentNotAllowed("search.medicareCardNumber", search.medicareCardNumber);
            Validation.ValidateArgumentNotAllowed("search.medicareIRN", search.medicareIRN);
            Validation.ValidateArgumentNotAllowed("search.ihiNumber", search.ihiNumber);
            Validation.ValidateArgumentNotAllowed("search.australianStreetAddress", search.australianStreetAddress);
            Validation.ValidateArgumentNotAllowed("search.dvaFileNumber", search.dvaFileNumber);
            Validation.ValidateArgumentNotAllowed("search.history", search.historySpecified);
            Validation.ValidateArgumentNotAllowed("search.internationalAddress", search.internationalAddress);    

            searches.Add(new SearchIHIRequestType()
            {
                searchIHI = search,
                requestIdentifier = identifier
            });
        }

        /// <summary>
        /// Adds an australian street address search to the batch search.
        /// </summary>
        /// <param name="searches">The current list of searches for the batch search.</param>
        /// <param name="identifier">A 36 character GUID which identifies this search.</param>
        /// <param name="search">
        /// The search criteria. The following fields are expected:
        /// <list type="bullet">
        /// <item><description>familyName (Mandatory)</description></item>
        /// <item><description>givenName (Optional)</description></item>
        /// <item><description>dateOfBirth (Mandatory)</description></item>
        /// <item><description>sex (Mandatory)</description></item>
        /// <item>
        ///     <description><b>australianStreetAddress</b> (Mandatory)
        ///     <list type="bullet">
        ///         <item>
        ///             <description><b>unitGroup</b> (Optional)
        ///             <list type="bullet">
        ///                 <item><description>unitType (Mandatory)</description></item>
        ///                 <item><description>unitNumber (Optional)</description></item>
        ///             </list>
        ///             </description>
        ///         </item>
        ///         <item>
        ///             <description><b>levelGroup</b> (Optional)
        ///             <list type="bullet">
        ///                 <item><description>levelType (Mandatory)</description></item>
        ///                 <item><description>levelNumber (Optional)</description></item>
        ///             </list>
        ///             </description>
        ///         </item>
        ///         <item><description>addressSiteName (Optional)</description></item>
        ///         <item><description>streetNumber (Optional)</description></item>
        ///         <item><description>lotNumber (Optional)</description></item>
        ///         <item><description>streetName (Mandatory)</description></item>
        ///         <item><description>streetType (Conditional on if streetTypeSpecified is set to true)</description></item>
        ///         <item><description>streetTypeSpecified (Mandatory)</description></item>
        ///         <item><description>streetSuffix (Conditional on if streetSuffixSpecified is set to true)</description></item>
        ///         <item><description>streetSuffixSpecified (Mandatory)</description></item>
        ///         <item><description>suburb (Mandatory)</description></item>
        ///         <item><description>state (Mandatory)</description></item>
        ///         <item><description>postcode (Mandatory)</description></item>
        ///     </list>
        ///     </description>
        /// </item>
        /// </list>
        /// All other fields are to be null. For the fields returned by this search, see return value of <see cref="Nehta.VendorLibrary.HI.ConsumerSearchIHIClient.AustralianStreetAddressSearch"/>.
        /// </param>
        public static void AddAustralianStreetAddressSearch(this List<SearchIHIRequestType> searches, string identifier, searchIHI search)
        {
            Validation.ValidateStringLength("identifier", identifier, 36, true);

            Validation.ValidateArgumentRequired("search", search);
            Validation.ValidateArgumentRequired("search.familyName", search.familyName);
            Validation.ValidateDateTime("search.dateOfBirth", search.dateOfBirth);

            // Check australian street address
            Dictionary<string, object> c1 = new Dictionary<string, object>();
            c1.Add("search.australianStreetAddress.streetNumber", search.australianStreetAddress.streetNumber);
            c1.Add("search.australianStreetAddress.lotNumber", search.australianStreetAddress.lotNumber);
            Validation.ValidateArgumentAtLeastOneRequired(c1);
            Validation.ValidateArgumentRequired("search.australianStreetAddress", search.australianStreetAddress);
            Validation.ValidateArgumentRequired("search.australianStreetAddress.postcode", search.australianStreetAddress.postcode);
            Validation.ValidateArgumentRequired("search.australianStreetAddress.suburb", search.australianStreetAddress.suburb);
            Validation.ValidateArgumentRequired("search.australianStreetAddress.streetName", search.australianStreetAddress.streetName);

            Validation.ValidateArgumentNotAllowed("search.medicareCardNumber", search.medicareCardNumber);
            Validation.ValidateArgumentNotAllowed("search.medicareIRN", search.medicareIRN);
            Validation.ValidateArgumentNotAllowed("search.ihiNumber", search.ihiNumber);
            Validation.ValidateArgumentNotAllowed("search.australianPostalAddress", search.australianPostalAddress);
            Validation.ValidateArgumentNotAllowed("search.dvaFileNumber", search.dvaFileNumber);
            Validation.ValidateArgumentNotAllowed("search.history", search.historySpecified);
            Validation.ValidateArgumentNotAllowed("search.internationalAddress", search.internationalAddress);

            searches.Add(new SearchIHIRequestType()
            {
                searchIHI = search,
                requestIdentifier = identifier
            });
        }

        /// <summary>
        /// Adds an international address search to the batch search.
        /// </summary>
        /// <param name="searches">The current list of searches for the batch search.</param>
        /// <param name="identifier">A 36 character GUID which identifies this search.</param>
        /// <param name="search">
        /// The search criteria. The following fields are expected:
        /// <list type="bullet">
        /// <item><description>familyName (Mandatory)</description></item>
        /// <item><description>givenName (Optional)</description></item>
        /// <item><description>dateOfBirth (Mandatory)</description></item>
        /// <item><description>sex (Mandatory)</description></item>
        /// <item>
        ///     <description><b>internationalAddress</b> (Mandatory)
        ///     <list type="bullet">
        ///         <item><description>internationalAddressLine (Mandatory)</description></item>
        ///         <item><description>internationalStateProvince (Mandatory)</description></item>
        ///         <item><description>internationalPostcode (Mandatory)</description></item>
        ///         <item><description>country (Mandatory)</description></item>
        ///     </list>
        ///     </description>
        /// </item>
        /// </list>
        /// All other fields are to be null. For the fields returned by this search, see return value of <see cref="Nehta.VendorLibrary.HI.ConsumerSearchIHIClient.InternationalAddressSearch"/>.
        /// </param>
        public static void AddInternationalAddressSearch(this List<SearchIHIRequestType> searches, string identifier, searchIHI search)
        {
            Validation.ValidateStringLength("identifier", identifier, 36, true);

            Validation.ValidateArgumentRequired("search", search);
            Validation.ValidateArgumentRequired("search.familyName", search.familyName);
            Validation.ValidateDateTime("search.dateOfBirth", search.dateOfBirth);

            // Check international address
            Validation.ValidateArgumentRequired("search.internationalAddress", search.internationalAddress);
            Validation.ValidateArgumentRequired("search.internationalAddress.internationalStateProvince", search.internationalAddress.internationalStateProvince);
            Validation.ValidateArgumentRequired("search.internationalAddress.internationalPostcode", search.internationalAddress.internationalPostcode);
            Validation.ValidateArgumentRequired("search.internationalAddress.internationalAddressLine", search.internationalAddress.internationalAddressLine);

            Validation.ValidateArgumentNotAllowed("search.medicareCardNumber", search.medicareCardNumber);
            Validation.ValidateArgumentNotAllowed("search.medicareIRN", search.medicareIRN);
            Validation.ValidateArgumentNotAllowed("search.ihiNumber", search.ihiNumber);
            Validation.ValidateArgumentNotAllowed("search.australianPostalAddress", search.australianPostalAddress);
            Validation.ValidateArgumentNotAllowed("search.australianStreetAddress", search.australianStreetAddress);
            Validation.ValidateArgumentNotAllowed("search.dvaFileNumber", search.dvaFileNumber);
            Validation.ValidateArgumentNotAllowed("search.history", search.historySpecified);  

            searches.Add(new SearchIHIRequestType()
            {
                searchIHI = search,
                requestIdentifier = identifier
            });
        }
    }
}
