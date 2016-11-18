using IntelliPack.DataAccessLayer.DataManagers;
using IntelliPack.DataAccessLayer.Models;
using IntelliPackWeb.Base;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.OleDb;
using System.IO;
using System.Web;
using System.Web.Mvc;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Xml;
using System.Xml.Serialization;

namespace IntelliPackWeb.Controllers
{
    public class BillingController : BaseController
    {
        [Authorize]
        //[RequireHttps]
        public ActionResult RetainedPackage()
        {
            getCookies();
            var SystemPackages = new PackagesManager();
            //Load Data
            var dataInventory = SystemPackages.GetReteinedUnprocceses(userIdLogged);
            if (dataInventory != null)
            {
                return View(dataInventory);
            }
            else
            {
                ViewBag.Message = "Ha ocurrido un error";
                LogError("RetainedPackage", "Objeto Llego Nulo");
            }
            return View(new List<Packages>());
        }
        [Authorize]
        //[RequireHttps]
        public ActionResult Invoice(string fileName)
        {
            try
            {
                getCookies();
                byte[] fileBytes = System.IO.File.ReadAllBytes(Path.Combine(RootUrl + "/facturas", fileName));

                return File(fileBytes, System.Net.Mime.MediaTypeNames.Application.Octet, fileName);
            }
            catch
            {
                return Json("El Archivo no existe.", JsonRequestBehavior.AllowGet);
            }
        }
        [HttpPost]
        [Authorize]
        //[RequireHttps]
        public ActionResult RetainedPackage(string message)
        {
            getCookies();
            var SystemPackages = new PackagesManager();
            string path = "";
                       
            try
            {
                DataSet ds = new DataSet();
                for (int b = 0; b < Request.Files.Count; b++)
                {
                    var file = Request.Files[b];

                    var extension = Path.GetExtension(file.FileName).Replace(".", "");

                    var fileName = Guid.NewGuid().ToString() + "." + extension;
                    if (extension == "xls" || extension == "xlsx")
                    {
                        string fileLocation = RootUrl + "/" + fileName;
                        if (System.IO.File.Exists(fileLocation))
                        {

                            System.IO.File.Delete(fileLocation);
                        }
                        file.SaveAs(fileLocation);
                        string excelConnectionString = string.Empty;
                        excelConnectionString = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" +
                        fileLocation + ";Extended Properties=\"Excel 12.0;HDR=Yes;IMEX=2\"";
                        //connection String for xls file format.
                        if (extension == "xls")
                        {
                            excelConnectionString = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" +
                            fileLocation + ";Extended Properties=\"Excel 8.0;HDR=Yes;IMEX=2\"";
                        }
                        //connection String for xlsx file format.
                        else if (extension == "xlsx")
                        {
                            excelConnectionString = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" +
                            fileLocation + ";Extended Properties=\"Excel 12.0;HDR=Yes;IMEX=2\"";
                        }
                        //Create Connection to Excel work book and add oledb namespace
                        OleDbConnection excelConnection = new OleDbConnection(excelConnectionString);
                        excelConnection.Open();
                        DataTable dt = new DataTable();

                        dt = excelConnection.GetOleDbSchemaTable(OleDbSchemaGuid.Tables, null);
                        if (dt == null)
                        {
                            return null;
                        }

                        String[] excelSheets = new String[dt.Rows.Count];
                        int t = 0;
                        //excel data saves in temp file here.
                        foreach (DataRow row in dt.Rows)
                        {
                            excelSheets[t] = row["TABLE_NAME"].ToString();
                            t++;
                        }
                        OleDbConnection excelConnection1 = new OleDbConnection(excelConnectionString);


                        string query = string.Format("Select * from [{0}]", excelSheets[0]);
                        using (OleDbDataAdapter dataAdapter = new OleDbDataAdapter(query, excelConnection1))
                        {
                            dataAdapter.Fill(ds);
                        }
                    }
                    if (extension.ToString().ToLower().Equals("xml"))
                    {
                        string fileLocation = path = RootUrl + "/" + file.FileName;
                        if (System.IO.File.Exists(fileLocation))
                        {
                            System.IO.File.Delete(fileLocation);
                        }

                        file.SaveAs(fileLocation);
                        XmlTextReader xmlreader = new XmlTextReader(fileLocation);
                        // DataSet ds = new DataSet();
                        ds.ReadXml(xmlreader);
                        xmlreader.Close();
                    }

                    for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                    {
                        decimal peso = 0, manejo = 0, costoXLibra = 0, valorMercancia = 0, precioXLibraCliente = 0, SeguroMonto = 0, CostoTotal = 0, itbis_pagado = 0;
                        decimal.TryParse(ds.Tables[0].Rows[i]["peso"].ToString(), out peso);
                        decimal.TryParse(ds.Tables[0].Rows[i]["manejo"].ToString(), out manejo);
                        decimal.TryParse(ds.Tables[0].Rows[i]["costoXLibra"].ToString(), out costoXLibra);
                        decimal.TryParse(ds.Tables[0].Rows[i]["ValorMercancia"].ToString(), out valorMercancia);
                        decimal.TryParse(ds.Tables[0].Rows[i]["SeguroMonto"].ToString(), out SeguroMonto);
                        decimal.TryParse(ds.Tables[0].Rows[i]["CostoTotal"].ToString(), out CostoTotal);
                        decimal.TryParse(ds.Tables[0].Rows[i]["itbis_pagado"].ToString(), out itbis_pagado);
                        decimal.TryParse(ds.Tables[0].Rows[i]["precioXLibraCliente"].ToString(), out precioXLibraCliente);
                        
                        var PackageToSave = new Packages()
                        {

                            tracking_code = ds.Tables[0].Rows[i]["tracking_code"].ToString(),
                            peso = peso,
                            WH = ds.Tables[0].Rows[i]["WH"].ToString(),
                            manejo = manejo,
                            costoXLibra = costoXLibra,
                            ValorMercancia = valorMercancia,
                            SeguroMonto = SeguroMonto,
                            CostoTotal = CostoTotal,
                            itbis_pagado = itbis_pagado,
                            precioXLibraCliente = precioXLibraCliente
                        };
                        SystemPackages.UpdateRetained(PackageToSave);
                        UsersManager user = new UsersManager();
                        var singleUser = user.GetUsers(PackageToSave.usersId);
                        string body = System.IO.File.ReadAllText(RootUrl + "/TemplateNotificationPackageWithPrices.html");
                        body = string.Format(body, singleUser.name + " " + singleUser.last_name, PackageToSave.tracking_code, PackageToSave.contenido, PackageToSave.peso, PackageToSave.total);
                        SendEmail("Notificación Estado De Paquetes De IntelliPaq ", singleUser.email, body, true);
                    }
                }
            }
            catch (Exception ex)
            {
                LogError("Billing Carga De Precios", ex.Message);
            }

            try
            {
                System.IO.File.Delete(path);
            }
            catch
            { }
            //Load Data
            var dataInventory = SystemPackages.GetReteinedUnprocceses(userIdLogged);
            if (dataInventory != null)
            {
                ViewBag.Message = "Proceso Completado Exitosamente";
                return View(dataInventory);
            }
            else
            {
                ViewBag.Message = "Ocurrio un error: Intentelo mas tarde";
                LogError("RetainedPackage", "Objeto Llego Nulo");
            }
            return View(new List<Packages>());
        }
        [Authorize]
        //[RequireHttps]
        public ActionResult UploadPackages()
        {
            getCookies();
            return View();
        }
        [Authorize]
        //[RequireHttps]
        public ActionResult UploadPrices()
        {
            getCookies();
            return View();
        }
        [HttpPost]
        [Authorize]
        //[RequireHttps]
        public ActionResult UploadPrices(string mesage )
        {
            try
            {
                getCookies();
                int userId = userIdLogged;
                DataSet ds = new DataSet();
                try
                {
                    string path = "";
                    for (int i = 0; i < Request.Files.Count; i++)
                    {
                        var file = Request.Files[i];

                        var extension = Path.GetExtension(file.FileName).Replace(".", "");

                        var fileName = Guid.NewGuid().ToString() + "." + extension;
                        if (extension == "xls" || extension == "xlsx")
                        {
                            string fileLocation = RootUrl + "/" + fileName;
                            if (System.IO.File.Exists(fileLocation))
                            {

                                System.IO.File.Delete(fileLocation);
                            }
                            file.SaveAs(fileLocation);
                            string excelConnectionString = string.Empty;
                            excelConnectionString = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" +
                            fileLocation + ";Extended Properties=\"Excel 12.0;HDR=Yes;IMEX=2\"";
                            //connection String for xls file format.
                            if (extension == "xls")
                            {
                                excelConnectionString = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" +
                                fileLocation + ";Extended Properties=\"Excel 8.0;HDR=Yes;IMEX=2\"";
                            }
                            //connection String for xlsx file format.
                            else if (extension == "xlsx")
                            {
                                excelConnectionString = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" +
                                fileLocation + ";Extended Properties=\"Excel 12.0;HDR=Yes;IMEX=2\"";
                            }
                            //Create Connection to Excel work book and add oledb namespace
                            OleDbConnection excelConnection = new OleDbConnection(excelConnectionString);
                            excelConnection.Open();
                            DataTable dt = new DataTable();

                            dt = excelConnection.GetOleDbSchemaTable(OleDbSchemaGuid.Tables, null);
                            if (dt == null)
                            {
                                return null;
                            }

                            String[] excelSheets = new String[dt.Rows.Count];
                            int t = 0;
                            //excel data saves in temp file here.
                            foreach (DataRow row in dt.Rows)
                            {
                                excelSheets[t] = row["TABLE_NAME"].ToString();
                                t++;
                            }
                            OleDbConnection excelConnection1 = new OleDbConnection(excelConnectionString);


                            string query = string.Format("Select * from [{0}]", excelSheets[0]);
                            using (OleDbDataAdapter dataAdapter = new OleDbDataAdapter(query, excelConnection1))
                            {
                                dataAdapter.Fill(ds);
                            }
                        }
                        if (extension.ToString().ToLower().Equals("xml"))
                        {
                            string fileLocation = RootUrl + "/" + file.FileName;
                            if (System.IO.File.Exists(fileLocation))
                            {
                                System.IO.File.Delete(fileLocation);
                            }

                            file.SaveAs(fileLocation);
                            XmlTextReader xmlreader = new XmlTextReader(fileLocation);
                            // DataSet ds = new DataSet();
                            ds.ReadXml(xmlreader);
                            xmlreader.Close();
                        }

                        for (int b = 0; b < ds.Tables[0].Rows.Count; b++)
                        {
                            decimal peso = 0, manejo = 0, costoXLibra = 0, valorMercancia = 0, precioXLibraCliente = 0, SeguroMonto = 0, CostoTotal = 0, itbis_pagado = 0;
                            decimal.TryParse(ds.Tables[0].Rows[b]["peso"].ToString(), out peso);
                            decimal.TryParse(ds.Tables[0].Rows[b]["manejo"].ToString(), out manejo);
                            decimal.TryParse(ds.Tables[0].Rows[b]["costoXLibra"].ToString(), out costoXLibra);
                            decimal.TryParse(ds.Tables[0].Rows[b]["ValorMercancia"].ToString(), out valorMercancia);
                            decimal.TryParse(ds.Tables[0].Rows[b]["SeguroMonto"].ToString(), out SeguroMonto);
                            decimal.TryParse(ds.Tables[0].Rows[b]["CostoTotal"].ToString(), out CostoTotal);
                            decimal.TryParse(ds.Tables[0].Rows[b]["itbis_pagado"].ToString(), out itbis_pagado);
                            decimal.TryParse(ds.Tables[0].Rows[b]["precioXLibraCliente"].ToString(), out precioXLibraCliente);
                            int userid = 0;
                            int.TryParse(ds.Tables[0].Rows[b]["usersId"].ToString(), out userid);
                            var SystemPackages = new PackagesManager();
                            var PackageToSave = new Packages()
                            {
                                consignado = ds.Tables[0].Rows[b]["consignado"].ToString(),
                                contenido = ds.Tables[0].Rows[b]["contenido"].ToString(),
                                tienda = ds.Tables[0].Rows[b]["tienda"].ToString(),
                                usersId = userid,
                                tracking_code = ds.Tables[0].Rows[b]["tracking_code"].ToString(),
                                peso = peso,
                                WH = ds.Tables[0].Rows[b]["WH"].ToString(),
                                manejo = manejo,
                                costoXLibra = costoXLibra,
                                ValorMercancia = valorMercancia,
                                SeguroMonto = SeguroMonto,
                                CostoTotal = CostoTotal,
                                itbis_pagado = itbis_pagado,
                                precioXLibraCliente = precioXLibraCliente,
                                status_code = 6,
                                workflowid = 0,
                                packageStatus = 2,
                                packageStatusFromSourceDesc = "En Transito RD",
                                Moneda = "PESO",
                                courierId = 0,
                                correo = "",
                                origen = ds.Tables[0].Rows[b]["origen"].ToString()
                            };
                            SystemPackages.Set(PackageToSave);
                            if (SystemPackages.Error_Message != "")
                            {
                                throw new Exception(SystemPackages.Error_Message);
                            }
                            SystemPackages.Update(PackageToSave);
                            if (SystemPackages.Error_Message != "")
                            {
                                throw new Exception(SystemPackages.Error_Message);
                            }
                            var pakageResult = SystemPackages.GetById(PackageToSave.WH, PackageToSave.tracking_code, PackageToSave.usersId);
                            if (pakageResult != null)
                            {
                                string Message = "";
                                UsersManager user = new UsersManager();
                                var singleUser = user.GetUsers(PackageToSave.usersId);
                                if (pakageResult.ValorMercancia <= 0 && singleUser.IsReseller == 0 && pakageResult.status_code == 7)
                                {
                                    Message = "Debe enviarnos adjunto por su perfil en el sistema la factura relacionada a este paquete de lo contrario será retenido por Aduana y no podremos entregarle hasta que la factura nos sea enviada.";
                                    string body = System.IO.File.ReadAllText(RootUrl + "/" + ConfigurationManager.AppSettings["TemplateForSystemsAlerts"].ToString());
                                    body = string.Format(body, singleUser.name + " " + singleUser.last_name, pakageResult.tracking_code, pakageResult.contenido, pakageResult.peso, pakageResult.ValorMercancia, pakageResult.status_description, Message, pakageResult.Moneda);
                                    SendEmail("Notificación Estado De Paquetes De IntelliPaq ", singleUser.email, body, true);
                                }
                                else
                                {
                                    string body = System.IO.File.ReadAllText(RootUrl + "/TemplateNotificationPackageWithPrices.html");
                                    body = string.Format(body, singleUser.name + " " + singleUser.last_name, pakageResult.tracking_code, pakageResult.contenido, pakageResult.peso, pakageResult.total);
                                    SendEmail("Notificación Estado De Paquetes De IntelliPaq ", singleUser.email, body, true);
                                }
                            }
                        }

                        path = Path.Combine(RootUrl + "/", fileName);
                        try
                        {
                            System.IO.File.Delete(path);
                        }
                        catch
                        { }
                    }
                }
                catch (Exception ex)
                {
                    ViewBag.ErrorMessage = "Ha Ocurrido un Error: " + ex.Message.ToString();
                }

                ViewBag.Message = "Proceso Completado Exitosamente";
                return View();
            }
            catch (Exception ex)
            {
                LogError("Billing Carga De Precios", ex.Message);
                ViewBag.Message = "Ocurrio un error: Intentelo mas tarde";
            }
            return 
                View();
        }

