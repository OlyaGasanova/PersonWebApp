using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Services.Description;
using Newtonsoft.Json;
using System.Data;
using System.Xml.Linq;

namespace PersonWebApp.Controllers
{
    public class PersonController : ApiController
    {
        float Current = 1;
        List<String> temp = new List<string>(); 

        public string Get(string args, string name="")
        {

            switch (args)
            {
                case "Names":
                    {
                         return JsonConvert.SerializeObject(getAllNames());
                    }
                case "SingleOperations": {
                        return JsonConvert.SerializeObject(getSingleOperations(name));
                    }
                case "AllOperations":
                    {
                        return JsonConvert.SerializeObject(getAllOperations());
                    }
                case "Currency":
                    {
                        return JsonConvert.SerializeObject(getNewCurrency(name));
                    }
                default:
                    {
                        return JsonConvert.SerializeObject(notFound(args));
                    }
            }
           
        }


        private GetResult getAllNames()
        {
            List<String> Names = new List<string>();

            using (SqlConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings["dbConnectionString"].ConnectionString))
            {


                try
                {
                    connection.Open();
                    SqlDataReader myReader = null;
                    SqlCommand myCommand = new SqlCommand("select Name from Person order by Name",
                                                             connection);
                    myReader = myCommand.ExecuteReader();
                    while (myReader.Read())
                    {
                        Console.WriteLine(myReader["Name"].ToString());
                        Names.Add(myReader["Name"].ToString());

                    }
                }
                catch (SqlException ex)
                {
                    return new GetResult()
                    {
                        Code = ex.ErrorCode,
                        Message = ex.Message
                    };
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.ToString());
                }
                finally
                {
                    connection.Close();
                }
            }
            return new GetResult()
            {
                Code = 200,
                Message = "OK",
                Names=Names
            };
        }


        private GetResult getSingleOperations(string Name)
        {
            List<String> Date = new List<string>();
            List<String> Value = new List<string>();
            List<String> Names = new List<string>();
            List<String> CurrencyNames = new List<string>();

            using (SqlConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings["dbConnectionString"].ConnectionString))
            {


                try
                {
                    connection.Open();
                    SqlDataReader myReader = null;

                    string commandText = "select o.Date, o.Value,  c.Code " +
                                                            "from Currency c inner join Operations o on c.id=o.CurrencyID " +
                                                            "inner join Person p on  o.PersonId=p.Id " +
                                                             "where p.Name='"+Name+"'";
                    
                    SqlCommand mycommand = new SqlCommand(commandText, connection);
                    myReader = mycommand.ExecuteReader();
                    while (myReader.Read())
                    {
                        Date.Add(myReader["Date"].ToString());
                        Value.Add(myReader["Value"].ToString());
                        CurrencyNames.Add(myReader["Code"].ToString());

                    }
                }
                catch (SqlException ex)
                {
                    return new GetResult()
                    {
                        Code = ex.ErrorCode,
                        Message = ex.Message
                    };
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.ToString());
                }
                finally
                {
                    connection.Close();
                }
            }
            temp.Sort();
            return new GetResult()
            {
                Code = 200,
                Message = "OK",
                ChoosenName  = Name,
                Date = Date,
                Value = Value,
                CurrencyNames = CurrencyNames
            };
        }


        private GetResult getAllOperations()
        {
            List<String> Names = new List<string>();
            List<String> Income = new List<string>();
            List<String> Expense = new List<string>();
            List<String> Total = new List<string>();

            using (SqlConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings["dbConnectionString"].ConnectionString))
            {


                try
                {

                      SqlDataReader myReader = null;

                    connection.Open();
                    SqlCommand myCommand = new SqlCommand(@"select Sum(Value * Currency.Ratio) as Sum, Person.Name 
                            from Person join Operations on Person.Id = Operations.PersonId 
                            join Currency on Operations.CurrencyId = Currency.Id 
                            Where Value<0 
                            Group by Person.Name order by Person.Name", connection);
                    myReader = myCommand.ExecuteReader();
                    while (myReader.Read())
                    {
                        // Console.WriteLine(myReader["Name"].ToString());
                        Names.Add(myReader["Name"].ToString());
                        Expense.Add(myReader["Sum"].ToString());

                    }
                    connection.Close();

                    connection.Open();
                    myCommand = new SqlCommand("select Sum(Value * Currency.Ratio) as Sum, Person.Name " +
                            "from Person join Operations on Person.Id = Operations.PersonId " +
                            "join Currency on Operations.CurrencyId = Currency.Id " +
                            "Where Value>0 " +
                            "Group by Person.Name order by Person.Name", connection);
                    myReader = myCommand.ExecuteReader();
                    while (myReader.Read())
                    {
                        Income.Add(myReader["Sum"].ToString());

                    }
                    connection.Close();


                    connection.Open();
                    myCommand = new SqlCommand("select Sum(Value * Currency.Ratio) as Sum, Person.Name " +
                            "from Person join Operations on Person.Id = Operations.PersonId " +
                            "join Currency on Operations.CurrencyId = Currency.Id " +
                            "Group by Person.Name order by Person.Name", connection);
                    myReader = myCommand.ExecuteReader();
                    while (myReader.Read())
                    {
                        Total.Add(myReader["Sum"].ToString());

                    }
                    connection.Close();

                }
                catch (SqlException ex)
                {
                    return new GetResult()
                    {
                        Code = ex.ErrorCode,
                        Message = ex.Message
                    };
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.ToString());
                }
                finally
                {
                    connection.Close();
                }
            }
            temp.Sort();
            return new GetResult()
            {
                Code = 200,
                Income= Income,
                Expense = Expense,
                Total = Total,
                Names = Names
            };
        }


        private GetResult getNewCurrency(string ChoosenCurrecy)
        {
              List<String> Message = new List<string>();
              List<String> Code = new List<string>();
              List<String> CurrencyNames = new List<string>();
              List<String> Currency = new List<string>();
              List<String> Nominal = new List<string>();

              

              using (SqlConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings["dbConnectionString"].ConnectionString))
              {

                    Current = 1; //для добавления ratio рубля, т.к. xml от цбр не содержит данного значения

                  try
                  {
                    SqlCommand myCommand;
                    SqlDataReader myReader = null;

                    connection.Open();
                    myCommand = new SqlCommand(@"select Code from Currency", connection);
                    myReader = myCommand.ExecuteReader();
                    //имеющиеся в базе валюты
                    while (myReader.Read())
                    {
                        Code.Add(myReader["Code"].ToString());

                    }
                    connection.Close();


                    var document = XDocument.Load("http://www.cbr.ru/scripts/XML_daily.asp");
                    var element = document.Root.Elements();
                    float delitel = 1;
                    foreach (var el in element)
                    {
                        if (Code.Contains(el.Element("CharCode").Value))
                        {
                            
                            Currency.Add((Convert.ToSingle(el.Element("Value").Value) / Convert.ToSingle(el.Element("Nominal").Value)).ToString());
                            CurrencyNames.Add(el.Element("CharCode").Value);
                            if (el.Element("CharCode").Value == ChoosenCurrecy)
                                delitel = (Convert.ToSingle(el.Element("Value").Value) / Convert.ToSingle(el.Element("Nominal").Value));
                        }
                    }

                    var RUBRatio = 1;
                    Currency.Add(RUBRatio.ToString());
                    CurrencyNames.Add("RUB");

                    //сначала курсы обновляются относительно рубля - для получения актуальных данных и избежания постепенно
                    //возврастающей погрешности, возникающей во время деления
                    string text = "";
                    for (int i = 0; i < CurrencyNames.Count; i++)
                    {
                        text += " Update Currency Set Ratio = (" + Currency[i] + ") where Code='" + CurrencyNames[i] + "'";
                    } 

                    text = text.Replace(",", ".");

                    connection.Open();
                    myCommand = new SqlCommand(text, connection);
                    myCommand.ExecuteNonQuery();
                    connection.Close();

                    //новые курсы относительно выбранно валюты
                    for (int i = 0; i < CurrencyNames.Count; i++)
                    {
                        float c=0;
                        float.TryParse(Currency[i],out c);
                        Currency[i] = (c / delitel).ToString();

                    }



                   text = "";
                   text += " Update Currency Set Ratio = (Ratio/" + delitel + ");";
                   text += " Update Currency set IsBase = 0 where IsBase=1";
                   text += " Update Currency set IsBase = 1 where Code='" + ChoosenCurrecy + "';";
                   text = text.Replace(",", ".");

                    connection.Open();
                    myCommand = new SqlCommand(text, connection);
                    myCommand.ExecuteNonQuery();
                    connection.Close();

                    

                }
                  catch (SqlException ex)
                  {
                      return new GetResult()
                      {
                          Code = ex.ErrorCode,
                          Message = ex.Message
                      };
                  }
                  catch (Exception e)
                  {
                      Console.WriteLine(e.ToString());
                  }
                  finally
                  {
                      connection.Close();
                  }
              }
              return new GetResult()
              {
                  Code = 200,
                  Currency= Currency,
                  CurrencyNames = CurrencyNames,
                  Message="OK"

              };
              
        }



        private GetResult notFound(string handler)
        {
            return new GetResult()
            {
                Code = 404,
                Message = string.Format("Handler {0} not found!", handler)
            };
        }


        public class GetResult
        {

            public int Code { get; set; }
            public String Message { get; set; }
            public String ChoosenName { get; set; }
            public List<String> Date { get; set; } = new List<string>();
            public List<String> Value { get; set; } = new List<string>();
            public List<String> Income { get; set; } = new List<string>();
            public List<String> Expense { get; set; } = new List<string>();
            public List<String> Total { get; set; } = new List<string>();
            public List<String> Names { get; set; } = new List<string>();
            public List<String> CurrencyNames { get; set; } = new List<string>();
            public List<String> Currency { get; set; } = new List<string>();

        }
    }
}
