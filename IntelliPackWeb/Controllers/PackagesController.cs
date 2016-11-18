using IntelliPack.DataAccessLayer.DataManagers;
using IntelliPack.DataAccessLayer.Models;
using IntelliPackWeb.Base;
using OfficeOpenXml;
using SelectPdf;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace IntelliPackWeb.Controllers
{
    public class PackagesController : BaseController
    {
        private int precioMax = 170;
        private int precioMin = 0;
        [Authorize]
        [HttpPost]
        //[RequireHttps]
        public ActionResult Add(Packages model)
        {
            try
            {
                if (model != null && (model.CourierCharge < precioMin ))//|| model.precioXLibraCliente > precioMax))
                {
                    throw new Exception("No puede asignar cargo Courier menor a " + precioMin.ToString()); //+ " ni mayor a " + precioMax.ToString() + ".");
                }
                if (model != null && string.IsNullOrEmpty(model.tracking_code))
                {
                    model.tracking_code = "";// Guid.NewGuid().ToString();
                }
                if (model != null && string.IsNullOrEmpty(model.Comments))
                {
                    model.Comments = "";
                }
                getCookies();
                PackagesManager manager = new PackagesManager();
                model.total = model.total_courier + model.CourierCharge;
                model.packageStatusFromSourceDesc = "";
                // Si el paquete se pago, entonces marcar como entregado
                if (model.status_code == 1)
                {
                    model.packageStatus = 4;
                }
                else if (model.status_code == 5)
                {
                    string body = System.IO.File.ReadAllText(RootUrl + "/" + ConfigurationManager.AppSettings["FileReturnToAdmin"].ToString());
                    body = string.Format(body, model.CourierName, model.tracking_code, model.Comments);
                    SendEmail(
                                ConfigurationManager.AppSettings["PaqueteDevueltoPorSucursalSubject"].ToString(),
                                ConfigurationManager.AppSettings["SmtpFrom"].ToString(),
                                body,
                                true);
                }
                manager.Set(model);
                manager.UpdateFinalCostumer(model);
                
                ViewBag.Success = "Datos Actualizados Satisfactoriamente";
                return Content("Datos Actualizados Satisfactoriamente");
            }
            catch (Exception ex)
            {
                ViewBag.Error = ex.Message;
            }
            return Content(ViewBag.Error);
        }
        [Authorize]
        //[RequireHttps]
        public ActionResult DownloaadInvoice(string url)
        {
            // instantiate the html to pdf converter 
            HtmlToPdf converter = new HtmlToPdf();
            var fileName = Guid.NewGuid()+".pdf";
            // convert the url to pdf 
            PdfDocument doc = converter.ConvertUrl(url);
            string InvoiceUrl = Path.Combine(RootUrl + "/Invoice", fileName);
            // save pdf document 
            doc.Save(InvoiceUrl);

            // close pdf document 
            doc.Close();

            byte[] fileBytes = System.IO.File.ReadAllBytes(InvoiceUrl);

            return File(fileBytes, System.Net.Mime.MediaTypeNames.Application.Octet, fileName);
        }
        [Authorize]
        [HttpPost]
        //[RequireHttps]
        public ActionResult UploadInvoice(Packages model)
        {
            try
            {               
                getCookies();
                PackagesManager manager = new PackagesManager();
                // Si el paquete se pago, entonces marcar como entregado
                string path = "";
                for (int i = 0; i < Request.Files.Count; i++)
                {
                    var file = Request.Files[i];

                    var extension = Path.GetExtension(file.FileName).Replace(".", "");
                    var fileName = Guid.NewGuid().ToString() + "." + extension;
                    model.Factura = fileName;

                    path = Path.Combine(RootUrl + "/facturas", fileName);
                    try
                    {
                        System.IO.File.Delete(path);
                    }
                    catch
                    { }
                    /// Guardando el archivo
                    if (ConfigurationManager.AppSettings["ExtensionsAllowed"].ToString().ToLower().Contains(extension.ToLower()))
                    {
                        try
                        {
                            file.SaveAs(path);
                            manager.UpdateInvoice(model);
                            ViewBag.Message = "Factura Cargada Satisfactoriamente";
                        }
                        catch (Exception ex) { throw ex; }
                    }
                    else
                    {
                        try
                        {
                            System.IO.File.Delete(path);
                        }
                        catch
                        { }
                        ViewBag.Message = "Ha Ocurrido un Error: Formato de archivo invalido, deben ser: " + ConfigurationManager.AppSettings["ExtensionsAllowed"].ToString().ToLower();
                    }
                }
                
            }
            catch (Exception ex)
            {
                ViewBag.Message = "Error: "+ex.Message;
            }
            return Json(ViewBag.Message, JsonRequestBehavior.AllowGet);
        }
        
        [Authorize]
        //[RequireHttps]
        public ActionResult FillEntregaDrp(int CourierId)
        {
            getCookies();
            PackagesManager pk = new PackagesManager();
            var result = pk.GetEntregasSecuenciaByCourier(userIdLogged,CourierId);
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        [Authorize]
        //[RequireHttps]
        public ActionResult InvoceManager(string Id, int userId)
        {
            getCookies();
            PackagesManager pk = new PackagesManager();
            if (string.IsNullOrEmpty(Id))
            {
                Id = "";
            }
            if (Id != null)
            {
                var result = pk.GetListById(Id, userIdLogged);
                if (result == null || result.Count() <= 0)//result.WH == null || result.WH.Trim() == "")
                {
                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    return View(result);
                }
            }
            else
            {
                var result = pk.GetByUsersIdPk(userId, userIdLogged);
                if (result == null || result.Count() <= 0)
                {
                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    return View(result);
                }
            }
           
        }
        [Authorize]
        //[RequireHttps]
        public ActionResult InvoceManagerInfo(string Id, string tracking, int userId)
        {
            getCookies();
            PackagesManager pk = new PackagesManager();
            if (string.IsNullOrEmpty(tracking))
            {
                tracking = "";
            }
            if (string.IsNullOrEmpty(Id))
            {
                Id = "";
            }
            if (Id != null)
            {
                var result = pk.GetListById(Id, tracking, userIdLogged);
                if (result == null || result.Count() <= 0)//result.WH == null || result.WH.Trim() == "")
                {
                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    return View("InvoceManager", result);
                }
            }
            else
            {
                var result = pk.GetByUsersIdPk(userId, userIdLogged);
                if (result == null || result.Count() <= 0)
                {
                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    return View(result);
                }
            }

        }
        [Authorize]
        //[RequireHttps]
        public ActionResult CourierListPackages(int CourierId)
        {
            getCookies();
            PackagesManager pk = new PackagesManager();
            var result = pk.GetByUserId( userIdLogged, CourierId);
            if (result == null || result.Count == 0)
            {
                ViewBag.NotPackage = "1";
                return RedirectToAction("PackageById", "Packages", new { CustId = userIdLogged});
            }
            else
            {
                return View(result);
            }            
        }
        [Authorize]
        //[RequireHttps]
        public ActionResult GeneratePrintListPackages(int courierId, string TipoFact, string Comprobante)
        {
            getCookies();
            PackagesManager pk = new PackagesManager();
            var result = pk.ApplyUserDelivery(userIdLogged, courierId, TipoFact, Comprobante);
            if (result == null || result.Count == 0)
            {
                return RedirectToAction("Index", "Home");
            }
            else
            {
                UsersManager user = new UsersManager();

                foreach (var PackageToSave in result)
                {
                    var singleUser = user.GetUsers(PackageToSave.usersId);
                    string body = System.IO.File.ReadAllText(RootUrl + "/TemplatesForSystemsAlertsFinal.html");
                    body = string.Format(body, singleUser.name + " " + singleUser.last_name, PackageToSave.tracking_code, PackageToSave.contenido, PackageToSave.peso, PackageToSave.total);
                    SendEmail("Notificación Estado De Paquetes De IntelliPaq ", singleUser.email, body, true);
                }
                return View("InvoiceSubAgent", result);
            }
        }
        [Authorize]
        //[RequireHttps]
        public ActionResult PrintListPackages(int CourierId, int FechaEntragaId)
        {
            getCookies();
            PackagesManager pk = new PackagesManager();
            var result = pk.GetEntregasByCourierEntregaId(userIdLogged, CourierId, FechaEntragaId);
            if (result == null || result.Count == 0)
            {
                return RedirectToAction("Index", "Home");
            }
            else
            {
                return View("InvoiceSubAgent", result);
            }
        }
        [Authorize]
        //[RequireHttps]
        public ActionResult Delete(Packages model)
        {
            try
            {
                getCookies();
                PackagesManager manager = new PackagesManager();
                /// Status de eliminado
                model.status_code = 2;
                manager.Set(model);

                ViewBag.Success = "Datos Eliminado Satisfactoriamente";
                return Content("Datos Eliminado Satisfactoriamente");
            }
            catch (Exception ex)
            {
                ViewBag.Error = ex.Message;
            }

            return Content(ViewBag.Error);
        }
        [Authorize]
        //[RequireHttps]
        public ActionResult PackageActived()
        {
            getCookies();
            PackagesManager manager = new PackagesManager();
            var result = manager.GetActived();
            ViewBag.PackageTitles = "Paquetes Activos Clientes";
            return View("Packages", result);
        }
        
            [Authorize]
        //[RequireHttps]
        public ActionResult ReturnPackages()
        {
            getCookies();
            PackagesManager manager = new PackagesManager();
            var result = manager.GetRetornedPackages();
            ViewBag.PackageTitles = "Paquetes Devueltos Por Sub-Agentes";
            return View(result);
        }
        [Authorize]
        //[RequireHttps]
        public ActionResult ReceivePackages(string Id)
        {
            getCookies();
            PackagesManager manager = new PackagesManager();
            var result = manager.GetById(Id, userIdLogged);
            ViewBag.PackageTitles = "Paquetes Recibido Del Sub-Agente";
            return View("Packages", result);
        }

        [Authorize]
        //[RequireHttps]
        [HttpPost]
        public ActionResult ReceivePackages(Packages model)
        {
            try
            {
                //if (model != null && (model.precioXLibraCliente < 130 || model.precioXLibraCliente > 155))
                //{
                //    throw new Exception("No puede asignar costos de libras menor a 130 ni mayor a 155.");
                //}
                if (model != null && (model.CourierCharge < precioMin))//|| model.precioXLibraCliente > precioMax))
                {
                    throw new Exception("No puede asignar cargo Courier menor a " + precioMin.ToString()); //+ " ni mayor a " + precioMax.ToString() + ".");
                }
                getCookies();
                PackagesManager manager = new PackagesManager();
                model.status_code = 0;
                manager.ReturnPackage(model);

                ViewBag.Success = "Datos Actualizados Satisfactoriamente";
                return Content("Datos Actualizados Satisfactoriamente");
            }
            catch (Exception ex)
            {
                ViewBag.Error = ex.Message;
            }

            return Content(ViewBag.Error);
        }
        [Authorize]
        //[RequireHttps]
        public ActionResult PackageinActived()
        {
            getCookies();
            PackagesManager manager = new PackagesManager();
            var result = manager.GetinActived();
            ViewBag.PackageTitles = "Paquetes inActivos Clientes";
            return View("PackagesHistoricos", result);
        }
        [Authorize]
        //[RequireHttps]
        public ActionResult PackageById(int CustId)
        {
            getCookies();
            if (userIdLogged != CustId)
            {
                CustId = userIdLogged;
            }
            PackagesManager manager = new PackagesManager();
            var result = manager.GetUserId(CustId);
            ViewBag.PackageTitles = "Paquetes Activos Cliente";
            return View("Packages", result);
        }
        [Authorize]
        //[RequireHttps]
        public ActionResult PackageByIdHistorico(int CustId)
        {
            getCookies();
            if (userIdLogged != CustId)
            {
                CustId = userIdLogged;
            }
            PackagesManager manager = new PackagesManager();
            var result = manager.GetHistoryUserId(CustId);
            ViewBag.PackageTitles = "Paquetes Historicos";
            return View("PackagesHistoricos", result);
        }
        [Authorize]
        //[RequireHttps]
        public ActionResult Get(string Id, string tracking, string partial_view)
        {
            getCookies();
            PackagesManager manager = new PackagesManager();
            if (string.IsNullOrEmpty(tracking))
            {
                tracking = "";
            }
            var result = manager.GetById(Id, tracking, userIdLogged);
            return PartialView(partial_view, result);
        }
        [Authorize]
        //[RequireHttps]
        public ActionResult SeleccionarCourier()
        {
            getCookies();
            ViewBag.Couriers = GetDrpCourier();
            return View();
        }
        [Authorize]
        //[RequireHttps]
        public ActionResult SeleccionarCourierEntregas()
        {
            getCookies();
            ViewBag.Couriers = GetDrpCourier();
            return View();
        }
        
        [Authorize]
        //[RequireHttps]
        public ActionResult ApplyPagos(int no_id)
        {
            getCookies();
            var model = new Pagos() { no_id = no_id };
            PagosManager pagos = new PagosManager();
            pagos.HacerPago(model, userIdLogged);
            return View(model);
        }

        [HttpPost]
        [Authorize]
        //[RequireHttps]
        public ActionResult ApplyPagos(Pagos model)
        {
            getCookies();
            PagosManager pagos = new PagosManager();
            pagos.HacerPago(model, userIdLogged);
            return RedirectToAction("ModuloPagoCouriers");
        }
        [Authorize]
        //[RequireHttps]
        public ActionResult ModuloPagoCouriers()
        {
            getCookies();
            CuotasManager cuotas = new CuotasManager();
            var result = cuotas.GetCuotas(userIdLogged);
                   
            return View(result);
        }
        [Authorize]
        //[RequireHttps]
        public ActionResult RunCuotas()
        {
            getCookies();
            CuotasManager cuotas = new CuotasManager();
            cuotas.CorrerPagos(userIdLogged);
            return RedirectToAction("ModuloPagoCouriers");            
        }
        
        // GET: Packages
        [Authorize]
        //[RequireHttps]
        public ActionResult PackageUploadBatch()
        {
            try
            {
                getCookies();
                if (Request != null)
                {
                    HttpPostedFileBase file = Request.Files["UploadedFile"];
                    if ((file != null) && (file.ContentLength >= 0) && !string.IsNullOrEmpty(file.FileName))
                    {
                        string fileName = file.FileName;
                        string fileContentType = file.ContentType;
                        byte[] fileBytes = new byte[file.ContentLength];
                        var data = file.InputStream.Read(fileBytes, 0, Convert.ToInt32(file.ContentLength));
                        var packagesList = new List<Packages>();

                        using (var package = new ExcelPackage(file.InputStream))
                        {
                            var currentSheet = package.Workbook.Worksheets;
                            var workSheet = currentSheet.First();
                            var noOfCol = workSheet.Dimension.End.Column;
                            var noOfRow = workSheet.Dimension.End.Row;

                            if (noOfRow >= 2)
                            {
                                PackagesManager pk = new PackagesManager();
                                var result = pk.SetWorkFlow();
                                int workflowId = 0;
                                if (result != null)
                                {
                                    workflowId = result.workflowid;
                                }
                                if (workflowId > 0)
                                {
                                    for (int rowIterator = 2; rowIterator <= noOfRow; rowIterator++)
                                    {
                                        if (workSheet.Cells[rowIterator, 1].Value != null &&
                                            workSheet.Cells[rowIterator, 1].Value.ToString().Trim() != "")
                                        {
                                            var packageInfo = new Packages();
                                            packageInfo.tracking_code = workSheet.Cells[rowIterator, 1].Value.ToString();
                                            packageInfo.correo = workSheet.Cells[rowIterator, 2].Value.ToString();
                                            decimal peso = 0;
                                            decimal.TryParse(workSheet.Cells[rowIterator, 3].Value.ToString(), out peso);
                                            packageInfo.peso = peso;
                                            packageInfo.workflowid = workflowId;
                                            packageInfo.WH = workSheet.Cells[rowIterator, 4].Value.ToString();
                                            packageInfo.consignado = workSheet.Cells[rowIterator, 5].Value.ToString();
                                            packageInfo.contenido = workSheet.Cells[rowIterator, 6].Value.ToString();
                                            packageInfo.tienda = workSheet.Cells[rowIterator, 7].Value.ToString();
                                            packageInfo.origen = workSheet.Cells[rowIterator, 8].Value.ToString();
                                            decimal manejo = 0;
                                            decimal.TryParse(workSheet.Cells[rowIterator, 9].Value.ToString(), out manejo);
                                            packageInfo.manejo = manejo;
                                            decimal costoXlibra = 0;
                                            decimal.TryParse(workSheet.Cells[rowIterator, 10].Value.ToString(), out costoXlibra);
                                            packageInfo.manejo = costoXlibra;
                                            packagesList.Add(packageInfo);
                                            pk.Set(packageInfo);
                                        }                                        
                                    }
                                }                                
                            }
                        }
                        Set_Message("Datos Guardados Correctamente");
                    }
                }
                
                return View();
            }
            catch (Exception ex)
            {
                Set_Message("Ha Ocurrido Un Error: " + ex.Message);
            }
            return View();
        }        
    }
}