        [Authorize]
        //[RequireHttps]
        public ActionResult ExportPackageInventory()
        {
            try
            {
                getCookies();
                var SystemPackages = new PackagesManager();
                //Load Data
                var dataInventory = SystemPackages.GetUnProccesed(userIdLogged);
                string xml = String.Empty;
                XmlDocument xmlDoc = new XmlDocument();

                XmlSerializer xmlSerializer = new XmlSerializer(dataInventory.GetType());

                using (MemoryStream xmlStream = new MemoryStream())
                {
                    xmlSerializer.Serialize(xmlStream, dataInventory);
                    xmlStream.Position = 0;
                    xmlDoc.Load(xmlStream);
                    xml = xmlDoc.InnerXml;
                }

                var fName = string.Format("Inventory-{0}", DateTime.Now.ToString("s"))+".xls";

                byte[] fileContents = System.Text.Encoding.UTF8.GetBytes(xml);

                return File(fileContents, "application/vnd.ms-excel", fName);
            }
            catch (Exception ex)
            {
                LogError("Billing Descarga Paquetes Pendientes", ex.Message);
            }
            return Content("Error...");
        }
        [Authorize]
        //[RequireHttps]
        public ActionResult ExportToExcelRetained()
        {
            try
            {
                getCookies();
                var SystemPackages = new PackagesManager();
                //Load Data
                var dataInventory = SystemPackages.GetReteinedUnproccesesFinished(userIdLogged);


                var grid = new GridView();
                grid.DataSource = dataInventory;
                grid.DataBind();

                Response.ClearContent();
                Response.Buffer = true;
                Response.AddHeader("content-disposition", "attachment; filename=InventoryFile.xls");
                Response.ContentType = "application/ms-excel";

                Response.Charset = "";
                StringWriter sw = new StringWriter();
                HtmlTextWriter htw = new HtmlTextWriter(sw);

                grid.RenderControl(htw);

                Response.Output.Write(sw.ToString());
                Response.Flush();
                Response.End();
            }
            catch (Exception ex)
            {
                LogError("Billing Paquetes", ex.Message);
                ViewBag.Message = "Ha ocurrido un error: Informar al Administrador";
            }

            return View("UploadPrices");
        }
        [Authorize]
        //[RequireHttps]
        public ActionResult ExportToExcel()
        {
            try
            {
                getCookies();
                var SystemPackages = new PackagesManager();
                //Load Data
                var dataInventory = SystemPackages.GetUnProccesed(userIdLogged);


                var grid = new GridView();
                grid.DataSource = dataInventory;
                grid.DataBind();

                Response.ClearContent();
                Response.Buffer = true;
                Response.AddHeader("content-disposition", "attachment; filename=InventoryFile.xls");
                Response.ContentType = "application/ms-excel";

                Response.Charset = "";
                StringWriter sw = new StringWriter();
                HtmlTextWriter htw = new HtmlTextWriter(sw);

                grid.RenderControl(htw);

                Response.Output.Write(sw.ToString());
                Response.Flush();
                Response.End();
            }
            catch(Exception ex)
            {
                LogError("Billing Paquetes", ex.Message);
                ViewBag.Message = "Ha ocurrido un error: Informar al Administrador";
            }

            return View("UploadPrices");
        }
        [HttpPost]
        [Authorize]
        //[RequireHttps]
        public ActionResult UploadPackages(string messages)
        {
            string tracking = "";
            getCookies();
            try
            {
                UsersManager user = new UsersManager();
                var listUsers = user.GetUsers();
                foreach (var singleUser in listUsers)
                {
                    if (singleUser != null && !string.IsNullOrEmpty(singleUser.ID))
                    {
                        var packagesFromSource = new PackagesFromSourceManagers();
                        tracking = singleUser.ID.Trim().Replace("-", "");
                        var packagesRegistered = packagesFromSource.GetPackagesFromSource(singleUser.ID.Trim().Replace("-", ""));
                        foreach (var SinglePackages in packagesRegistered)
                        {
                            if (SinglePackages != null)
                            {
                                var SystemPackages = new PackagesManager();
                                var PackageToSave = SystemPackages.GetById(SinglePackages.fpkuno,SinglePackages.fpktck, userIdLogged);
                                if (PackageToSave == null || string.IsNullOrEmpty(PackageToSave.WH))
                                {
                                    PackageToSave = new Packages()
                                    {
                                        usersId = singleUser.usersId,
                                        courierId = singleUser.CourierId,
                                        correo = singleUser.email,
                                        tracking_code = SinglePackages.fpktck,
                                        peso = SinglePackages.fpklb,
                                        WH = SinglePackages.fpkuno,
                                        status_code = 6,
                                        consignado = SinglePackages.cnombrec,
                                        contenido = SinglePackages.fpkdes,
                                        tienda = SinglePackages.fpksup,
                                        origen = SinglePackages.nac_desc,
                                        workflowid = 0,
                                        manejo = 0,
                                        costoXLibra = 0,
                                        packageStatus = 0,
                                        packageStatusFromSourceDesc = SinglePackages.pksdesc,
                                        ValorMercancia = SinglePackages.fpkval,
                                        SeguroMonto = 0,
                                        Moneda = SinglePackages.mon_desc
                                    };
                                    string Message = "";
                                    if (PackageToSave.ValorMercancia <= 0 && singleUser.IsReseller == 0)
                                    {
                                        PackageToSave.status_code = 7;
                                        Message = "Debe enviarnos adjunto por su perfil en el sistema la factura relacionada a este paquete de lo contrario será retenido por Aduana y no podremos entregarle hasta que la factura nos sea enviada.";
                                    }                                    
                                    string body = System.IO.File.ReadAllText(RootUrl + "/" + ConfigurationManager.AppSettings["TemplateForSystemsAlerts"].ToString());
                                    body = string.Format(body, singleUser.name + " " + singleUser.last_name, PackageToSave.tracking_code, PackageToSave.contenido, PackageToSave.peso, PackageToSave.ValorMercancia, PackageToSave.packageStatusFromSourceDesc, Message, PackageToSave.Moneda);
                                    SendEmail("Notificación Estado De Paquetes De IntelliPaq ", singleUser.email, body, true);
                                }
                                else
                                {
                                    PackageToSave.packageStatusFromSourceDesc = SinglePackages.pksdesc;
                                }
                                
                                SystemPackages.Set(PackageToSave);
                            }
                        }
                    }
                }
                ViewBag.Message = "Proceso Completado Exitosamente";
                return View( );
            }
            catch (Exception ex)
            {
                LogError("Billing Paquetes", ex.Message);
                ViewBag.Message = "Ha ocurrido un error: Informar al Administrador";
            }
            return View();
        }
    }
}