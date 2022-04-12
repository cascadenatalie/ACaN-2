using EllieMae.Encompass.Automation;
using EllieMae.Encompass.BusinessObjects.Loans;
using EllieMae.Encompass.ComponentModel;
using Newtonsoft.Json;
using System;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace ACaN2
{
    public class SendDisclosureRecord
    {
        private HttpClient _httpClient = new HttpClient();
        private TokenResponse _tokenResponse;
        private string Secret { get; set; }
        private string AuthEndpoint { get; set; }
        private string DocuTechEndpoint { get; set; }
        public static string DocutechResults = null;


        private void GetConnectionString()
        {
            string clientID = ACaN.session.ClientID; 
            switch (clientID)
            {
                case "3000799583":
                    Secret = @"C8pNhtCi51EU";
                    AuthEndpoint = @"https://cascadeauth.cascadeloans.com/connect/token";
                    DocuTechEndpoint = @"https://datagw.cascadeloans.com";
                    break;
                case "3011204678":
                    Secret = @"8uU43n%26YQwBLbrdK";
                    AuthEndpoint = @"https://uatauth.cascadeloans.com/connect/token";
                    DocuTechEndpoint = @"https://uat-datagw.cascadeloans.com";
                    break;
                default:
                    Secret = @"8uU43n%26YQwBLbrdK";
                    AuthEndpoint = @"https://devauth.cascadeloans.com/connect/token";
                    DocuTechEndpoint = @"https://dev-datagw.cascadeloans.com";
                    break;
            }
        }

        public async Task PostToDocuTech(DisclosureTypes disclosureType)
        {
            GetConnectionString();
            var payload = new DocumentDeliveryPayload()
            {
                DocumentType = disclosureType.ToString(),
            };

            var pairCount = ACaN.loan.BorrowerPairs.Count;

            for (var pairIndex = 0; pairIndex < pairCount; pairIndex++)
            {
                var pair = ACaN.loan.BorrowerPairs[pairIndex];

                payload.LoanNumber = ACaN.loan
                    .Fields[EncompassFields.LoanNumber]
                    .GetValueForBorrowerPair(pair);

                payload.LoanGuid = ACaN.loan
                    .Fields[EncompassFields.LoanGuid]
                    .GetValueForBorrowerPair(pair);

                payload.Recipients.Add(GetBorrowerData(pair, pairIndex));

                var coBorrowerSsn = ACaN.loan
                    .Fields[EncompassFields.CoBorrower.Ssn]
                    .GetValueForBorrowerPair(pair);

                var coBorrowerExists = !string.IsNullOrWhiteSpace(coBorrowerSsn);

                if (coBorrowerExists)
                {
                    payload.Recipients.Add(GetCoBorrowerData(pair, pairIndex));
                }
            }
            await Send(payload);
        }

        private Recipient GetBorrowerData(BorrowerPair pair, int pairIndex)
        {
            Recipient borrower = new Recipient()
            {
                PairOrdinal = pairIndex + 1,
                FirstName = ACaN.loan
                    .Fields[EncompassFields.Borrower.FirstName]
                    .GetValueForBorrowerPair(pair),
                LastName = ACaN.loan
                    .Fields[EncompassFields.Borrower.LastName]
                    .GetValueForBorrowerPair(pair)
            };
            bool BorrHasMailingAddress()
            {
                return 
                    !ACaN.loan.Fields[EncompassFields.Borrower.MailingAddress.Street].GetValueForBorrowerPair(pair).Equals("")&&
                    !ACaN.loan.Fields[EncompassFields.Borrower.MailingAddress.City].GetValueForBorrowerPair(pair).Equals("") &&
                    !ACaN.loan.Fields[EncompassFields.Borrower.MailingAddress.State].GetValueForBorrowerPair(pair).Equals("") &&
                    !ACaN.loan.Fields[EncompassFields.Borrower.MailingAddress.PostalCode].GetValueForBorrowerPair(pair).Equals("");
            }
            if (BorrHasMailingAddress())
            {
                {
                    borrower.Address = new Address()
                    {
                        Street = ACaN.loan
                            .Fields[EncompassFields.Borrower.MailingAddress.Street]
                            .GetValueForBorrowerPair(pair),
                        Unit = ACaN.loan
                            .Fields[EncompassFields.Borrower.MailingAddress.Unit]
                            .GetValueForBorrowerPair(pair),
                        City = ACaN.loan
                            .Fields[EncompassFields.Borrower.MailingAddress.City]
                            .GetValueForBorrowerPair(pair),
                        State = ACaN.loan
                            .Fields[EncompassFields.Borrower.MailingAddress.State]
                            .GetValueForBorrowerPair(pair),
                        PostalCode = ACaN.loan
                            .Fields[EncompassFields.Borrower.MailingAddress.PostalCode]
                            .GetValueForBorrowerPair(pair)
                    };
                    //Leaving here for future troubleshooting if needed
                    //Console.WriteLine("Borr Mailing Address Used");
                    //Console.WriteLine($"Borr Street: {borrower.Address.Street}");
                    //Console.WriteLine($"Borr Unit: {borrower.Address.Unit}");
                    //Console.WriteLine($"Borr City: {borrower.Address.City}");
                    //Console.WriteLine($"Borr State: {borrower.Address.State}");
                    //Console.WriteLine($"Borr Postal Code: {borrower.Address.PostalCode}\n");
                }
            }
            else
            {
                {
                    borrower.Address = new Address()
                    {
                        Street = ACaN.loan
                            .Fields[EncompassFields.Borrower.CurrentAddress.Street]
                            .GetValueForBorrowerPair(pair),
                        Unit = ACaN.loan
                            .Fields[EncompassFields.Borrower.CurrentAddress.Unit]
                            .GetValueForBorrowerPair(pair),
                        City = ACaN.loan
                            .Fields[EncompassFields.Borrower.CurrentAddress.City]
                            .GetValueForBorrowerPair(pair),
                        State = ACaN.loan
                            .Fields[EncompassFields.Borrower.CurrentAddress.State]
                            .GetValueForBorrowerPair(pair),
                        PostalCode = ACaN.loan
                            .Fields[EncompassFields.Borrower.CurrentAddress.PostalCode]
                            .GetValueForBorrowerPair(pair)
                    };
                    //Leaving here for future troubleshooting if needed
                    //Console.WriteLine("Borr Current Address Used");
                    //Console.WriteLine($"Borr Street: {borrower.Address.Street}");
                    //Console.WriteLine($"Borr Unit: {borrower.Address.Unit}");
                    //Console.WriteLine($"Borr City: {borrower.Address.City}");
                    //Console.WriteLine($"Borr State: {borrower.Address.State}");
                    //Console.WriteLine($"Borr Postal Code: {borrower.Address.PostalCode}\n");
                }
            }
            return borrower;
        }



        private Recipient GetCoBorrowerData(BorrowerPair pair, int pairIndex)
        {
            Recipient coBorrower = new Recipient()
            {
                PairOrdinal = pairIndex + 1,
                FirstName = ACaN.loan
                    .Fields[EncompassFields.CoBorrower.FirstName]
                    .GetValueForBorrowerPair(pair),
                LastName = ACaN.loan
                    .Fields[EncompassFields.CoBorrower.LastName]
                    .GetValueForBorrowerPair(pair),
            };
            bool CoBorrHasMailingAddress()
            {
                return
                    !ACaN.loan.Fields[EncompassFields.CoBorrower.MailingAddress.Street].GetValueForBorrowerPair(pair).Equals("") &&
                    !ACaN.loan.Fields[EncompassFields.CoBorrower.MailingAddress.City].GetValueForBorrowerPair(pair).Equals("") &&
                    !ACaN.loan.Fields[EncompassFields.CoBorrower.MailingAddress.State].GetValueForBorrowerPair(pair).Equals("") &&
                    !ACaN.loan.Fields[EncompassFields.CoBorrower.MailingAddress.PostalCode].GetValueForBorrowerPair(pair).Equals("");
            }
            if (CoBorrHasMailingAddress())
            {
                {
                    coBorrower.Address = new Address()
                    {
                        Street = ACaN.loan
                            .Fields[EncompassFields.CoBorrower.MailingAddress.Street]
                            .GetValueForBorrowerPair(pair),
                        Unit = ACaN.loan
                            .Fields[EncompassFields.CoBorrower.MailingAddress.Unit]
                            .GetValueForBorrowerPair(pair),
                        City = ACaN.loan
                            .Fields[EncompassFields.CoBorrower.MailingAddress.City]
                            .GetValueForBorrowerPair(pair),
                        State = ACaN.loan
                            .Fields[EncompassFields.CoBorrower.MailingAddress.State]
                            .GetValueForBorrowerPair(pair),
                        PostalCode = ACaN.loan
                            .Fields[EncompassFields.CoBorrower.MailingAddress.PostalCode]
                            .GetValueForBorrowerPair(pair)
                    };
                    //Leaving here for future troubleshooting if needed
                    //Console.WriteLine("CoBorr Mailing Address Used");
                    //Console.WriteLine($"Borr Street: {coBorrower.Address.Street}");
                    //Console.WriteLine($"Borr Unit: {coBorrower.Address.Unit}");
                    //Console.WriteLine($"Borr City: {coBorrower.Address.City}");
                    //Console.WriteLine($"Borr State: {coBorrower.Address.State}");
                    //Console.WriteLine($"Borr Postal Code: {coBorrower.Address.PostalCode}\n");
                }
            }
            else
            {
                coBorrower.Address = new Address()
                {
                    Street = ACaN.loan
                        .Fields[EncompassFields.CoBorrower.CurrentAddress.Street]
                        .GetValueForBorrowerPair(pair),
                    Unit = ACaN.loan
                        .Fields[EncompassFields.CoBorrower.CurrentAddress.Unit]
                        .GetValueForBorrowerPair(pair),
                    City = ACaN.loan
                        .Fields[EncompassFields.CoBorrower.CurrentAddress.City]
                        .GetValueForBorrowerPair(pair),
                    State = ACaN.loan
                        .Fields[EncompassFields.CoBorrower.CurrentAddress.State]
                        .GetValueForBorrowerPair(pair),
                    PostalCode = ACaN.loan
                        .Fields[EncompassFields.CoBorrower.CurrentAddress.PostalCode]
                        .GetValueForBorrowerPair(pair)
                };
                //Leaving here for future troubleshooting if needed
                //Console.WriteLine("CoBorr Current Address Used");
                //Console.WriteLine($"Borr Street: {coBorrower.Address.Street}");
                //Console.WriteLine($"Borr Unit: {coBorrower.Address.Unit}");
                //Console.WriteLine($"Borr City: {coBorrower.Address.City}");
                //Console.WriteLine($"Borr State: {coBorrower.Address.State}");
                //Console.WriteLine($"Borr Postal Code: {coBorrower.Address.PostalCode}\n");
            }
            return coBorrower;
        }
        private async Task Send(DocumentDeliveryPayload payload)
        {
            await GetAuthToken();
            if (_httpClient.BaseAddress == null)
            {
                _httpClient.BaseAddress = new Uri(DocuTechEndpoint);
            }
            _httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", _tokenResponse.access_token);
            var json = JsonConvert.SerializeObject(payload);
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            var result = await _httpClient.PostAsync("/document-deliveries/api/v1/shared-drive-documents", content);
            DocutechResults = result.StatusCode.ToString();
            Console.WriteLine($"DocuTech Results = {DocutechResults}");
            //Keeping this here if needed for future troubleshooting
            //Console.WriteLine($"{json}\n\n{result.StatusCode.ToString()}");
        }

        private async Task GetAuthToken()
        {
            string postData = $"client_id=encompass_service&client_secret={Secret}&scope=documents_delivery_api&grant_type=client_credentials";
            ASCIIEncoding encoding = new ASCIIEncoding();
            byte[] encodedData = encoding.GetBytes(postData);
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(AuthEndpoint);
            request.ContentType = "application/x-www-form-urlencoded";
            request.ContentLength = encodedData.Length;
            request.Method = "POST";

            Stream stream = request.GetRequestStream();
            stream.Write(encodedData, 0, encodedData.Length);

            var httpResponse = await request.GetResponseAsync();
            using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
            {
                var result = streamReader.ReadToEnd();
                _tokenResponse = JsonConvert.DeserializeObject<TokenResponse>(result.ToString());
            }
        }
    }
}

