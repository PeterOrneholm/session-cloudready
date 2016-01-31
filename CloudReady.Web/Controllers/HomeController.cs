using System.Collections.Generic;
using System.Data.SqlClient;
using System.IO;
using System.Web;
using System.Web.Mvc;
using CloudReady.Web.Models;

namespace CloudReady.Web.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            var images = new List<Image>();

            using (var connection = new SqlConnection("Data Source=.;Initial Catalog=CloudReady;Trusted_Connection=True"))
            using (var command = connection.CreateCommand())
            {
                command.CommandText = "SELECT * FROM Images";
                connection.Open();
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        var id = (int)reader["Id"];
                        var extension = (string)reader["Extension"];

                        images.Add(new Image
                        {
                            Id = id,
                            Extension = extension,
                            Name = (string)reader["Name"],
                            Size = (int)reader["Size"],
                            Url = $"/fileuploads/{id}.{extension}"
                        });
                    }
                }
            }

            return View(images);
        }

        [HttpPost]
        public ActionResult UploadImages(IEnumerable<HttpPostedFileBase> files)
        {
            foreach (var file in files)
            {
                if (file.ContentLength > 0)
                {
                    var fileName = Path.GetFileName(file.FileName);
                    var path = Path.Combine(Server.MapPath("~/fileuploads/"), fileName);
                    file.SaveAs(path);
                }
            }

            return RedirectToAction("Index");
        }
    }
}