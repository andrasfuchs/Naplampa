using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using NaplampaService;
using NaplampaService.DataContract;
using NaplampaWcfHost;
using NaplampaWcfHost.DataContract;
using System.IO;

namespace NaplampaService
{
    [ServiceContract(SessionMode=SessionMode.Required)]
    public interface INaplampaService
    {
        [OperationContract(IsOneWay = true)]
        void EnableEmailService();

        [OperationContract(IsOneWay = true)]
        void DisableEmailService();

        [OperationContract]
        bool IsEmailServiceDisabled();

        [OperationContract]
        Person Login(int personId, string password);

        [OperationContract(IsOneWay = true)]
        void ResetPassword(int personId);

        [OperationContract]
        int ConfirmEmail(string email, string confirmationCode);

        [OperationContract]
        int CreateNewPartner(string partnerName, string partnerPhone, string contactPersonTitle, string contactPersonFirstName, string contactPersonLastName, string contactPersonEmail, bool contactPersonNewsletter, string contactPersonCultureName);

        [OperationContract]
        int CreateNewAddress(int partnerId, int countryId, string province, string town, string postalCode, string addressLine, string name);

        [OperationContract]
        int CreateNewOrderWithoutLogin(string title, string firstName, string lastName, string email, string phone, bool newsletter, string cultureName, int countryId, string province, string town, string postalCode, string addressLine, short paymentMethod, string referer, List<ProductQuantity> basket, int invoiceCurrencyId, int? invoiceCountryId, string invoiceProvince, string invoiceTown, string invoicePostalCode, string invoiceAddressLine, string invoiceFullName, int? initialStatus, string couponCode);

        [OperationContract]
        int CreateNewOrderForPartner(int partnerId, int deliveryAddressId, short paymentMethod, string referer, List<ProductQuantity> basket, int invoiceCurrencyId, string couponCode);

        [OperationContract]
        int CreateWarrantyOrder(int originalOrderId, List<ProductQuantity> basket);

        [OperationContract]
        OrderCosts CalculateOrderCosts(int? parnerId, int countryId, short paymentMethod, List<ProductQuantity> basket, int invoiceCurrencyId, string couponCode);

        [OperationContract]
        Order GetOrder(int orderId);

        [OperationContract]
        OrderReview GetOrderReview(int orderId, string payPalToken);

        [OperationContract]
        bool ConfirmOrder(int orderId, string payPalToken, string payPalPayerId);

        [OperationContract]
        Order[] ListOrders(int? partnerId, DateTime from);

        [OperationContract(IsOneWay = true)]
        void DeleteOrder(int orderId);

        [OperationContract(IsOneWay = true)]
        void SetOrderStatus(int orderId, int status);

        [OperationContract(IsOneWay = true)]
        void OrderPackageSent(int orderId, int sentByPersonId, DateTime sentOn, string trackingNumber);

        [OperationContract(IsOneWay = true)]
        void OrderDelivered(int orderId, DateTime deliveredOn);

        [OperationContract(IsOneWay = true)]
        void OrderReturned(int orderId);

        [OperationContract(IsOneWay = true)]
        void OrderResendPaymentRequest(int orderId);

        [OperationContract(IsOneWay = true)]
        void OrderPaymentRequestSent(int orderId);

        [OperationContract(IsOneWay = true)]
        void OrderPaymentReceived(int orderId, string payPalToken);

        [OperationContract(IsOneWay = true)]
        void OrderRefund(int orderId, decimal refundAmount, List<ProductQuantity> productsReturned);

        [OperationContract(IsOneWay = true)]
        void SetOrderSellerComment(int orderId, string comment);

        [OperationContract(IsOneWay = true)]
        void SetOrderBuyerComment(int orderId, string comment);

        //[OperationContract(IsOneWay = true)]
        //void SetOrderMigrationValues(int orderId, DateTime createdOn, decimal sum, int statusModifiedByPersonId, DateTime statusModifiedOn);

        [OperationContract(IsOneWay = true)]
        void SendEmail(string templateName, string cultureName, string recipientEmail, object[] parameters);

        [OperationContract]
        Country[] ListCountries();

        [OperationContract]
        Currency[] ListCurrencies();

        [OperationContract]
        ProductDiscountPrice[] GetEffectiveProductPrices(Product[] products, int currencyId, int? partnerId, int countryId, DateTime dateTime);

        [OperationContract]
        Product[] ListProducts();

        [OperationContract]
        Invoice GetInvoice(int invoiceId);

        [OperationContract]
        byte[] GetInvoiceImage(int invoiceId, string cultureName);

        [OperationContract]
        Survey GetSurvey(int surveyId);

        [OperationContract]
        Person GetPersonById(int personId);

        [OperationContract(IsOneWay = true)]
        void SendSurveyRequest(int orderId, int surveyId);

        [OperationContract]
        Survey[] ListSurveys(bool validOnly);

        [OperationContract(IsOneWay = true)]
        void SaveSurveyResult(int surveyId, int orderId, string cultureName, string surveyQuestions
            , int resultValue1, int resultValue2, int resultValue3, int resultValue4, int resultValue5, int resultValue6, int resultValue7
            , string resultText1, string resultText2, string resultText3, string resultText4, string resultText5, string resultText6, string resultText7
            , int resultFlags1, int resultFlags2, int resultFlags3, int resultFlags4, int resultFlags5, int resultFlags6, int resultFlags7);

        [OperationContract(IsOneWay = true)]
        void SendRecommendation(int? orderId, string[] recipients, string message);

        [OperationContract]
        Order[] SearchOrders(int? partnerId, string keyword, int minimumRank, decimal minimumOrder, int minimumOrderCurrencyId, bool showDeleted, bool? newsletter);

        [OperationContract]
        ValidationResult[] ValidateOrder(string title, string firstName, string lastName, string email, string phone, bool newsletter, string cultureName, int countryId, string province, string town, string postalCode, string addressLine, short paymentMethod, string referer, List<ProductQuantity> basket, int invoiceCurrencyId, int? invoiceCountryId, string invoiceProvince, string invoiceTown, string invoicePostalCode, string invoiceAddressLine, string invoiceFullName);

        [OperationContract]
        int UpdateUserPreferences(int personId, string title, string firstName, string lastName, string email, string phone, string fax, bool newsletter, string cultureName);

        [OperationContract]
        int SendNewsletter(string templateName, int[] recipientPersonIds, object[] parameters);
    }
}
