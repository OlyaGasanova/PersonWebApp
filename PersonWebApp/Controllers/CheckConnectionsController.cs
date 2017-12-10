using System;
using System.Configuration;
using System.Data.SqlClient;
using System.Web.Http;

using Newtonsoft.Json;

namespace PersonWebApp.Controllers {

    public class CheckConnectionsController : ApiController {

        public string Get(string args) {
            switch (args) {
                case "SqlConnection": {
                        return JsonConvert.SerializeObject(sqlConnectionCheck());
                    }
                case "ApiConnection": {
                        return JsonConvert.SerializeObject(apiConnectionCheck());
                    }
                default: {
                        return JsonConvert.SerializeObject(notFound(args));
                    }
            }
        }

        private ConnectionCheckResult sqlConnectionCheck () {
            using (SqlConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings["dbConnectionString"].ConnectionString)) {
                try {
                    connection.Open();
                } catch (SqlException ex) {
                    return new ConnectionCheckResult() {
                        Code = ex.ErrorCode,
                        Message = ex.Message
                    };
                } finally {
                    connection.Close();
                }
            }
            return new ConnectionCheckResult() {
                Code = 200,
                Message = "OK"
            };
        }

        private ConnectionCheckResult apiConnectionCheck () {
            return new ConnectionCheckResult() {
                Code = 200,
                Message = "OK"
            };
        }

        private ConnectionCheckResult notFound (string handler) {
            return new ConnectionCheckResult() {
                Code = 404,
                Message = string.Format("Handler {0} not found!", handler)
            };
        }

    }

    public class ConnectionCheckResult {

        public int Code { get; set; }
        public string Message { get; set; }

    }

}