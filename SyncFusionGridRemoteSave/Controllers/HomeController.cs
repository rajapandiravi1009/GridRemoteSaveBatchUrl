using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using SyncFusionGridRemoteSave.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace SyncFusionGridRemoteSave.Controllers
{
    public class HomeController : Controller
    {
        int counter = 0;
        public static List<BigData> order = new List<BigData>();
        public IActionResult Index()
        {
            var ordera = OrdersDetails.GetAllRecords();
            ViewBag.datasource = ordera.ToArray();
            ViewBag.dataSource2 = Employee1Details.GetAllRecords().ToList();
            ViewBag.DataSource = BigData.GetAllRecords().ToArray();
            return View();
        }

        public IActionResult BatchUpdate([FromBody] SyncfusionOperationModel<BigData> batchmodel)
        {
            if (batchmodel.Changed != null)
            {
                for (var i = 0; i < batchmodel.Changed.Count(); i++)
                {
                    var ord = batchmodel.Changed[i];
                    BigData val = BigData.GetAllRecords().Where(or => or.OrderID == ord.OrderID).FirstOrDefault();
                    val.OrderID = ord.OrderID;
                    val.CustomerID = ord.CustomerID;
                    val.Freight = ord.Freight;
                    val.ShipCountry = ord.ShipCountry;
                    val.ShipName = ord.ShipName;
                    val.N2 = ord.N2;
                }
            }

            if (batchmodel.Deleted != null)
            {
                for (var i = 0; i < batchmodel.Deleted.Count(); i++)
                {
                    BigData.GetAllRecords().Remove(order.Where(or => or.OrderID == batchmodel.Deleted[i].OrderID).FirstOrDefault());
                }
            }

            if (batchmodel.Added != null)
            {
                for (var i = 0; i < batchmodel.Added.Count(); i++)
                {
                    BigData.GetAllRecords().Insert(0, batchmodel.Added[i]);
                }
            }
            var data = order.ToList();
            return Json(new { added = batchmodel.Added, changed = batchmodel.Changed, deleted = batchmodel.Deleted, value = batchmodel.Value, action = batchmodel.Action, key = batchmodel.Key });

        }

        public class SyncfusionOperationModel<T> where T : class

        {

            [JsonProperty("action")]

            public string Action { get; set; }

            [JsonProperty("table")]

            public string Table { get; set; }

            [JsonProperty("keyColumn")]

            public string KeyColumn { get; set; }

            [JsonProperty("key")]

            public object Key { get; set; }

            [JsonProperty("value")]

            public T Value { get; set; }

            [JsonProperty("added")]

            public IList<T> Added { get; set; }

            [JsonProperty("changed")]

            public IList<T> Changed { get; set; }

            [JsonProperty("deleted")]

            public IList<T> Deleted { get; set; }

            [JsonProperty("params")]

            public IDictionary<string, object> @params { get; set; }

        }

        public class BigData
        {
            public static List<BigData> order = new List<BigData>();
            public BigData()
            {

            }
            public BigData(int OrderID, string Id, int N1, int N2, string CustomerID, int QuestionTypeId, double Freight, bool Verified, DateTime? OrderDate, string ShipCity, string ShipName, string ShipCountry, DateTime? ShippedDate, string ShipAddress)
            {
                this.OrderID = OrderID;
                this.Id = Id;
                this.N1 = N1;
                this.N2 = N2;
                this.CustomerID = CustomerID;
                this.QuestionTypeId = QuestionTypeId;
                this.Freight = Freight;
                this.ShipCity = ShipCity;
                this.Verified = Verified;
                this.OrderDate = OrderDate;
                this.ShipName = ShipName;
                this.ShipCountry = ShipCountry;
                this.ShippedDate = ShippedDate;
                this.ShipAddress = ShipAddress;
            }
            public static List<BigData> GetAllRecords()
            {
                if (order.Count() == 0)
                {
                    int code = 10000;
                    for (int i = 1; i < 7; i++)
                    {
                        order.Add(new BigData(code + 1, "1,2", 15, 10, "ALFKI", 4, 1112.3 * i, false, DateTime.Now, "#ff00ff", "Simons bistro", "Denmark", new DateTime(1996, 7, 16), "Kirchgasse 6"));
                        order.Add(new BigData(code + 2, "2,3", 20, 8, "ANATR", 2, 456433.3 * i, true, new DateTime(1990, 04, 04), "#ffee00", "Queen Cozinha", "Brazil", new DateTime(1996, 9, 11), "Avda. Azteca 123"));
                        order.Add(new BigData(code + 3, "1,3", 22, 15, "ANTON", 1, 6544.3 * i, true, new DateTime(1957, 11, 30), "#110011", "Frankenversand", "Germany", new DateTime(1996, 10, 7), "Carrera 52 con Ave. Bolívar #65-98 Llano Largo"));
                        order.Add(new BigData(code + 4, "4,2", 18, 11, "BLONP", 3, 455.3 * i, false, new DateTime(1930, 10, 22), "#ff5500", "Ernst Handel", "Austria", new DateTime(1996, 12, 30), "Magazinweg 7"));
                        order.Add(new BigData(code + 5, "3,5", 26, 13, "BOLID", 4, 63.3 * i, true, new DateTime(1953, 02, 18), "#aa0088", "Hanari Carnes", "Switzerland", new DateTime(1997, 12, 3), "1029 - 12th Ave. S."));
                        code += 5;
                    }
                }
                return order;
            }
            public int? OrderID { get; set; }
            public string Id { get; set; }
            public int? N1 { get; set; }
            public int? N2 { get; set; }
            public string CustomerID { get; set; }
            public int? QuestionTypeId { get; set; }
            public double? Freight { get; set; }
            public string ShipCity { get; set; }
            public bool Verified { get; set; }
            public DateTime? OrderDate { get; set; }
            public string ShipName { get; set; }
            public string ShipCountry { get; set; }
            public DateTime? ShippedDate { get; set; }
            public string ShipAddress { get; set; }
        }

        public ActionResult Update([FromBody] CRUDModel<OrdersDetails> value)
        {
            var ord = value.value;
            OrdersDetails val = OrdersDetails.GetAllRecords().Where(or => or.OrderID == ord.OrderID).FirstOrDefault();
            val.OrderID = ord.OrderID;
            val.EmployeeID = ord.EmployeeID;
            val.CustomerID = ord.CustomerID;
            val.Freight = ord.Freight;
            val.OrderDate = ord.OrderDate;
            val.ShipCity = ord.ShipCity;

            return Json(value.value);
        }
        //insert the record
        public ActionResult Insert([FromBody] CRUDModel<OrdersDetails> value)
        {
            value.value.OrderID = counter + 1;
            OrdersDetails.GetAllRecords().Insert(0, value.value);
            return Json(value.value);
        }
        //Delete the record
        public ActionResult Delete([FromBody] CRUDModel<OrdersDetails> value)
        {
            OrdersDetails.GetAllRecords().Remove(OrdersDetails.GetAllRecords().Where(or => or.OrderID == int.Parse(value.key.ToString())).FirstOrDefault());
            return Json(value);
        }


        public class Data
        {

            public bool requiresCounts { get; set; }
            public int skip { get; set; }
            public int take { get; set; }
        }
        public class CRUDModel<T> where T : class
        {
            public string action { get; set; }

            public string table { get; set; }

            public string keyColumn { get; set; }

            public object key { get; set; }

            public T value { get; set; }

            public List<T> added { get; set; }

            public List<T> changed { get; set; }

            public List<T> deleted { get; set; }

            public IDictionary<string, object> @params { get; set; }
        }
    }

    public class Employee1Details
    {
        public static List<Employee1Details> order = new List<Employee1Details>();
        public Employee1Details()
        {

        }
        public Employee1Details(int EmployeeId, string FirstName, string LastName, int ReportsTO)
        {
            this.EmployeeID = EmployeeId;
            this.FirstName = FirstName;
            this.LastName = LastName;
            this.ReportsTo = ReportsTo;
        }
        public static List<Employee1Details> GetAllRecords()
        {
            if (order.Count() == 0)
            {
                int code = 10000;
                for (int i = 1; i < 2; i++)
                {
                    order.Add(new Employee1Details(1, "Nancy", "Davolio", i + 0));
                    order.Add(new Employee1Details(2, "Andrew", "Fuller", i + 0));
                    order.Add(new Employee1Details(3, "Janet", "Leverling", i + 0));
                    order.Add(new Employee1Details(4, "Margaret", "Peacock", i + 0));
                    order.Add(new Employee1Details(5, "John", "Dev", i + 0));
                    code += 5;
                }
            }
            return order;
        }


        public int? EmployeeID { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public int? ReportsTo { get; set; }
    }
    public class OrdersDetails
    {
        public static List<OrdersDetails> order = new List<OrdersDetails>();
        public OrdersDetails()
        {

        }
        public OrdersDetails(int OrderID, string CustomerId, string UserID, int EmployeeId, double Freight, bool Deny, bool Grant, DateTime OrderDate, string ShipCity, string ShipName, string ShipCountry, DateTime ShippedDate, string ShipAddress)
        {
            this.OrderID = OrderID;
            this.CustomerID = CustomerId;
            this.UserID = UserID;
            this.EmployeeID = EmployeeId;
            this.Freight = Freight;
            this.ShipCity = ShipCity;
            this.Deny = Deny;
            this.Grant = Grant;
            this.OrderDate = OrderDate;
            this.ShipName = ShipName;
            this.ShipCountry = ShipCountry;
            this.ShippedDate = ShippedDate;
            this.ShipAddress = ShipAddress;
        }
        public static List<OrdersDetails> GetAllRecords()
        {
            if (order.Count() == 0)
            {
                int code = 10000;
                for (int i = 1; i < 2; i++)
                {
                    order.Add(new OrdersDetails(code + 1, "ALFKI", "111", 1, 2.3 * i, false, true, new DateTime(1991, 05, 15), "Berlin", "Simons bistro", "Denmark", new DateTime(1996, 7, 16), "Kirchgasse 6"));
                    order.Add(new OrdersDetails(code + 2, "ANATR", "222", 2, 3.3 * i, true, false, new DateTime(1990, 04, 04), "Madrid", "Queen Cozinha", "Brazil", new DateTime(1996, 9, 11), "Avda. Azteca 123"));
                    order.Add(new OrdersDetails(code + 3, "ANTON", "333", 3, 4.3 * i, true, false, new DateTime(1957, 11, 30), "Cholchester", "Frankenversand", "Germany", new DateTime(1996, 10, 7), "Carrera 52 con Ave. Bolívar #65-98 Llano Largo"));
                    order.Add(new OrdersDetails(code + 4, "BLONP", "444", 4, 5.3 * i, false, true, new DateTime(1930, 10, 22), "Marseille", "Ernst Handel", "Austria", new DateTime(1996, 12, 30), "Magazinweg 7"));
                    order.Add(new OrdersDetails(code + 5, "BOLID", "555", 5, 6.3 * i, true, false, new DateTime(1953, 02, 18), "Tsawassen", "Hanari Carnes", "Switzerland", new DateTime(1997, 12, 3), "1029 - 12th Ave. S."));
                    code += 5;
                }
            }
            return order;
        }

        public int? OrderID { get; set; }
        public string CustomerID { get; set; }
        public string UserID { get; set; }
        public int? EmployeeID { get; set; }
        public double? Freight { get; set; }
        public string ShipCity { get; set; }
        public bool Deny { get; set; }
        public DateTime OrderDate { get; set; }

        public string ShipName { get; set; }

        public string ShipCountry { get; set; }

        public DateTime ShippedDate { get; set; }
        public string ShipAddress { get; set; }
        public bool Grant { get; set; }
    }
}