using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using LanguageClasses.Data;
using LanguageClasses.Models;
using LanguageClassesWebApp.Services;
using LanguageClassesWebApp.Infrastructure;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;
using System.Runtime.ConstrainedExecution;


var builder = Microsoft.AspNetCore.Builder.WebApplication.CreateBuilder(args);

string connString = builder.Configuration.GetConnectionString("DefaultConnection");


builder.Services.AddDbContext<LanguageClassesContext>(options => options.UseSqlServer(connString));
builder.Services.AddMemoryCache();

builder.Services.AddDistributedMemoryCache();// добавляем IDistributedMemoryCache
builder.Services.AddSession();  // добавляем сервисы сессии

builder.Services.AddScoped<ICachedCoursesService, CachedCoursesService>();
builder.Services.AddScoped<ICachedListenersService, CachedListenersService>();
builder.Services.AddScoped<ICachedPaymentsService, CachedPaymentsService>();


var app = builder.Build();

app.UseSession();  // добавляем middleware для работы с сессиями

app.Map("/info", (appBuilder) =>
{
    appBuilder.Run(async (context) =>
    {
        // Формирование строки для вывода 
        string strResponse = "<HTML><HEAD><TITLE>info</TITLE></HEAD>" +
        "<META http-equiv='Content-Type' content='text/html; charset=utf-8'/>" +
        "<BODY><H1>Информация:</H1>";
        strResponse += "<BR> Сервер: " + context.Request.Host;
        strResponse += "<BR> Путь: " + context.Request.PathBase;
        strResponse += "<BR> Протокол: " + context.Request.Protocol;
        strResponse += "<BR><A href='/'>Главная</A></BODY></HTML>";
        // Вывод данных
        await context.Response.WriteAsync(strResponse);
    });
});

app.Map("/courses", (appBuilder) =>
{
    appBuilder.Run(async (context) =>
    {
        ICachedCoursesService cachedCourse = context.RequestServices.GetService<ICachedCoursesService>();
        IEnumerable<Course> courses = cachedCourse.GetCourses();
        string HtmlString = "<HTML><HEAD><TITLE>Course</TITLE></HEAD>" +
        "<META http-equiv='Content-Type' content='text/html; charset=utf-8'/>" +
        "<BODY><H1>Список курсов</H1>" +
        "<TABLE BORDER=1>"; 
        HtmlString += "<TR>";
        HtmlString += "<TH>Название курса</TH>";
        HtmlString += "<TH>программа</TH>";
        HtmlString += "<TH>Интенсивность занятий</TH>";
        HtmlString += "<TH>Количетсов часов</TH>";
        HtmlString += "<TH>Стоимость</TH>";

        HtmlString += "</TR>";
        foreach (var course in courses)
        {
            HtmlString += "<TR>";
            HtmlString += "<TD>" + course.Name + "</TD>";
            HtmlString += "<TD>" + course.Program + "</TD>";
            HtmlString += "<TD>" + course.Intensity + "</TD>";
            HtmlString += "<TD>" + course.HoursNumber + "</TD>";
            HtmlString += "<TD>" + course.Cost + "</TD>";
            HtmlString += "</TR>";
        }
        HtmlString += "</TABLE>";
        HtmlString += "<BR><A href='/'>Главная</A></BR>";
        HtmlString += "</BODY></HTML>";
        await context.Response.WriteAsync(HtmlString);
    });
});

app.Map("/courseSearch", (appBuider) =>
{
    appBuider.Run(async (context) =>
    {
        int hoursNumber;
        if (context.Session.Keys.Contains("hoursNumber"))
        {
            hoursNumber = context.Session.Get<int>("hoursNumber");
        }
        hoursNumber = Convert.ToInt32(context.Request.Query["hoursNumber"]);
        ICachedCoursesService cachedCourses = context.RequestServices.GetService<ICachedCoursesService>();
        IEnumerable<Course> courses = cachedCourses.GetCourses(20);
        string HtmlString = "<HTML><HEAD><TITLE>Курсы</TITLE></HEAD>" +
        "<META http-equiv='Content-Type' content='text/html; charset=utf-8'/>" +
        "<BODY><H1>Курсы, отобранные по количеству часов</H1>" +
        "<BODY><FORM action = '/courseSearch'/>" +
        "Площадь: <BR><INPUT type = 'text' name = 'hoursNumber' value = " + hoursNumber + ">" +
        "<BR><BR><INPUT type = 'submit' value = 'Сохранить в сессию и вывести курсы с количеством часов больше, чем заданно'></FORM>" +
        "<TABLE BORDER=1>";
        HtmlString += "<TR>";
        HtmlString += "<TH>Название курса</TH>";
        HtmlString += "<TH>программа</TH>";
        HtmlString += "<TH>Интенсивность занятий</TH>";
        HtmlString += "<TH>Количетсов часов</TH>";
        HtmlString += "<TH>Стоимость</TH>";

        HtmlString += "</TR>";
        foreach (var course in courses.Where(h => h.HoursNumber > hoursNumber))
        {
            HtmlString += "<TR>";
            HtmlString += "<TD>" + course.Name + "</TD>";
            HtmlString += "<TD>" + course.Program + "</TD>";
            HtmlString += "<TD>" + course.Intensity + "</TD>";
            HtmlString += "<TD>" + course.HoursNumber + "</TD>";
            HtmlString += "<TD>" + course.Cost + "</TD>";
            HtmlString += "</TR>";
        }
        HtmlString += "</TABLE>";
        HtmlString += "<BR><A href='/'>Главная</A></BR>";
        HtmlString += "</BODY></HTML>";
        await context.Response.WriteAsync(HtmlString);
    });
});

