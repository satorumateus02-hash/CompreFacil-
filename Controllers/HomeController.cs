using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using HelpDeskMvc.Models;

namespace HelpDeskMvc.Controllers;

public class HomeController : Controller
{
    public IActionResult Index()
    {
        if (TempData.ContainsKey("Nome"))
            ViewBag.Nome = TempData["Nome"];
        if (TempData.ContainsKey("Usuario"))
            ViewBag.Usuario = TempData["Usuario"];

        return View();
    }

    public IActionResult Privacy()
    {
        return View();
    }

    public IActionResult Login()
    {
        return View();
    }

    // GET: exibe a mesma view de Login/Cadastro
    public IActionResult Register()
    {
        return View("Register");
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Login(HelpDeskMvc.Models.LoginViewModel model)
    {
        var errors = new List<string>();
        if (string.IsNullOrWhiteSpace(model.Usuario))
            errors.Add("Usuário é obrigatório.");
        if (string.IsNullOrWhiteSpace(model.Senha))
            errors.Add("Senha é obrigatória.");

        if (errors.Count > 0)
        {
            TempData["LoginErrors"] = string.Join(" ", errors);
            return View();
        }

        // Em implementação simples, aceitamos qualquer usuário/senha não vazios.
        HttpContext.Session.SetString("Usuario", model.Usuario ?? "");
        TempData["Nome"] = model.Usuario;

        return RedirectToAction("Index");
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Register(HelpDeskMvc.Models.RegisterViewModel model)
    {
        var errors = new List<string>();

        if (string.IsNullOrWhiteSpace(model.Usuario))
            errors.Add("Usuário é obrigatório.");
        if (string.IsNullOrWhiteSpace(model.Senha))
            errors.Add("Senha é obrigatória.");
        if (model.Senha != model.ConfirmarSenha)
            errors.Add("As senhas não coincidem.");
        if (string.IsNullOrWhiteSpace(model.Email))
            errors.Add("Email é obrigatório para recuperação.");

        if (errors.Count > 0)
        {
            TempData["RegErrors"] = string.Join(" ", errors);
            return View("Register");
        }

        // Implementação básica: normalmente aqui salvaríamos o usuário em um banco.
        HttpContext.Session.SetString("Usuario", model.Usuario ?? "");
        TempData["Nome"] = model.Usuario;
        TempData["Email"] = model.Email;

        return RedirectToAction("Index");
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
