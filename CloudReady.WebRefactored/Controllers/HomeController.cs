using System;
using System.Collections.Generic;
using System.IO;
using CloudReady.WebRefactored.Models;

namespace CloudReady.WebRefactored.Controllers
{
    public class HomeController : Controller
    {
        private readonly string _connectionString = "Data Source=.;Initial Catalog=CloudReady;Trusted_Connection=True";

        public ActionResult Index()
        {
            var images = GetImages();

            return View(images);
        }

        [HttpPost]
        public ActionResult UploadImages(IEnumerable<HttpPostedFileBase> files)
        {
            foreach (var file in files)
            {
                if (file != null && file.ContentLength > 0)
                {
                    var originalFilename = file.FileName;
                    var extension = Path.GetExtension(originalFilename)?.TrimStart() ?? string.Empty;
                    var name = Path.GetFileNameWithoutExtension(originalFilename);
                    var size = file.ContentLength;
                    var id = SaveImage(extension, name, size);

                    var newFilename = $"{id}.{extension}";
                    var path = Path.Combine(Server.MapPath("~/fileuploads/"), newFilename);

                    file.SaveAs(path);
                }
            }

            return RedirectToAction("Index");
        }

        private int SaveImage(string extension, string name, int size)
        {
            using (var connection = new SqlConnection(_connectionString))
            using (var command = connection.CreateCommand())
            {
                command.CommandText = @"
                            INSERT INTO Images (Extension, Name, Size)
                            VALUES (@Extension, @Name, @Size);
                            SELECT @@IDENTITY;
                        ";
                command.Parameters.AddWithValue("Extension", extension);
                command.Parameters.AddWithValue("Name", name);
                command.Parameters.AddWithValue("Size", size);

                connection.Open();
                return Convert.ToInt32(command.ExecuteScalar());
            }
        }

        private List<Image> GetImages()
        {
            var images = new List<Image>();

            using (var connection = new SqlConnection(_connectionString))
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

            return images;
        }
    }
}