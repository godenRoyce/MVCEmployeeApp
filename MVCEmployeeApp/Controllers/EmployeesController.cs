using iText.Kernel.Colors;
using iText.Kernel.Geom;
using iText.Kernel.Pdf;
using iText.Layout;
using iText.Layout.Element;
using iText.Layout.Properties;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MVCEmployeeApp.Data;
using MVCEmployeeApp.Models;
using MVCEmployeeApp.Models.Domain;

namespace MVCEmployeeApp.Controllers
{
    public class EmployeesController : Controller
    {
        private readonly EmployeeAppContext employeeAppContext;

        public EmployeesController(EmployeeAppContext employeeAppContext)
        {
            this.employeeAppContext = employeeAppContext;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var employees = await employeeAppContext.Employees.ToListAsync();
            return View(employees);
        }

        [HttpGet]
        public IActionResult Add()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Add(AddEmployeeViewModel addEmployeeRequest)
        {
            var employee = new Employee()
            {
                Id = Guid.NewGuid(),
                Name = addEmployeeRequest.Name,
                Email = addEmployeeRequest.Email,
                Age = addEmployeeRequest.Age,
                JobPosition = addEmployeeRequest.JobPosition,
                City = addEmployeeRequest.City,
            };

            await employeeAppContext.Employees.AddAsync(employee);
            await employeeAppContext.SaveChangesAsync();
            return RedirectToAction("Add");
        }

        [HttpGet]
        public async Task<IActionResult> View(Guid id)
        {
            var employee = await employeeAppContext.Employees.FirstOrDefaultAsync(x => x.Id == id);

            if (employee != null)
            {
                var viewModel = new UpdateEmployeeViewModel()
                {
                    Id = employee.Id,
                    Name = employee.Name,
                    Email = employee.Email,
                    Age = employee.Age,
                    JobPosition = employee.JobPosition,
                    City = employee.City,
                };

                return await Task.Run(() => View("View", viewModel));
            }

            return RedirectToAction("Index");
        }

        [HttpPost]

        public async Task<IActionResult> View(UpdateEmployeeViewModel model)
        {
            var employee = await employeeAppContext.Employees.FindAsync(model.Id);

            if (employee != null)
            {
                employee.Name = model.Name;
                employee.Email = model.Email;
                employee.Age = model.Age;
                employee.JobPosition = model.JobPosition;
                employee.City = model.City;

                await employeeAppContext.SaveChangesAsync();

                return RedirectToAction("Index");
            }

            return RedirectToAction("Index");
        }
        [HttpPost]
        public async Task<IActionResult> Delete(UpdateEmployeeViewModel model)
        {
            var employee = await employeeAppContext.Employees.FindAsync(model.Id);

            if (employee != null)
            {
                employeeAppContext.Employees.Remove(employee);
                await employeeAppContext.SaveChangesAsync();

                return RedirectToAction("Index");
            }

            return RedirectToAction("Index");
        }

        [HttpPost]
        public async Task<IActionResult> PDF(Guid id)
        {
            var employee = await employeeAppContext.Employees.FirstOrDefaultAsync(x => x.Id == id);
            var currentRow = 1;

            if (employee != null)
            {
                

 
                String path = "..\\employee-details.pdf";
                PdfWriter writer = new PdfWriter(path);
                PdfDocument pdf = new PdfDocument(writer);
                Document document = new Document(pdf);
                Paragraph header = new Paragraph("Employee Details").SetTextAlignment(iText.Layout.Properties.TextAlignment.CENTER).SetFontSize(20);
                document.Add(header);

                Table table = new Table(2, true);
                Cell name_label = new Cell(1, 1)
                  .SetTextAlignment(TextAlignment.CENTER)
                  .Add(new Paragraph("Name"));
                Cell name_value = new Cell(1, 1)
                  .SetTextAlignment(TextAlignment.CENTER)
                  .Add(new Paragraph(employee.Name));
                Cell email_label = new Cell(1, 1)
                   .SetTextAlignment(TextAlignment.CENTER)
                   .Add(new Paragraph("Email"));
                Cell email_value = new Cell(1, 1)
                  .SetTextAlignment(TextAlignment.CENTER)
                  .Add(new Paragraph(employee.Email));
                Cell age_label = new Cell(1, 1)
                   .SetTextAlignment(TextAlignment.CENTER)
                   .Add(new Paragraph("Age"));
                Cell age_value = new Cell(1, 1)
                  .SetTextAlignment(TextAlignment.CENTER)
                  .Add(new Paragraph(employee.Age.ToString()));
                Cell job_label = new Cell(1, 1)
                   .SetTextAlignment(TextAlignment.CENTER)
                   .Add(new Paragraph("Job Position"));
                Cell job_value = new Cell(1, 1)
                  .SetTextAlignment(TextAlignment.CENTER)
                  .Add(new Paragraph(employee.JobPosition));
                Cell city_label = new Cell(1, 1)
                  .SetTextAlignment(TextAlignment.CENTER)
                  .Add(new Paragraph("City"));
                Cell city_value = new Cell(1, 1)
                  .SetTextAlignment(TextAlignment.CENTER)
                  .Add(new Paragraph(employee.City));

                table.AddCell(name_label);
                table.AddCell(name_value);
                table.AddCell(email_label);
                table.AddCell(email_value);
                table.AddCell(age_label);
                table.AddCell(age_value);
                table.AddCell(job_label);
                table.AddCell(job_value);
                table.AddCell(city_label);
                table.AddCell(city_value);


                document.Add(table);

                document.Close();

                return RedirectToAction("Index");
            }

            return RedirectToAction("Index");
        }

    }
}
