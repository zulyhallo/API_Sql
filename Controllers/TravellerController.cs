using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Data;
using System.Data.SqlClient;
using Microsoft.Extensions.Configuration;

namespace CRUD_API5.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TravellerController : ControllerBase
    {

        private readonly IConfiguration _configuration;
        public TravellerController(IConfiguration configuration)
        {
            _configuration = configuration;
        }
       
        //API Methods
        [HttpGet]
        public JsonResult Get()
        {
            string query = @"
                           select TravellerID,TravellerName,TravellerCity,TravellerAge from 
                           dbo.Traveller
                           "; // sql komutunu query'e atma
            DataTable table = new DataTable(); // table nesnesi oluturma 
            string sqlDataSource = _configuration.GetConnectionString("TravellerAppCon");//sql bağlantısı 
            SqlDataReader myReader; //get komutu için sqlden verileri alma metodu(sql komutu)
            using (SqlConnection myCon= new SqlConnection(sqlDataSource))
            {
                myCon.Open();// bağlantıyı açma
                using(SqlCommand myCommand=new SqlCommand(query, myCon)) // mycona bağlanıp query'i çalıştırması için
                {
                    myReader = myCommand.ExecuteReader();
                    table.Load(myReader);
                    myReader.Close(); // bağlantıları kapatma 
                    myCon.Close();  
                }
            }
            return new JsonResult(table);

        }
    }
}