app.Map("/payments", (appBuilder) =>
{
    appBuilder.Run(async (context) =>
    {
        ICachedPaymentsService cachedPayment = context.RequestServices.GetService<ICachedPaymentsService>();
        IEnumerable<Payment> payments = cachedPayment.GetPayments();
        string HtmlString = "<HTML><HEAD><TITLE>Payment</TITLE></HEAD>" +
        "<META http-equiv='Content-Type' content='text/html; charset=utf-8'/>" +
        "<BODY><H1>Список платежей</H1>" +
        "<TABLE BORDER=1>";
        HtmlString += "<TR>";
        HtmlString += "<TH>Дата платежа</TH>";
        HtmlString += "<TH>Сумма</TH>";
        HtmlString += "</TR>";
        foreach (var payment in payments)
        {
            HtmlString += "<TR>";
            HtmlString += "<TD>" + payment.Date + "</TD>";
            HtmlString += "<TD>" + payment.Amount + "</TD>";
            HtmlString += "</TR>";
        }
        HtmlString += "</TABLE>";
        HtmlString += "<BR><A href='/'>Главная</A></BR>";
        HtmlString += "</BODY></HTML>";
        await context.Response.WriteAsync(HtmlString);
    });
});

app.Map("/paymentSearch", (appBuider) =>
{
    appBuider.Run(async (context) =>
    {
        double amount;
        if (context.Session.Keys.Contains("amount"))
        {
            amount = context.Session.Get<int>("amount");
        }
        amount = Convert.ToDouble(context.Request.Query["amount"]);
        ICachedPaymentsService cachedPayments = context.RequestServices.GetService<ICachedPaymentsService>();
        IEnumerable<Payment> payments = cachedPayments.GetPayments(20);
        string HtmlString = "<HTML><HEAD><TITLE>Платежи</TITLE></HEAD>" +
        "<META http-equiv='Content-Type' content='text/html; charset=utf-8'/>" +
        "<BODY><H2>Платежи с ценой выше заданной</H2>" +
        "<BODY><FORM action = '/paymentSearch'/>" +
        "Площадь: <BR><INPUT type = 'amount' name = 'amount' value = " + amount + ">" +
        "<BR><BR><INPUT type = 'submit' value = 'Сохранить в сессию и вывести платежи с суммой выше заданной'></FORM>" +
        "<TABLE BORDER=1>";
        HtmlString += "<TR>";
        HtmlString += "<TH>Дата платежа</TH>";
        HtmlString += "<TH>Сумма</TH>";

        HtmlString += "</TR>";
        foreach (var payment in payments.Where(p => p.Amount > amount))
        {
            HtmlString += "<TR>";
            HtmlString += "<TD>" + payment.Date + "</TD>";
            HtmlString += "<TD>" + payment.Amount + "</TD>";

            HtmlString += "</TR>";
        }
        HtmlString += "</TABLE>";
        HtmlString += "<BR><A href='/'>Главная</A></BR>";
        HtmlString += "</BODY></HTML>";
        await context.Response.WriteAsync(HtmlString);
    });
});

app.MapGet("/", (context) =>
{
    ICachedCoursesService cachedCourse = context.RequestServices.GetService<ICachedCoursesService>();
    cachedCourse?.AddCourses("Courses10");

    string HtmlString = "<HTML><HEAD><TITLE>language classes</TITLE></HEAD>" +
            "<META http-equiv='Content-Type' content='text/html; charset=utf-8'/>" +
            "<BODY><H1>Главная</H1>";
    HtmlString += "<H2>Language classes</H2>";
    HtmlString += "<BR><A href='/'>Главная</A></BR>";
    HtmlString += "<BR><A href='/courseSearch'>Поиск курсов</A></BR>";
    HtmlString += "<BR><A href='/paymentSearch'>Поиск по платежам</A></BR>";
    HtmlString += "<BR><A href='/courses'>Курсы</A></BR>";
    HtmlString += "<BR><A href='/payments'>Платежи</A></BR>";
    HtmlString += "<BR><A href='/info'>Информация о клиенте</A></BR>";
    HtmlString += "</BODY></HTML>";
    return context.Response.WriteAsync(HtmlString);

});

app.Run();

