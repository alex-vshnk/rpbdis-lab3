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

builder.Services.AddDistributedMemoryCache();// ��������� IDistributedMemoryCache
builder.Services.AddSession();  // ��������� ������� ������

builder.Services.AddScoped<ICachedCoursesService, CachedCoursesService>();
builder.Services.AddScoped<ICachedListenersService, CachedListenersService>();
builder.Services.AddScoped<ICachedPaymentsService, CachedPaymentsService>();


var app = builder.Build();

app.UseSession();  // ��������� middleware ��� ������ � ��������

app.Map("/info", (appBuilder) =>
{
    appBuilder.Run(async (context) =>
    {
        // ������������ ������ ��� ������ 
        string strResponse = "<HTML><HEAD><TITLE>info</TITLE></HEAD>" +
        "<META http-equiv='Content-Type' content='text/html; charset=utf-8'/>" +
        "<BODY><H1>����������:</H1>";
        strResponse += "<BR> ������: " + context.Request.Host;
        strResponse += "<BR> ����: " + context.Request.PathBase;
        strResponse += "<BR> ��������: " + context.Request.Protocol;
        strResponse += "<BR><A href='/'>�������</A></BODY></HTML>";
        // ����� ������
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
        "<BODY><H1>������ ������</H1>" +
        "<TABLE BORDER=1>"; 
        HtmlString += "<TR>";
        HtmlString += "<TH>�������� �����</TH>";
        HtmlString += "<TH>���������</TH>";
        HtmlString += "<TH>������������� �������</TH>";
        HtmlString += "<TH>���������� �����</TH>";
        HtmlString += "<TH>���������</TH>";

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
        HtmlString += "<BR><A href='/'>�������</A></BR>";
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
        string HtmlString = "<HTML><HEAD><TITLE>�����</TITLE></HEAD>" +
        "<META http-equiv='Content-Type' content='text/html; charset=utf-8'/>" +
        "<BODY><H1>�����, ���������� �� ���������� �����</H1>" +
        "<BODY><FORM action = '/courseSearch'/>" +
        "�������: <BR><INPUT type = 'text' name = 'hoursNumber' value = " + hoursNumber + ">" +
        "<BR><BR><INPUT type = 'submit' value = '��������� � ������ � ������� ����� � ����������� ����� ������, ��� �������'></FORM>" +
        "<TABLE BORDER=1>";
        HtmlString += "<TR>";
        HtmlString += "<TH>�������� �����</TH>";
        HtmlString += "<TH>���������</TH>";
        HtmlString += "<TH>������������� �������</TH>";
        HtmlString += "<TH>���������� �����</TH>";
        HtmlString += "<TH>���������</TH>";

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
        HtmlString += "<BR><A href='/'>�������</A></BR>";
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
        "<BODY><H1>������ ��������</H1>" +
        "<TABLE BORDER=1>";
        HtmlString += "<TR>";
        HtmlString += "<TH>���� �������</TH>";
        HtmlString += "<TH>�����</TH>";
        HtmlString += "</TR>";
        foreach (var payment in payments)
        {
            HtmlString += "<TR>";
            HtmlString += "<TD>" + payment.Date + "</TD>";
            HtmlString += "<TD>" + payment.Amount + "</TD>";
            HtmlString += "</TR>";
        }
        HtmlString += "</TABLE>";
        HtmlString += "<BR><A href='/'>�������</A></BR>";
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
        string HtmlString = "<HTML><HEAD><TITLE>�������</TITLE></HEAD>" +
        "<META http-equiv='Content-Type' content='text/html; charset=utf-8'/>" +
        "<BODY><H2>������� � ����� ���� ��������</H2>" +
        "<BODY><FORM action = '/paymentSearch'/>" +
        "�������: <BR><INPUT type = 'amount' name = 'amount' value = " + amount + ">" +
        "<BR><BR><INPUT type = 'submit' value = '��������� � ������ � ������� ������� � ������ ���� ��������'></FORM>" +
        "<TABLE BORDER=1>";
        HtmlString += "<TR>";
        HtmlString += "<TH>���� �������</TH>";
        HtmlString += "<TH>�����</TH>";

        HtmlString += "</TR>";
        foreach (var payment in payments.Where(p => p.Amount > amount))
        {
            HtmlString += "<TR>";
            HtmlString += "<TD>" + payment.Date + "</TD>";
            HtmlString += "<TD>" + payment.Amount + "</TD>";

            HtmlString += "</TR>";
        }
        HtmlString += "</TABLE>";
        HtmlString += "<BR><A href='/'>�������</A></BR>";
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
            "<BODY><H1>�������</H1>";
    HtmlString += "<H2>Language classes</H2>";
    HtmlString += "<BR><A href='/'>�������</A></BR>";
    HtmlString += "<BR><A href='/courseSearch'>����� ������</A></BR>";
    HtmlString += "<BR><A href='/paymentSearch'>����� �� ��������</A></BR>";
    HtmlString += "<BR><A href='/courses'>�����</A></BR>";
    HtmlString += "<BR><A href='/payments'>�������</A></BR>";
    HtmlString += "<BR><A href='/info'>���������� � �������</A></BR>";
    HtmlString += "</BODY></HTML>";
    return context.Response.WriteAsync(HtmlString);

});

app.Run();

